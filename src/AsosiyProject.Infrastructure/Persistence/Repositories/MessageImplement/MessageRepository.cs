//using AsosiyProject.Application.Interfaces.MESSAGE;
//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.MessageImplement;

//public class MessageRepository : IMessageRepository
//{
//    private readonly AppDbContext _context;

//    public MessageRepository(AppDbContext context)
//    {
//        _context = context;
//    }

//    // 1. Yangi xabar qo‘shish
//    public async Task<Message> CreateAsync(Message message)
//    {
//        await _context.Messages.AddAsync(message);
//        return message;
//    }

//    // 2. ID orqali xabar olish
//    public async Task<Message?> GetByIdAsync(Guid id)
//    {
//        return await _context.Messages
//            .Include(m => m.Sender)
//            .Include(m => m.Files)
//            .Include(m => m.Reads)
//                .ThenInclude(r => r.User)
//            .FirstOrDefaultAsync(m => m.Id == id);
//    }

//    // 3. Bitta suhbatdagi xabarlar (pagination bilan)
//    public async Task<List<Message>> GetConversationMessagesAsync(
//        Guid conversationId,
//        int skip,
//        int take)
//    {
//        return await _context.Messages
//            .Where(m => m.ConversationId == conversationId)
//            .Include(m => m.Sender)
//            .Include(m => m.Files)
//            .Include(m => m.Reads)
//                .ThenInclude(r => r.User)
//            .OrderByDescending(m => m.SentAt)
//            .Skip(skip)
//            .Take(take)
//            .ToListAsync();
//    }

//    // 4. User uchun eng oxirgi xabarlar (chat list)
//    public async Task<List<Message>> GetLatestMessagesForUserAsync(Guid userId, int take)
//    {
//        return await _context.Messages
//            .Where(m =>
//                m.Conversation.Members.Any(cm => cm.UserId == userId))
//            .Include(m => m.Sender)
//            .Include(m => m.Files)
//            .Include(m => m.Conversation)
//            .OrderByDescending(m => m.SentAt)
//            .Take(take)
//            .ToListAsync();
//    }

//    // 5. Bitta chat uchun unread count
//    public async Task<int> GetUnreadCountAsync(Guid userId, Guid conversationId)
//    {
//        return await _context.Messages
//            .Where(m =>
//                m.ConversationId == conversationId &&
//                m.SenderId != userId &&
//                !m.Reads.Any(r => r.UserId == userId))
//            .CountAsync();
//    }

//    // 6. Foydalanuvchida umuman unread bormi (qizil nuqta)
//    public async Task<bool> HasUnreadMessagesAsync(Guid userId)
//    {
//        return await _context.Messages
//            .AnyAsync(m =>
//                m.SenderId != userId &&
//                !m.Reads.Any(r => r.UserId == userId));
//    }

//    public async Task MarkAsReadAsync(Guid conversationId, Guid userId, CancellationToken ct = default)
//    {
//        // 1. Suhbatdagi o‘qilmagan xabarlarni topamiz
//        // Faqat boshqa odam yozgan va hali o‘qilmaganlarni
//        var unreadMessages = await _context.Messages
//            .Where(m => m.ConversationId == conversationId &&
//                        m.SenderId != userId &&     // o‘zim yozganlarini emas
//                        !m.IsRead)                  // hali o‘qilmaganlarini
//            .ToListAsync(ct);

//        // 2. Agar o‘qilmagan xabar bo‘lsa — belgilaymiz
//        if (unreadMessages.Any())
//        {
//            foreach (var message in unreadMessages)
//            {
//                message.IsRead = true;
//                message.SentAt = DateTime.UtcNow; // ixtiyoriy — qachon o‘qilganini saqlash uchun
//            }

//            // 3. Saqlaymiz
//            await _context.SaveChangesAsync(ct);
//        }
//    }

//    // 7. DB ga commit
//    public async Task SaveChangesAsync()
//    {
//        await _context.SaveChangesAsync();
//    }
//}