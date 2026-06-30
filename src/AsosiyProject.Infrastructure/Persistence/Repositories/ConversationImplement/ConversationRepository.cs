//using AsosiyProject.Application.Interfaces.CONVERSATION;
//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualBasic;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.ConversationImplement;

//public class ConversationRepository : IConversationRepository
//{
//    private readonly AppDbContext _context;
//    public ConversationRepository(AppDbContext context) => _context = context;

//    public async Task<Conversation> GetOrCreatePrivateAsync(Guid user1Id, Guid user2Id, CancellationToken ct)
//    {
//        var conv = await _context.Conversations
//            .Include(c => c.Members)
//            .FirstOrDefaultAsync(c => c.Type == ConversationType.Private &&
//                c.Members.Count == 2 &&
//                c.Members.Any(m => m.UserId == user1Id) &&
//                c.Members.Any(m => m.UserId == user2Id), ct);

//        if (conv != null)
//            return conv;

//        conv = new Conversation
//        {
//            ConversationId = Guid.NewGuid(),
//            Type = ConversationType.Private,
//            IsGroup = false,
//            CreatedAt = DateTime.UtcNow
//        };

//        conv.Members.Add(new ConversationMember
//        {
//            UserId = user1Id,
//            Role = "Member"
//        });

//        conv.Members.Add(new ConversationMember
//        {
//            UserId = user2Id,
//            Role = "Member"
//        });

//        await _context.Conversations.AddAsync(conv,ct);
//        await _context.SaveChangesAsync(ct);

//        return conv;
//    }

//    public async Task<List<Conversation>> GetUserConversationsAsync(Guid userId, CancellationToken ct = default)
//    {
//        return await _context.Conversations
//            .Include(c => c.Members)
//            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
//            .Where(c => c.Members.Any(m => m.UserId == userId))
//            .OrderByDescending(c => c.LastMessageAt)
//            .ToListAsync(ct);
//    }

//    public async Task AddMemberAsync(Guid conversationId, Guid userId, CancellationToken ct = default)
//    {
//        var exists = await _context.ConversationMembers
//            .AnyAsync(m => m.ConversationId == conversationId && m.UserId == userId, ct);

//        if (exists) return;

//        await _context.ConversationMembers.AddAsync(new ConversationMember
//        {
//            ConversationId = conversationId,
//            UserId = userId,
//            JoinedAt = DateTime.UtcNow
//        }, ct);

//        await _context.SaveChangesAsync(ct);
//    }

//    public async Task AddAsync(Conversation conversation, CancellationToken ct = default)
//    {
//        await _context.Conversations.AddAsync(conversation, ct);
//        await _context.SaveChangesAsync(ct);
//    }

//    public async Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken ct = default)
//    {
//        return await _context.Conversations
//            .Include(c => c.Members)
//            .FirstOrDefaultAsync(c => c.ConversationId == conversationId, ct);
//    }
//}
