using AsosiyProject.Application.Interfaces.MESSAGE;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Services.notificationService;
using AsosiyProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace AsosiyProject.Api.Hubs;

[Authorize] // Faqat login qilganlar ulansin!
public class ChatHub : Hub
{
    private readonly IChatRepository _chatRepository;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;

    public ChatHub(IChatRepository chatRepository, INotificationService notificationService, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _notificationService = notificationService;
    }

    // --- FOYDALANUVCHI ONLINE BO'LGANDA ---
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (Guid.TryParse(userId, out Guid userGuid))
        {
            var user = await _userRepository.GetByIdAsync(userGuid);
            if (user != null)
            {
                user.IsOnline = true; // Domain Entity'da bu maydon bo'lishi kerak
                await _userRepository.UpdateAsync(user);

                // Hammani ushbu user online bo'lganidan xabardor qilish (ixtiyoriy)
                await Clients.Others.SendAsync("UserStatusChanged", userGuid, true);
            }
        }
        await base.OnConnectedAsync();
    }

    // --- FOYDALANUVCHI OFFLINE BO'LGANDA ---
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (Guid.TryParse(userId, out Guid userGuid))
        {
            var user = await _userRepository.GetByIdAsync(userGuid);
            if (user != null)
            {
                user.IsOnline = false;  
                user.LastSeen = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                await Clients.Others.SendAsync("UserStatusChanged", userGuid, false);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    // --- XABAR YUBORISH + NOTIFICATION ---
    public async Task SendMessage(Guid receiverId, string message)
    {
        var senderId = Guid.Parse(Context.UserIdentifier!);
        //var sender = await _userRepository.GetByIdAsync(senderId.ToString());

        var chatMsg = new ChatMessage
        {
            MessageId = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        // 1. Bazaga saqlash
        await _chatRepository.AddMessageAsync(chatMsg);

        // 2. Real-time (SignalR) orqali yuborish
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message, chatMsg.Timestamp);

        // 3. O'qilmagan xabarlar sonini yangilash
        var totalUnread = await _chatRepository.GetTotalUnreadCountAsync(receiverId);
        await Clients.User(receiverId.ToString()).SendAsync("UpdateUnreadCount", totalUnread);

        // 4. NOTIFICATION QISMI
        var receiver = await _userRepository.GetByIdAsync(receiverId);

        // Agar qabul qiluvchi offline bo'lsa yoki hozir chatda bo'lmasa
        if (receiver != null && !receiver.IsOnline)
        {
            // Sizning notification servisingiz orqali xabar yuboramiz
            // Masalan: "Ali sizga xabar yubordi: Salom!"
            await _notificationService.SendAsync(
                userId: receiverId,
                title: "Yangi xabar",
                message: message,
                type: "ChatMessage",
                actorId: senderId,
                postId: null,
                projectId: null,
                ct: default
            );
        }
    }

    // --- XABARLARNI O'QILDI QILISH (YANGI QO'SHILDI) ---
    public async Task MarkAsRead(Guid senderId)
    {
        var currentUserId = Guid.Parse(Context.UserIdentifier!);

        // Bazada yangilash
        await _chatRepository.MarkAsReadAsync(senderId, currentUserId);

        // Xabar yuborgan odamga (Sender) "O'qildi" signalini yuborish
        await Clients.User(senderId.ToString()).SendAsync("MessagesSeen", currentUserId);

        // O'zimizning unread countni yangilash
        var totalUnread = await _chatRepository.GetTotalUnreadCountAsync(currentUserId);
        await Clients.Caller.SendAsync("UpdateUnreadCount", totalUnread);
    }


    public async Task EditMessage(Guid messageId, string newMessage)
    {
        var currentUserId = Guid.Parse(Context.UserIdentifier!);
        var message = await _chatRepository.GetByIdAsync(messageId);

        // Faqat xabar egasi tahrirlay oladi va xabar mavjud bo'lishi kerak
        if (message != null && message.SenderId == currentUserId)
        {
            message.Message = newMessage;
            message.IsEdited = true;
            await _chatRepository.UpdateAsync(message);

            // Qabul qiluvchiga xabar o'zgarganini bildiramiz
            await Clients.User(message.ReceiverId.ToString())
                .SendAsync("MessageEdited", messageId, newMessage);

            // O'zimizga ham tasdiq yuboramiz (ixtiyoriy)
            await Clients.Caller.SendAsync("MessageEdited", messageId, newMessage);
        }
    }

    public async Task DeleteMessage(Guid messageId)
    {
        var currentUserId = Guid.Parse(Context.UserIdentifier!);
        var message = await _chatRepository.GetByIdAsync(messageId);

        if (message != null && message.SenderId == currentUserId)
        {
            var receiverId = message.ReceiverId;
            await _chatRepository.DeleteAsync(message);

            // Qabul qiluvchiga xabar o'chirilganini va uning Id sini yuboramiz
            await Clients.User(receiverId.ToString())
                .SendAsync("MessageDeleted", messageId);

            // O'zimizga ham tasdiq
            await Clients.Caller.SendAsync("MessageDeleted", messageId);
        }
    }


    public async Task Typing(Guid receiverId)
    {
        var senderId = Context.UserIdentifier;
        await Clients.User(receiverId.ToString()).SendAsync("UserTyping", senderId);
    }
}