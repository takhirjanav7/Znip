using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.DTOs.LikeDTOs;

public record GetLikeDto(
    Guid LikeId,
    UserSmallDto User,
    DateTime LikedAt
);