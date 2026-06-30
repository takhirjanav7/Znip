using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.DTOs.LikeDTOs;

public record NewLikeNotificationDto(
    Guid TargetId,
    string TargetType,      // "Post" yoki "Project"
    UserSmallDto Liker,
    DateTime LikedAt
);