using AsosiyProject.Application.DTOs.UserDTOs;
using AsosiyProject.Application.Extensions;

namespace AsosiyProject.Application.DTOs.NotificationDTOs;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    string Type,
    string TargetUrl,
    UserSmallDto? Actor,
    DateTime CreatedAt,
    bool IsRead
)
{
    public string TimeAgo => CreatedAt.ToTimeAgo();
}