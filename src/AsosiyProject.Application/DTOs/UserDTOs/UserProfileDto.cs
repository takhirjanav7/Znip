namespace AsosiyProject.Application.DTOs.UserDTOs;

public record UserProfileDto(
    Guid Id,
    string UserName,
    string FullName,
    string? Bio,
    string? ProfilePictureUrl,
    string? CoverPictureUrl,
    DateTime JoinedDate,
    bool IsVerified,
    int FollowersCount,
    int FollowingCount,
    int PostsCount,
    int ProjectsCount
);