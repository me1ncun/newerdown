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
    private readonly IUserService _userService;
    
    public ServicesService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userService = userService;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServices()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<ServiceDto>>(CacheKey);
        if (cached is not null)
            return cached;
        
        var services = await _context.Services
            .Where(x => x.UserId == _userService.GetUserId())
            .ToListAsync();
        
        var result = _mapper.Map<IEnumerable<ServiceDto>>(services);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }
    
    public async Task CreateServiceAsync(AddServiceDto serviceDto)
    {
        var service = await GetServiceByUserIdAsync(serviceDto.Name);
        if (service is not null)
            throw new EntityAlreadyExistsException(nameof(Service));
        
        _context.Services.Add(_mapper.Map<Service>(serviceDto));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateServiceAsync(Guid serviceId, UpdateServiceDto serviceDto)
    {
       var service =  await GetServiceByUserIdAsync(serviceDto.Name);
       if (service is null)
           throw new EntityNotFoundException(nameof(Service));
        
        _mapper.Map(serviceDto, service);
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
    
    private async Task<ServiceDto> GetServiceByUserIdAsync(string name)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.UserId == _userService.GetUserId() 
                                      && s.Name == name);
        
        return _mapper.Map<ServiceDto>(service);
    }
}