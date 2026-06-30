using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.DTOs.FollowDTOs;

public record NewFollowerNotificationDto(
    UserSmallDto NewFollower,
    DateTime FollowedAt
);