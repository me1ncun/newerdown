using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Extensions;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Extensions;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.Application.Services;

public class SignInService : ISignInService
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<SignInService> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly IQueueSenderFactory _queueSenderFactory;

    public SignInService(
        UserManager<User> userManager,
        IAuthService authService,
        IUserService userService,
        IMapper mapper,
        ILogger<SignInService> logger,
        SignInManager<User> signInManager,
        IQueueSenderFactory senderFactory)
    {
        _userManager = userManager;
        _authService = authService;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
        _signInManager = signInManager;
        _queueSenderFactory = senderFactory;
    }

    public async Task<string> LoginUserAsync(LoginAccountDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidOperationException("Invalid username or password.");
        }
        
        return _authService.GenerateToken(user);
    }

    public async Task RegisterUserAsync(RegisterAccountDto request)
    {
        var user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var exceptions = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User {name} registration failed: {errors}", request.UserName, exceptions);
            
            throw new InvalidAccessException("User registration failed: " + exceptions);
        }

        _logger.LogInformation("User {name} registered successfully.", request.UserName);
        
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        var sender = _queueSenderFactory.Create(QueueType.Emails.GetQueueName());
        var email = new EmailDto(user.Email, user.UserName, DateTime.UtcNow);
        await sender.SendAsync(email, sessionId: email.Id);
    }

    public async Task ChangePasswordAsync(ChangePasswordDto request)
    {
        var userId = _userService.GetUserId();
        var user = (await _userManager.FindByIdAsync(userId.ToString())).ThrowIfNull(nameof(User));

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password change failed: {errors}");
        }
    }
}