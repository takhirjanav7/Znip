using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Services.notificationService;

namespace AsosiyProject.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notifications")
                       .WithTags("Notifications")
                       .RequireAuthorization();

        // 1. Foydalanuvchining o‘qilmagan bildirishnomalari
        group.MapGet("/unread", async (INotificationService notifService, ICurrentUserService currentUser) =>
        {
            if (currentUser.UserId == null)
                return Results.Unauthorized();

            var notifications = await notifService.GetUnreadAsync(currentUser.UserId.Value);
            return Results.Ok(notifications);
        });

        // 2. Bitta bildirishnomani o‘qilgan deb belgilash
        group.MapPost("/{notificationId:guid}/read", async (Guid notificationId, INotificationService notifService, ICurrentUserService currentUser) =>
        {
            if (currentUser.UserId == null)
                return Results.Unauthorized();

            await notifService.MarkAsReadAsync(notificationId, currentUser.UserId.Value);
            return Results.Ok(new { Message = "O‘qildi" });
        });

        // 3. Barcha bildirishnomalarni o‘qilgan qilish (bonus)
        group.MapPost("/mark-all-read", async (INotificationService notifService, ICurrentUserService currentUser) =>
        {
            if (currentUser.UserId == null)
                return Results.Unauthorized();

            var userId = currentUser.UserId.Value;
            var unread = await notifService.GetUnreadAsync(userId);
            foreach (var n in unread)
                await notifService.MarkAsReadAsync(n.Id, userId);

            return Results.Ok(new { Message = "Hammasi o‘qildi" });
        });

        // 4. Real-time uchun — nechta o‘qilmagan bor? (har soniyada so‘raladi)
        group.MapGet("/unread-count", async (INotificationService notifService, ICurrentUserService currentUser) =>
        {
            if (currentUser.UserId == null)
                return Results.Unauthorized();

            var count = await notifService.GetUnreadCountAsync(currentUser.UserId.Value);

            return Results.Ok(new
            {
                UnreadCount = count
            });

        });
    }
}