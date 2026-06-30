namespace AsosiyProject.Application.DTOs.FollowDTOs;

public record ProfileStatsDto(
    int PostsCount,
    int ProjectsCount,
    int FollowersCount,
    int FollowingCount,
    bool IsFollowingCurrentUser,    // "Follow back" tugmasi uchun
    bool IsFollowedByCurrentUser    // "Following" holati
);