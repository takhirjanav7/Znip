namespace AsosiyProject.Application.DTOs.UserDTOs;

public record UserSmallDto(
    Guid UserId,
    string UserName,
    string FullName,
    string? ProfilePictureUrl
);