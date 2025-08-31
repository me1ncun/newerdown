using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Errors;
using NewerDown.Application.Extensions;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Domain.DTOs.Token;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Domain.Result;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Extensions;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.Application.Services;

public class SignInService : ISignInService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    private readonly ILogger<SignInService> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly IQueueSender _queueSender;
    private readonly ApplicationDbContext _context;

    public SignInService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IUserContextService userContextService,
        IMapper mapper,
        ILogger<SignInService> logger,
        SignInManager<User> signInManager,
        IQueueSenderFactory queueSenderFactory,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userContextService = userContextService;
        _mapper = mapper;
        _logger = logger;
        _signInManager = signInManager;
        _queueSender = queueSenderFactory.Create(QueueType.Emails.GetQueueName());
        _context = context;
    }

    public async Task<Result<TokenDto>> LoginUserAsync(LoginUserDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<TokenDto>.Failure(UserErrors.InvalidCredentials);
        }
        
        List<Claim> authClaims = [
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
        
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        authClaims.AddRange(GenerateClaims(user));
        
        var token = _tokenService.GenerateAccessToken(authClaims);
        
        string refreshToken = _tokenService.GenerateRefreshToken();
        
        var tokenInfo = _context.TokenInfos.FirstOrDefault(a => a.Username == user.UserName);
        if (tokenInfo == null)
        {
            var ti = new TokenInfo
            {
                Username = user.UserName,
                RefreshToken = refreshToken,
                ExpiredAt = DateTime.UtcNow.AddDays(7)
            };
            _context.TokenInfos.Add(ti);
        }
        else
        {
            tokenInfo.RefreshToken = refreshToken;
            tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
        }
        
        await _context.SaveChangesAsync();

        return Result<TokenDto>.Success(new TokenDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken
        });
    }

    public async Task<Result> SignUpUserAsync(RegisterUserDto request)
    {
        var user = _mapper.Map<User>(request);
        
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {email} already exists.", request.Email);
            return Result.Failure(UserErrors.AlreadyExists);
        }

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var exceptions = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User {name} registration failed: {errors}", request.UserName, exceptions);
            
            return Result.Failure(UserErrors.RegistrationFailed);
        }

        _logger.LogInformation("User {name} registered successfully.", request.UserName);
        
        await _signInManager.SignInAsync(user, isPersistent: false);
        await _userManager.AddToRoleAsync(user, nameof(RoleType.Administrator));
        
        var email = new EmailDto(user.Email, user.UserName, DateTime.UtcNow);
        await _queueSender.SendAsync(email, sessionId: email.Id);
        
        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordDto request)
    {
        var userId = _userContextService.GetUserId();
        var user = (await _userManager.FindByIdAsync(userId.ToString())).ThrowIfNull(nameof(User));

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Result.Failure(UserErrors.PasswordChangeError);
        }
        
        return Result.Success();
    }
    
    public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var username = principal.Identity?.Name;

        var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
        if (tokenInfo == null 
            || tokenInfo.RefreshToken != tokenDto.RefreshToken 
            || tokenInfo.ExpiredAt <= DateTime.UtcNow)
        {
            throw new InvalidAccessException("Invalid refresh token. Please login again.");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        tokenInfo.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync();

        return new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    
    private static List<Claim> GenerateClaims(User user)
    {
        return
        [
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        ];
    }
}