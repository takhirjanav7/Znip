using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;
using AsosiyProject.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace AsosiyProject.Application.Services.notificationService;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hub; // SignalR uchun

    public NotificationService(IUnitOfWork uow, IHubContext<NotificationHub> hub)
    {
        _uow = uow;
        _hub = hub;
    }

    public async Task SendAsync(Guid userId, string title, string message, string type,
        Guid actorId, Guid? postId = null, Guid? projectId = null, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ActorId = actorId,
            Title = title,
            Message = message,
            Type = type,
            PostId = postId,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _uow.Notifications.AddAsync(notification, ct);
        await _uow.SaveChangesAsync(ct);

        await _hub.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
            ActorId = notification.ActorId,
            CreatedAt = notification.CreatedAt,
            IsRead = false
        }, ct);
    }

    public async Task<List<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct = default)
        => await _uow.Notifications.GetUnreadAsync(userId, ct);

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
    {
        await _uow.Notifications.MarkAsReadAsync(notificationId, userId, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
    {
        return await _uow.Notifications.GetUnreadCountAsync(userId, ct);
    }
}