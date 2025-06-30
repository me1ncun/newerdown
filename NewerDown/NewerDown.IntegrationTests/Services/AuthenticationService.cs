using System.Net.Http.Json;
using NewerDown.Domain.DTOs.Account;

namespace NewerDown.IntegrationTests.Services;

public class AuthenticationService
{
    private readonly HttpClient _client;

    public AuthenticationService(HttpClient client)
    {
        _client = client;
    }

    public async Task CreateUser()
    {
        var request = new RegisterAccountDto()
        {
            Email = "test@example.com",
            UserName = "testuser",
            Password = "YourSecurePassword123_"
        };
        
        var response = await _client.PostAsJsonAsync("/api/account/register", request);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<string> GetJwtTokenAsync()
    {
        var loginDto = new
        {
            UserName = "testuser",
            Password = "YourSecurePassword123_"
        };

        var response = await _client.PostAsJsonAsync("/api/account/login", loginDto);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
    
    public async Task<string> GetCurrentUserIdAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/account/current");
        response.EnsureSuccessStatusCode();

        var userId = await response.Content.ReadFromJsonAsync<string>();
        return userId;
    }
}