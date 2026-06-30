using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.MESSAGE;

public interface IChatRepository
{
    Task<ChatMessage?> GetByIdAsync(Guid id);
    Task UpdateAsync(ChatMessage message);
    Task DeleteAsync(ChatMessage message);
    Task<int> GetTotalUnreadCountAsync(Guid userId);
    Task<Dictionary<Guid, int>> GetUnreadCountsByChatAsync(Guid userId);
    Task AddMessageAsync(ChatMessage message);
    Task<List<ChatMessage>> GetChatHistoryAsync(Guid user1, Guid user2, int page, int pageSize);
    Task MarkAsReadAsync(Guid senderId, Guid receiverId);
}
