using AsosiyProject.Application.Commands;
using AsosiyProject.Application.DTOs.GoogleDTOs;
using AsosiyProject.Application.DTOs.SignInDtos;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Domain.Entities;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AsosiyProject.Application.Services.SignUp.Registration;

public class AuthService : IAuthService
{
    // DEPENDENCY INJECTION
    private readonly IAppDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAppDbContext context, IUserRepository userRepository, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _logger = logger;
        _context = context;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> GoogleLoginAsync(GoogleAuthDto dto)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings());

        var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            await GoogleRegisterAsync(dto);
            user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);
        }

        return await GenerateTokensForUserAsync(user);
    }

    public async Task<Guid> GoogleRegisterAsync(GoogleAuthDto dto)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings());

        var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (user != null)
        {
            return user.UserId;
        }

        user = new User
        {
            UserName = payload.Email.Split('@')[0],
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            Email = payload.Email,
            EmailConfirmed = payload.EmailVerified,
            GoogleId = payload.Subject,
            GoogleProfilePictureUrl = payload.Picture,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(CancellationToken.None);
        return user.UserId;
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginUserDto userLoginDto)
    {
        var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == userLoginDto.UsernameOrEmail

                           || u.UserName == userLoginDto.UsernameOrEmail);

        if (user == null)
            throw new Exception("UserName or password incorrect");

        // Password tekshirish
        var isPasswordValid = PasswordHasher.Verify(userLoginDto.Password, user.PasswordHash, user.Salt);

        if (!isPasswordValid)
            throw new Exception("UserName or password incorrect");


        return await GenerateTokensForUserAsync(user);
    }

    public async Task<Guid> SignUpUserAsync(RegisterUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email already registered");

        if (await _context.Users.AnyAsync(u => u.UserName == dto.Username))
            throw new Exception("Username already taken");

        var tupleFromHasher = PasswordHasher.Hasher(dto.Password);

        var user = new User
        {
            FirstName = dto.FullName,
            LastName = dto.FullName,
            UserName = dto.Username,
            Email = dto.Email,
            PasswordHash = tupleFromHasher.Hash,
            Salt = tupleFromHasher.Salt,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
            GoogleId = string.Empty,
            GoogleProfilePictureUrl = string.Empty
        };

        await _userRepository.CreateAsync(user);
        await _context.SaveChangesAsync(CancellationToken.None);

        return user.UserId;
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var user = await _tokenService.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null)
            return;

        // Refresh Tokenni ma'lumotlar bazasidan o'chirish
        await _tokenService.RemoveRefreshTokenAsync(refreshToken);

        _logger.LogInformation("Foydalanuvchi tizimdan chiqdi: {Email}", user.Email);
    }

    private async Task<LoginResponseDto> GenerateTokensForUserAsync(User user)
    {
        var userTokenDto = new UserTokenDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };

        var accessToken = _tokenService.GenerateToken(userTokenDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.UserId,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync(CancellationToken.None);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 60
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(TokenDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            throw new Exception("Refresh token yuborilmadi");

        // 1️⃣ Refresh token orqali userni topamiz
        var user = await _tokenService.GetUserByRefreshTokenAsync(dto.RefreshToken);
        if (user == null)
            throw new Exception("Refresh token noto‘g‘ri yoki muddati tugagan");

        // 2️⃣ Eski refresh tokenni o‘chiramiz
        await _tokenService.RemoveRefreshTokenAsync(dto.RefreshToken);

        // 3️⃣ Yangi tokenlar yaratamiz
        var userTokenDto = new UserTokenDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };

        var newAccessToken = _tokenService.GenerateToken(userTokenDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // 4️⃣ Yangi refresh tokenni DB ga saqlaymiz
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync(CancellationToken.None);

        // 5️⃣ Javob
        return new TokenDto
        {
            RefreshToken = newRefreshToken
        };
    }
}