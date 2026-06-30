//using AsosiyProject.Application.Commands;
//using AsosiyProject.Application.Commands.ForMessages;
//using AsosiyProject.Application.DTOs.MessageDTOs;
//using AsosiyProject.Application.DTOs.UserDTOs;
//using AsosiyProject.Application.Queries.Chat;
//using AsosiyProject.Application.SignUp.Registration;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.SignalR;
//using System.Security.Claims;

//namespace AsosiyProject.Api.Endpoints;

//[Authorize]
//public class ChatHub : Hub
//{
//    private readonly IMediator _mediator;

//    public ChatHub(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    public override async Task OnConnectedAsync()
//    {
//        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (userId != null)
//        {
//            var conversations = await _mediator.Send(new GetUserConversationsQuery { UserId = Guid.Parse(userId) });
//            foreach (var conv in conversations)
//            {
//                await Groups.AddToGroupAsync(Context.ConnectionId, conv.ConversationId.ToString());
//            }
//        }

//        await base.OnConnectedAsync();
//    }

//    public override async Task OnDisconnectedAsync(Exception? exception)
//    {
//        await base.OnDisconnectedAsync(exception);
//    }

//    // Xabar yuborish
//    public async Task SendMessage(Guid conversationId, string content, Guid? replyToMessageId = null)
//    {
//        var senderId = Guid.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

//        var message = await _mediator.Send(new SendMessageCommand
//        {
//            ConversationId = conversationId,
//            SenderId = senderId,
//            Content = content,
//            ReplyToMessageId = replyToMessageId
//        });

//        var dto = new ReceiveMessageDto(
//            MessageId: message.Id,
//            ConversationId: conversationId,
//            Message: new GetMessageDto(
//                Id: message.Id,
//                Content: message.Content,
//                Type: message.Type,
//                SentAt: message.SentAt,
//                IsRead: false,
//                IsEdited: false,
//                IsDeleted: false,
//                Sender: new UserSmallDto
//                (
//                    senderId,
//                    Context.User!.FindFirst(ClaimTypes.Name)?.Value ?? "UserName",
//                    Context.User!.FindFirst("FullName")?.Value ?? "Full Name",
//                    Context.User!.FindFirst("ProfilePictureUrl")?.Value
//                ),

//                Files: new List<MessageFileDto>(),
//                ReadBy: new List<UserSmallDto>()
//            ),
//            NewUnreadCount: await _mediator.Send(new GetUnreadCountQuery(conversationId))
//        );

//        await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", dto);
//    }

//    // Xabar tahrirlash
//    public async Task EditMessage(Guid messageId, string newContent)
//    {
//        var message = await _mediator.Send(new EditMessageCommand
//        {
//            MessageId = messageId,
//            NewContent = newContent
//        });

//        await Clients.Group(message.ConversationId.ToString())
//                     .SendAsync("MessageEdited", message.Id, newContent);
//    }

//    // Xabar o‘chirish
//    public async Task DeleteMessage(Guid messageId)
//    {
//        var message = await _mediator.Send(new DeleteMessageCommand
//        {
//            MessageId = messageId
//        });

//        await Clients.Group(message.ConversationId.ToString())
//                     .SendAsync("MessageDeleted", message.Id);
//    }

//    // Yozmoqda indikator
//    public async Task Typing(Guid conversationId, bool isTyping)
//    {
//        var senderId = Guid.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
//        var userName = Context.User!.FindFirst(ClaimTypes.Name)?.Value ?? "User";

//        var typingDto = new TypingNotificationDto(
//            ConversationId: conversationId,
//            UserId: senderId,
//            UserName: userName,
//            ProfilePictureUrl: null,
//            IsTyping: isTyping
//        );

//        await Clients.Group(conversationId.ToString())
//                     .SendAsync("UserTyping", typingDto);
//    }

//    // Xabar o‘qildi deb belgilash
//    public async Task MarkMessageAsRead(Guid messageId)
//    {
//        var userId = Guid.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

//        var seenDto = await _mediator.Send(new MarkMessageAsReadCommand
//        {
//            MessageId = messageId,
//            UserId = userId
//        });

//        await Clients.Group(seenDto.ConversationId.ToString())
//                     .SendAsync("MessageSeen", seenDto);
//    }
//}