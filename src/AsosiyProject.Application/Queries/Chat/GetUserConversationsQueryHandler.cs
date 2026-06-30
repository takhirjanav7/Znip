//using AsosiyProject.Application.Commands;
//using AsosiyProject.Application.DTOs.MessageDTOs;
//using AsosiyProject.Application.DTOs.UserDTOs;
//using AsosiyProject.Application.SignUp.Registration;
//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Application.Queries.Chat;

//public class GetUserConversationsQueryHandler : IRequestHandler<GetUserConversationsQuery, List<ConversationDto>>
//{
//    private readonly IAppDbContext _context;
//    private readonly IMapper _mapper;

//    public GetUserConversationsQueryHandler(IAppDbContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<List<ConversationDto>> Handle(GetUserConversationsQuery request, CancellationToken ct)
//    {
//        var conversations = await _context.Conversations
//            .Include(c => c.Members).ThenInclude(m => m.User)
//            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
//                .ThenInclude(m => m.Sender)
//            .Include(c => c.Messages).ThenInclude(m => m.Reads)
//            .Where(c => c.Members.Any(m => m.UserId == request.UserId))
//            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
//            .ToListAsync(ct);

//        var result = new List<ConversationDto>();

//        foreach (var conv in conversations)
//        {
//            var members = conv.Members.Select(m => m.User).ToList();
//            var lastMessage = conv.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();

//            var unreadCount = conv.Messages
//                .Where(m => m.SenderId != request.UserId)
//                .Count(m => !m.Reads.Any(r => r.UserId == request.UserId));

//            var dto = new ConversationDto(
//                ConversationId: conv.ConversationId,
//                Name: conv.Name,
//                ImageUrl: conv.ImageUrl,
//                Type: conv.Type,
//                LastMessageAt: conv.LastMessageAt,
//                LastMessage: lastMessage != null ? _mapper.Map<GetMessageDto>(lastMessage) : null,
//                Members: members.Select(u => _mapper.Map<UserSmallDto>(u)).ToList(),
//                UnreadCount: unreadCount,
//                IsOnline: false // Bu yerda onlayn holatni aniqlash logikasi qo‘shilishi mumkin
//            );

//            result.Add(dto);
//        }

//        return result;
//    }
//}