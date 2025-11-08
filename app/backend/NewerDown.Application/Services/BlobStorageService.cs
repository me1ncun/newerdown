using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.File;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
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
        var blobName = $"{Guid.NewGuid()}{extension}";
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;
            await _blobContainerClient.UploadBlobAsync(blobName, stream);
        }
        
        var sasUrl = await GenerateSasUrlAsync(blobName, TimeSpan.FromMinutes(30));
        
        var fileAttachmentDto = new FileAttachmentDto()
        {
            Id = Guid.NewGuid(),
            Uri = _blobContainerClient.Uri.AbsoluteUri,
            FileName = blobName,
            ContentType = file.ContentType,
            Size = file.Length,
            CreatedAt = _timeProvider.UtcNow(),
            FilePath = sasUrl
        };
        
        var fileAttachment = _mapper.Map<FileAttachment>(fileAttachmentDto);
        _context.FileAttachments.Add(fileAttachment);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("File uploaded successfully: {FileName}", blobName);
        
        return new FileAttachmentResponseDto
        {
            FileAttachment = fileAttachmentDto,
            Status = StatusType.Success.ToString(),
            Error = false
        };
    }

    public async Task<string> GenerateSasUrlAsync(string blobName, TimeSpan validFor)
    {
        var azureStorageAccount = _configuration["AzureStorageAccount"];
        var azureStorageAccessKey = _configuration["AzureStorageAccessKey"];

        var blobClient = _blobContainerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
            throw new EntityNotFoundException($"Blob '{blobName}' not found.");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _blobContainerClient.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount, azureStorageAccessKey));

        return $"{blobClient.Uri}?{sasToken}";
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