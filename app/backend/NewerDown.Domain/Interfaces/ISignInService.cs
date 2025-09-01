using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Token;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface ISignInService
{
    Task<Result<TokenDto>> LoginUserAsync(LoginUserDto request);
    Task<Result.Result> ChangePasswordAsync(ChangePasswordDto request);
    Task<Result.Result> SignUpUserAsync(RegisterUserDto request);
    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
}