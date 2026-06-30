namespace AsosiyProject.Application.DTOs.UserDTOs;

public record UpdateProfileDto(
    string? FullName,
    string? Bio,
    string? ProfilePictureUrl,
    string? CoverPictureUrl
);