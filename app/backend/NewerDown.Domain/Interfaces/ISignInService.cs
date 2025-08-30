using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Token;

namespace NewerDown.Domain.Interfaces;

public interface ISignInService
{
    Task<TokenDto> LoginUserAsync(LoginUserDto request);
    Task ChangePasswordAsync(ChangePasswordDto request);
    Task SignUpUserAsync(RegisterUserDto request);
    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
}