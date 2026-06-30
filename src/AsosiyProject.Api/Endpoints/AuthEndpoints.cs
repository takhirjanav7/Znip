using AsosiyProject.Application.DTOs.GoogleDTOs;
using AsosiyProject.Application.DTOs.SignInDtos;
using AsosiyProject.Application.Services.SignUp.Registration;

namespace AsosiyProject.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth").WithTags("Auth");

        // ---------------------------
        // Register
        // ---------------------------
        authGroup.MapPost("/register", async (RegisterUserDto dto, IAuthService authService) =>
        {
            var userId = await authService.SignUpUserAsync(dto);
            return Results.Ok(new { UserId = userId });
        });

        // ---------------------------
        // Login
        // ---------------------------
        authGroup.MapPost("/login", async (LoginUserDto dto, IAuthService authService) =>
        {
            var result = await authService.LoginUserAsync(dto);
            return Results.Ok(result);
        });

        // ---------------------------
        // Google Register
        // ---------------------------
        authGroup.MapPost("/google/register", async (GoogleAuthDto dto, IAuthService authService) =>
        {
            var userId = await authService.GoogleRegisterAsync(dto);
            return Results.Ok(new { UserId = userId });
        });

        // ---------------------------
        // Google Login
        // ---------------------------
        authGroup.MapPost("/google/login", async (GoogleAuthDto dto, IAuthService authService) =>
        {
            var result = await authService.GoogleLoginAsync(dto);
            return Results.Ok(result);
        });

        // Refresh Token
        authGroup.MapPost("/refresh", async (TokenDto dto, IAuthService authService) =>
        {
            try
            {
                var result = await authService.RefreshTokenAsync(dto);
                return Results.Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
        });

        // Logout
        authGroup.MapPost("/logout", async (LogOutResultDto dto, IAuthService authService) =>
        {
            try
            {
                await authService.LogoutAsync(dto.RefreshToken);
                return Results.Ok(new { message = "Logged out successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
    }
}