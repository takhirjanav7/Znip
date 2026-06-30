namespace AsosiyProject.Application.DTOs.LikeDTOs;

public record CreateLikeDto(
    Guid TargetId,          // PostId yoki ProjectId
    string TargetType      // "Post" yoki "Project"
);