namespace AsosiyProject.Application.DTOs.NotificationDTOs;

public record MarkAsReadRequestDto(
    Guid? NotificationId = null,    // null bo‘lsa — hammasini o‘qiydi
    List<Guid>? NotificationIds = null
);

public record MarkAsReadResponseDto(
    int MarkedCount,
    int RemainingUnreadCount
);

// 4. Notification turlari (enum)
public enum NotificationType
{
    Like = 0,
    Comment = 1,
    Reply = 2,
    Follow = 3,
    Mention = 4,
    ProjectInvite = 5,
    ProjectJoinRequest = 6,
    NewMessage = 7,
    PostTagged = 8,
    Achievement = 9
}