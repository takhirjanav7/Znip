using AsosiyProject.Api.Hubs;
using AsosiyProject.Application.Interfaces.MESSAGE;
using AsosiyProject.Application.Services.chatService;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AsosiyProject.Api.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/chat").WithTags("Chat").RequireAuthorization();

        // 1. Foydalanuvchi bilan bo'lgan chat tarixini olish (pagination bilan)
        group.MapGet("/history/{userId:guid}", async (Guid userId, int page, IChatService chatService, ClaimsPrincipal user) =>
        {
            var currentUserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var history = await chatService.GetHistory(currentUserId, userId, page);
            return Results.Ok(history);
        });

        // 2. Umumiy o'qilmagan xabarlar soni (masalan: 5)
        group.MapGet("/unread-total", async (IChatService chatService, ClaimsPrincipal user) =>
        {
            var currentUserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var count = await chatService.GetTotalUnreadCount(currentUserId);
            return Results.Ok(new { TotalUnread = count });
        });

        // 3. Har bir foydalanuvchidan qancha kelgan (masalan: { "ali-id": 2, "vali-id": 3 })
        group.MapGet("/unread-summary", async (IChatService chatService, ClaimsPrincipal user) =>
        {
            var currentUserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var summary = await chatService.GetUnreadSummary(currentUserId);
            return Results.Ok(summary);     
        });

        // 4. Ma'lum bir foydalanuvchidan kelgan xabarlarni o'qilgan deb belgilash
        group.MapPost("/read/{senderId:guid}", async (Guid senderId, IChatRepository chatRepo, IHubContext<ChatHub> hubContext, ClaimsPrincipal user) =>
        {
            var currentUserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Bazada o'qildi qilish
            await chatRepo.MarkAsReadAsync(senderId, currentUserId);

            // --- REAL-TIME SINXRONIZATSIYA ---
            // Xabar yuborgan odamga "Sening xabarlaring o'qildi" deb bildirish
            await hubContext.Clients.User(senderId.ToString()).SendAsync("MessagesSeen", currentUserId);

            // O'zimizning unread countni yangilash (SignalR orqali)
            var newTotalUnread = await chatRepo.GetTotalUnreadCountAsync(currentUserId);
            await hubContext.Clients.User(currentUserId.ToString()).SendAsync("UpdateUnreadCount", newTotalUnread);

            return Results.NoContent();
        });

        // 5. Xabarni o'chirish (faqat o'zi yuborgan xabarni)
        group.MapDelete("/message/{id:guid}", async (Guid id, IChatRepository chatRepo, IHubContext<ChatHub> hubContext, ClaimsPrincipal user) =>
        {
            var currentUserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var message = await chatRepo.GetByIdAsync(id);

            if (message == null || message.SenderId != currentUserId)
                return Results.Forbid();

            await chatRepo.DeleteAsync(message);

            // API orqali o'chirilganda ham SignalR orqali hammaga xabar beramiz
            await hubContext.Clients.User(message.ReceiverId.ToString()).SendAsync("MessageDeleted", id);

            return Results.NoContent();
        });
    }
}