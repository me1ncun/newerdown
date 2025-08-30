using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class UserService : IUserService
{
    private readonly IUserContextService _userContextService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(
        IUserContextService userContextService,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _userContextService = userContextService;
        _context = context;
        _mapper = mapper;
    }
 
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(x => x.FileAttachment)
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        return user;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = _context.Users
            .Include(x => x.FileAttachment)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(await users);
    }
    
    public async Task<UserDto> UpdateUserAsync(UpdateUserDto request)
    {
        var userId = _userContextService.GetUserId();
        var user = await GetUserByIdAsync(userId);
        if (user == null)
            throw new EntityNotFoundException("User not found.");
        
        _mapper.Map(request, user);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task DeleteUserAsync()
    {
        var userId = _userContextService.GetUserId();
        var user = _context.Users.FirstOrDefault(x => x.Id == userId);
        if (user == null)
            throw new EntityNotFoundException("User not found.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}