using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _mapper = mapper;
    }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
        
        return Guid.TryParse(userId, out var id)
            ? id
            : throw new UnauthorizedAccessException("User is not authenticated.");
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

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var userId = GetUserId();
        var user = await GetUserByIdAsync(userId);
        
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task DeleteUserAsync()
    {
        var userId = GetUserId();
        var user = _context.Users.FirstOrDefault(x => x.Id == userId);
        if (user == null)
            throw new EntityNotFoundException("User not found.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}