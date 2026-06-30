using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.DTOs.FollowDTOs;

public record GetFollowDto(
    Guid FollowId,
    UserSmallDto Follower,      // kim follow qildi
    UserSmallDto Following,     // kimni follow qildi
    DateTime FollowedAt
);