using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        var users = _dbContext.Users
            .Include(x => x.FileAttachment)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(await users);
    }
}