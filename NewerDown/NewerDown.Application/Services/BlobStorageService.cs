using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.File;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlobStorageService> _logger;
    private readonly IMapper _mapper;
    private readonly IScopedTimeProvider _timeProvider;
    private readonly BlobContainerClient _blobContainerClient;
    
    public BlobStorageService(
        IConfiguration configuration,
        ApplicationDbContext context,
        ILogger<BlobStorageService> logger,
        IMapper mapper,
        IScopedTimeProvider timeProvider)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _blobContainerClient = new BlobContainerClient(_configuration["BlobConnection"], _configuration["BlobContainerName"]);
    }
    
    public async Task<FileAttachmentResponseDto> UploadFileAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;
            await _blobContainerClient.UploadBlobAsync(fileName, stream);
        }
        
        var fileAttachmentDto = new FileAttachmentDto()
        {
            Id = Guid.NewGuid(),
            Uri = _blobContainerClient.Uri.AbsoluteUri,
            FileName = fileName,
            FilePath = $"{_blobContainerClient.Uri}/{fileName}",
            ContentType = file.ContentType,
            Size = file.Length
        };
        
        var fileAttachment = _mapper.Map<FileAttachment>(fileAttachmentDto);
        fileAttachment.CreatedAt = _timeProvider.UtcNow();
        
        _context.FileAttachments.Add(fileAttachment);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
        
        return new FileAttachmentResponseDto
        {
            FileAttachment = fileAttachmentDto,
            Status = "Success",
            Error = false
        };
    }
    
    public async Task<FileAttachment> GetFileAttachmentByIdAsync(Guid? fileAttachmentId)
    {
        var fileAttachment = await _context.FileAttachments.FindAsync(fileAttachmentId);
        if (fileAttachment is null)
        {
            _logger.LogWarning("File attachment with ID {FileAttachmentId} not found.", fileAttachmentId);
            throw new EntityNotFoundException($"File attachment was not found by id: {fileAttachmentId}.");
        }
        
        return fileAttachment;
    }
    
    public async Task DeleteFileAsync(Guid? fileAttachmentId)
    {
        var fileAttachment = await GetFileAttachmentByIdAsync(fileAttachmentId);
        
        var blobClient = _blobContainerClient.GetBlobClient(fileAttachment.FileName);
        await blobClient.DeleteIfExistsAsync();
        
        _context.FileAttachments.Remove(fileAttachment);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("File with ID {FileAttachmentId} deleted successfully.", fileAttachmentId);
    }
}