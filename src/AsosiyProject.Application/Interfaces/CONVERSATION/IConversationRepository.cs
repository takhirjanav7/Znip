//using AsosiyProject.Domain.Entities;

//namespace AsosiyProject.Application.Interfaces.CONVERSATION;

//public interface IConversationRepository
//{
//    Task<Conversation> GetOrCreatePrivateAsync(Guid user1Id, Guid user2Id, CancellationToken ct = default);
//    Task<List<Conversation>> GetUserConversationsAsync(Guid userId, CancellationToken ct = default);
//    Task AddMemberAsync(Guid conversationId, Guid userId, CancellationToken ct = default);
//    Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken ct = default);


//    Task AddAsync(Conversation conversation, CancellationToken ct = default);
//}