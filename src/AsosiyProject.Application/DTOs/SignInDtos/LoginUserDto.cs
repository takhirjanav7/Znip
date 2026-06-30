namespace AsosiyProject.Application.DTOs.SignInDtos;

public class LoginUserDto
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}