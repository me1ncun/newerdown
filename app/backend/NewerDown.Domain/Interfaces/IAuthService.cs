using System.Security.Claims;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IAuthService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    
    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}