using AsosiyProject.Application.DTOs.MessageDTOs;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.Services.chatService;

public interface IChatService
{
    Task<List<ChatMessageResultDto>> GetHistory(Guid currentUserId, Guid otherUserId, int page);
    Task<int> GetTotalUnreadCount(Guid userId);
    Task<Dictionary<Guid, int>> GetUnreadSummary(Guid userId);
}