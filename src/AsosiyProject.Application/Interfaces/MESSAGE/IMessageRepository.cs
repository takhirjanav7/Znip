//namespace AsosiyProject.Application.Interfaces.MESSAGE;

//public interface IMessageRepository
//{
//    Task<Message> CreateAsync(Message message);
//    Task<Message?> GetByIdAsync(Guid id);
//    Task<List<Message>> GetConversationMessagesAsync(Guid conversationId, int skip, int take);
//    Task<List<Message>> GetLatestMessagesForUserAsync(Guid userId, int take);
//    Task<int> GetUnreadCountAsync(Guid userId, Guid conversationId);
//    Task<bool> HasUnreadMessagesAsync(Guid userId);
//    Task MarkAsReadAsync(Guid conversationId, Guid userId, CancellationToken ct = default);  // YANGI QO‘SHILDI!
//    Task SaveChangesAsync();
//}