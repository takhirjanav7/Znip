    namespace AsosiyProject.Application.DTOs.SignInDtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? TokenType { get; set; }
    public int Expires { get; set; } = 24;
}