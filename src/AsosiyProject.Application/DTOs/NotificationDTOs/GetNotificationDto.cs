using AsosiyProject.Application.DTOs.NotificationDTOs;

namespace AsosiyProject.Application.SignUp.Registration;
public record GetNotificationsDto(
    IReadOnlyList<NotificationDto> Notifications,
    int TotalCount,
    int UnreadCount,                
    int Page,
    int PageSize,
    bool HasMore
);