using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.DTOs;

public record ActivityDto(
    Guid Id,
    string Type,                    // "like", "follow", "comment", "mention"
    UserSmallDto Actor,
    string Message,                 // "Ali sizning postingizga like bosdi"
    string? TargetUrl,              // "/post/123"
    DateTime CreatedAt,
    bool IsRead = false
);