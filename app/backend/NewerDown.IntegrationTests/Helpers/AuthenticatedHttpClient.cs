using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.IntegrationTests.Services;

namespace NewerDown.IntegrationTests.Helpers;

public class AuthenticatedHttpClient 
{
    private readonly AuthenticationService _authenticationService;
    private readonly HttpClient _client;

    public AuthenticatedHttpClient(HttpClient client)
    {
        _client = client;
        _authenticationService = new AuthenticationService(client);
    }
    
    public async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        await _authenticationService.CreateUser();
        var token = await _authenticationService.GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client;
    }
}