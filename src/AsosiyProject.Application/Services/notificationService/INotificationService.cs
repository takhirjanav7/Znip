using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.notificationService;

public interface INotificationService
{
    Task SendAsync(Guid userId, string title, string message, string type,
        Guid actorId, Guid? postId = null, Guid? projectId = null, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    Task<List<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default);
}