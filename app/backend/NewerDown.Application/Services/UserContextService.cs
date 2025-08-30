using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserContextService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _context = context;
    }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return Guid.TryParse(userId, out var id)
            ? id
            : throw new UnauthorizedAccessException("User is not authenticated.");
    }
    
    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var userId = GetUserId();
        var user = await _context.Users
            .Include(x => x.FileAttachment)
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
            throw new EntityNotFoundException("User not found.");
        
        return _mapper.Map<UserDto>(user);
    }
}