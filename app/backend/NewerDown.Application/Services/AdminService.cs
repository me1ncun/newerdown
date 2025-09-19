using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public AdminService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _dbContext.Users
            .Include(x => x.FileAttachment)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<IEnumerable<MonitorDto>> GetAllMonitorsAsync()
    {
        var monitors = await _dbContext.Monitors.ToListAsync();
        
        return _mapper.Map<IEnumerable<MonitorDto>>(monitors);
    }
}