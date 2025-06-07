using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Constants;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class ServicesService : IServicesService
{
    private const string CacheKey = "AllServices";
    
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    
    public ServicesService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServices()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<ServiceDto>>(CacheKey);
        if (cached is not null)
            return cached;
        
        var services = await _context.Services.ToListAsync();
        var result = _mapper.Map<IEnumerable<ServiceDto>>(services);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }
    
    public async Task CreateServiceAsync(AddServiceDto serviceDto)
    {
        var service = await GetServiceByUserIdAsync(serviceDto.UserId, serviceDto.Name);
        if (service is not null)
            throw new EntityAlreadyExistsException(nameof(Service));
        
        _context.Services.Add(_mapper.Map<Service>(serviceDto));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateServiceAsync(Guid serviceId, UpdateServiceDto serviceDto)
    {
        await GetServiceByUserIdAsync(serviceDto.UserId, serviceDto.Name);
        // do entity update
        _context.Services.Update(_mapper.Map<Service>(serviceDto));
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteServiceAsync(Guid id)
    {
        var service = _context.Services.Find(id);
        if (service is null)
            throw new EntityNotFoundException(nameof(Service));
        
        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }
    
    public async Task<ServiceDto> GetServiceByIdAsync(Guid id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (service is null)
            throw new EntityNotFoundException(nameof(Service));
        
        return _mapper.Map<ServiceDto>(service);
    }
    
    private async Task<ServiceDto> GetServiceByUserIdAsync(Guid userId, string name)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.UserId == userId 
                                      && s.Name == name);
        
        return _mapper.Map<ServiceDto>(service);
    }
}