namespace AsosiyProject.Application.DTOs.UserDTOs;
public class UpdateUserDto
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
    public string? Bio { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? Location { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
}