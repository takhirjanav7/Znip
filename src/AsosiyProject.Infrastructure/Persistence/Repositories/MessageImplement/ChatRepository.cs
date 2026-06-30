using AsosiyProject.Application.Interfaces.MESSAGE;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.MessageImplement;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;
    public ChatRepository(AppDbContext context) => _context = context;

    public async Task AddMessageAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ChatMessage message)
    {
        _context.ChatMessages.Remove(message);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatMessage?> GetByIdAsync(Guid id)
    {
        return await _context.ChatMessages.FindAsync(id);
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync(Guid user1, Guid user2, int page, int pageSize)
    {
        return await _context.ChatMessages
            .Where(m => (m.SenderId == user1 && m.ReceiverId == user2) ||
                        (m.SenderId == user2 && m.ReceiverId == user1))
            .OrderByDescending(m => m.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalUnreadCountAsync(Guid userId)
    {
        return await _context.ChatMessages
            .CountAsync(m => m.ReceiverId == userId && !m.IsRead);
    }

    public async Task<Dictionary<Guid, int>> GetUnreadCountsByChatAsync(Guid userId)
    {
        return await _context.ChatMessages
        .Where(m => m.ReceiverId == userId && !m.IsRead)
        .GroupBy(m => m.SenderId)
        .Select(g => new { SenderId = g.Key, Count = g.Count() })
        .ToDictionaryAsync(x => x.SenderId, x => x.Count);
    }

    public async Task MarkAsReadAsync(Guid senderId, Guid receiverId)
    {
        var messages = await _context.ChatMessages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsRead)
            .ToListAsync();

        messages.ForEach(m => m.IsRead = true);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ChatMessage message)
    {
        _context.ChatMessages.Update(message);
        await _context.SaveChangesAsync();
    }
}