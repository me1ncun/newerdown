using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
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
}