using AsosiyProject.Application.DTOs.MessageDTOs;
using AsosiyProject.Application.Interfaces.MESSAGE;
using AutoMapper;

namespace AsosiyProject.Application.Services.chatService;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;

    public ChatService(IChatRepository chatRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _mapper = mapper;
    }

    public async Task<List<ChatMessageResultDto>> GetHistory(Guid currentUserId, Guid otherUserId, int page)
    {
        var messages = await _chatRepository.GetChatHistoryAsync(currentUserId, otherUserId, page, 20);
        var orderedMessages = messages.OrderBy(x => x.Timestamp).ToList();
        return _mapper.Map<List<ChatMessageResultDto>>(orderedMessages);
    }

    public async Task<int> GetTotalUnreadCount(Guid userId)
        => await _chatRepository.GetTotalUnreadCountAsync(userId);

    public async Task<Dictionary<Guid, int>> GetUnreadSummary(Guid userId)
        => await _chatRepository.GetUnreadCountsByChatAsync(userId);
}