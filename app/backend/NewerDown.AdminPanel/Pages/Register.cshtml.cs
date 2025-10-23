using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Interfaces;

namespace NewerDown.AdminPanel.Pages;

public class Register : PageModel
{
    private readonly ISignInService _signInService;

    public Register(ISignInService signInService)
    {
        _signInService = signInService;
    }
    
    [BindProperty]
    public string Username { get; set; } = string.Empty;
    
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [TempData]
    public string? ErrorMessage { get; set; }
    
    public void OnGet() { }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var registerUserDto = new RegisterUserDto
        {
            UserName = Username,
            Email = Email,
            Password = Password
        };

        var result = await _signInService.SignUpUserAsync(registerUserDto);

        if (result.IsFailure)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        return RedirectToPage("/Login");
    }
}