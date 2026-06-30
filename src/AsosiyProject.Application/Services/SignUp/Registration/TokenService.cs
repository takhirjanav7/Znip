using AsosiyProject.Application.Commands;
using AsosiyProject.Application.DTOs.SignInDtos;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Services.Settings;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AsosiyProject.Application.Services.SignUp.Registration;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;
    private readonly IAppDbContext _context;

    public TokenService(IOptions<JwtSettings> jwtSettings, IAppDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    public string GenerateToken(UserTokenDto tokenDto)
    {
        var IdentityClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, tokenDto.UserId.ToString()),
            new Claim("UserId",tokenDto.UserId.ToString()),
            new Claim("FirstName",tokenDto.FirstName.ToString()),
            new Claim("LastName",tokenDto.LastName.ToString()),
            new Claim("UserName",tokenDto.UserName.ToString()),
            new Claim(ClaimTypes.Role,tokenDto.Role.ToString()),
            new Claim(ClaimTypes.Email,tokenDto.Email.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: IdentityClaims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: keyCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var tokenEntity = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.Token == refreshToken &&
                rt.ExpiresAt > DateTime.UtcNow);

        return tokenEntity?.User;
    }


    public async Task RemoveRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
        .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token == null)
            return;

        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailVerificationTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParametrs = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrWhiteSpace(_jwtSettings.Issuer),
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(_jwtSettings.Audience),
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            var principal = tokenHandler.ValidateToken(token, validationParametrs, out var validatedToken);

            var userIdClaim = principal.FindFirst("UserId") ?? principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user;
            }

            var emailClaim = principal.FindFirst(ClaimTypes.Email) ?? principal.FindFirst("Email");
            if (emailClaim != null && !string.IsNullOrWhiteSpace(emailClaim.Value))
            {
                var user = await _userRepository.GetByEmailAsync(emailClaim.Value);
                return user;
            }

            return null;
        }
        catch (SecurityTokenException)
        {
            return null;
        }
        catch
        {
            return null;
        }
    }
}