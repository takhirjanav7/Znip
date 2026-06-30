//using AsosiyProject.Application.Commands;
//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;
//using AsosiyProject.Domain.Entities;
//using MediatR;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Application.Validators.Commands.Comments.CreateComment;

//public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
//{
//    private readonly AppDbContext _context;
//    private readonly ICurrentUserService _currentUser;
//    private readonly IHubContext<NotificationHub> _hubContext;

//    public CreateCommentCommandHandler(
//        AppDbContext context,
//        ICurrentUserService currentUser,
//        IHubContext<NotificationHub> hubContext)
//    {
//        _context = context;
//        _currentUser = currentUser;
//        _hubContext = hubContext;
//    }

//    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken ct)
//    {
//        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

//        var post = await _context.Posts
//            .Include(p => p.User)
//            .FirstOrDefaultAsync(p => p.PostId   == request.PostId, ct)
//            ?? throw new KeyNotFoundException("Post topilmadi!");

//        var comment = new Comment
//        {
//            Id = Guid.NewGuid(),
//            PostId = request.PostId,
//            UserId = userId,
//            Text = request.Text.Trim(),
//            ParentCommentId = request.ParentCommentId,
//            CreatedAt = DateTime.UtcNow
//        };

//        _context.Comments.Add(comment);
//        await _context.SaveChangesAsync(ct);

//        // Real-time: yangi commentni hammaga yuborish
//        var commentDto = new
//        {
//            comment.Id,
//            comment.Text,
//            comment.CreatedAt,
//            User = new
//            {
//                Id = userId,
//                FullName = _currentUser.FullName,
//                ProfilePictureUrl = _currentUser.ProfilePictureUrl
//            },
//            LikesCount = 0,
//            RepliesCount = 0,
//            IsLiked = false
//        };

//        await _hubContext.Clients.Group("Post_" + request.PostId)
//            .SendAsync("NewComment", commentDto);

//        // Agar post egasiga comment bo‘lsa — bildirishnoma
//        if (post.UserId != userId)
//        {
//            await _hubContext.Clients.User(post.UserId.ToString())
//                .SendAsync("ReceiveNotification", new
//                {
//                    Type = "Comment",
//                    Message = $"{_currentUser.FullName} postingizga izoh qoldirdi",
//                    PostId = request.PostId,
//                    CommentId = comment.Id
//                });
//        }

//        return comment.Id;
//    }
//}