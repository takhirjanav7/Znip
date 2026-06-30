using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.NOTIFICATION;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken ct = default);
    Task<List<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct);

    Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default);
}