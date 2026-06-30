using AsosiyProject.Application.DTOs.SignInDtos;
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.SignUp.Registration;

public interface ITokenService
{
    public string GenerateToken(UserTokenDto tokenDto);
    public string GenerateRefreshToken();
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    public Task RemoveRefreshTokenAsync(string refreshToken);
    Task<User?> GetUserByEmailVerificationTokenAsync(string token);
}