//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Services.messageService;
//using AsosiyProject.Application.SignUp.Registration;
//using Microsoft.AspNetCore.Mvc;

//namespace AsosiyProject.Api.Endpoints;

//public static class MessageEndpoints
//{
//    public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/api/messages")
//                       .WithTags("Messages")
//                       .RequireAuthorization();

//        // 1. YANGI XABAR YUBORISH (fayl, rasm, video, code — hammasi!)
//        group.MapPost("/", async ([FromForm] CreateMessageDto dto, IMessageService messageService, ICurrentUserService currentUser) =>
//        {
//            var messageId = await messageService.SendMessageAsync(currentUser.UserId!.Value, dto);
//            return Results.Created($"/api/messages/{messageId}", new { Id = messageId });
//        });

//        // 2. SUHBATDAGI XABARLARNI OLISH (pagination bilan)
//        group.MapGet("/conversation/{conversationId:guid}", async (
//            Guid conversationId,
//            [AsParameters] PaginationRequest request,
//            IMessageService messageService) =>
//        {
//            var messages = await messageService.GetConversationMessagesAsync(
//                conversationId, request.Skip, request.Take);
//            return Results.Ok(messages);
//        })
//        .WithName("GetConversationMessages");

//        // 3. XABARNI O‘QILGAN DEB BELGILASH
//        group.MapPost("/{messageId:guid}/read", async (
//            Guid messageId,
//            IMessageService messageService,
//            ICurrentUserService currentUser) =>
//        {
//            await messageService.MarkMessageAsReadAsync(messageId, currentUser.UserId!.Value);
//            return Results.Ok(new { Message = "Marked as read" });
//        })
//        .WithName("MarkMessageAsRead");

//        // 4. FOYDALANUVCHI UCHUN ENG OXIRGI XABARLAR (chat ro‘yxati uchun)
//        group.MapGet("/latest", async (
//            IMessageService messageService,
//            ICurrentUserService currentUser,
//            int take = 30) =>
//        {
//            var messages = await messageService.GetLatestMessagesForUserAsync(currentUser.UserId!.Value, take);
//            return Results.Ok(messages);
//        })
//        .WithName("GetLatestMessages");

//        // 5. O‘QILMAGAN XABAR BOR-YO‘QLIGI (qizil nuqta uchun)
//        group.MapGet("/has-unread", async (
//            IMessageService messageService,
//            ICurrentUserService currentUser) =>
//        {
//            var hasUnread = await messageService.HasUnreadMessagesAsync(currentUser.UserId!.Value);
//            return Results.Ok(new { HasUnread = hasUnread });
//        })
//        .WithName("HasUnreadMessages");

//        // 6. XABARNI TAHRIRLASH
//        group.MapPut("/{messageId:guid}", async (
//            Guid messageId,
//            UpdateMessageRequest request,
//            IMessageService messageService,
//            ICurrentUserService currentUser) =>
//        {
//            await messageService.EditMessageAsync(messageId, currentUser.UserId!.Value, request.NewContent);
//            return Results.Ok(new { Message = "Message updated" });
//        })
//        .WithName("EditMessage");

//        // 7. XABARNI O‘CHIRISH (faqat o‘zi yozgani)
//        group.MapDelete("/{messageId:guid}", async (
//            Guid messageId,
//            IMessageService messageService,
//            ICurrentUserService currentUser) =>
//        {
//            await messageService.DeleteMessageAsync(messageId, currentUser.UserId!.Value);
//            return Results.NoContent();
//        })
//        .WithName("DeleteMessage");
//    }
//}

//// Qo‘shimcha DTO’lar
//public record PaginationRequest(int Skip = 0, int Take = 50);
//public record UpdateMessageRequest(string NewContent);