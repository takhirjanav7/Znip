using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.Services.notificationService;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly INotificationService _notificationService;

    public FollowUserCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, INotificationService notification)
    {
        _uow = unitOfWork;
        _currentUser = currentUser;
        _notificationService = notification;
    }

    public async Task<Unit> Handle(FollowUserCommand request, CancellationToken ct)
    {
        var followerId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

        // O‘zini follow qilish taqiqlansin
        if (followerId == request.TargetUserId)
            throw new InvalidOperationException("O‘zingizni follow qila olmaysiz!");

        // Allaqachon follow qilganmi?
        var isFollowing = await _uow.Follows.IsFollowingAsync(followerId, request.TargetUserId, ct);

        if (isFollowing)
            throw new InvalidOperationException("Allaqachon follow qilingan!");

        // 4. 🔥 NOTIFICATION YUBORISH (Eng muhim joyi)
        // Follower nomini olish (Notification matni uchun)
        var followerUser = await _uow.Users.GetByIdAsync(followerId, ct);
        string followerName = followerUser?.UserName ?? "Foydalanuvchi";

        await _notificationService.SendAsync(
            userId: request.TargetUserId,            // Kimga: Target User
            title: "New Follower",
            message: $"{followerName} sizni follow qildi",
            type: "Follow",
            actorId: followerId,                     // Kim qildi: Hozirgi user
            ct: ct
        );

        return Unit.Value;
    }
}