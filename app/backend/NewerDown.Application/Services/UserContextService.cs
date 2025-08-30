using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserContextService(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _userService = userService;
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
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            throw new EntityNotFoundException("User not found.");
        
        return _mapper.Map<UserDto>(user);
    }
}