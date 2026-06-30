using AsosiyProject.Application.Interfaces.NOTIFICATION;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.NotificationImplement;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;
    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken ct)
    {
        await _context.Notifications.AddAsync(notification, ct);
    }

    public async Task<List<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct)
        => await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync(ct);

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct)
    {
        return await _context.Notifications
        .CountAsync(n => n.UserId == userId && !n.IsRead, ct);
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct)
    {
        var notif = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId, ct);


        if (notif == null)
            return;

        notif.IsRead = true;
        //notif.CreatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
    }
}