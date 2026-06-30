using AsosiyProject.Application.DTOs.GoogleDTOs;
using AsosiyProject.Application.DTOs.SignInDtos;

namespace AsosiyProject.Application.Services.SignUp.Registration;

public interface IAuthService
{
    Task<LoginResponseDto> GoogleLoginAsync(GoogleAuthDto dto);
    Task<Guid> GoogleRegisterAsync(GoogleAuthDto dto);
    Task<Guid> SignUpUserAsync(RegisterUserDto userCreateDto);
    Task<LoginResponseDto> LoginUserAsync(LoginUserDto userLoginDto);
    Task<TokenDto> RefreshTokenAsync(TokenDto dto);
    Task LogoutAsync(string refreshToken);
}