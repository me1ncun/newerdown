using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Interfaces;

namespace NewerDown.AdminPanel.Pages;

public class Login : PageModel
{
    private readonly ISignInService _signInService;

    public Login(ISignInService signInService)
    {
        _signInService = signInService;
    }

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

        var loginDto = new LoginUserDto
        {
            Email = Email,
            Password = Password
        };

        var result = await _signInService.LoginUserAsync(loginDto);

        if (result.IsFailure)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }
        
        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(1)
        });
        
        HttpContext.Session.SetString("accessToken", result.Value.AccessToken);
        
        return RedirectToPage("/Index");
    }
}