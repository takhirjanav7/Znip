using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Validators.Commands.Follows.DeleteFollow;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Features.Follow.Commands.UnfollowUser;

public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UnfollowUserCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        // O‘zini unfollow qilish taqiqlansin
        if (currentUserId == request.TargetUserId)
            throw new InvalidOperationException("O‘zingizni unfollow qila olmaysiz!");

        // Follow munosabatini topamiz
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f =>
                f.FollowerId == currentUserId &&
                f.FollowingId == request.TargetUserId,
                cancellationToken);

        if (follow == null)
            throw new KeyNotFoundException("Siz bu odamni follow qilmagansiz!");

        // O‘chiramiz
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync(cancellationToken);

        // (Ixtiyoriy) Real-time bildirishnoma — "Ali sizni unfollow qildi"
        // Agar kerak bo‘lsa SignalR orqali yuborish mumkin

        return Unit.Value;
    }
}