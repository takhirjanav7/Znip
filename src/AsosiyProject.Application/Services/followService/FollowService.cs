using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Exceptions;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.DTOs.NotificationDTOs;
using AsosiyProject.Application.Interfaces.FOLLOW;
using AsosiyProject.Application.Interfaces.NOTIFICATION;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Services.notificationService;
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.followService;

public class FollowService : IFollowService
{
    private readonly INotificationRepository _notificationRepo;
    private readonly IUserRepository _userRepo;
    private readonly IFollowRepository _followRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly INotificationService _notification;
    private readonly IAppDbContext _context;

    public FollowService(IAppDbContext appDbContext, INotificationRepository notificationRepository, IUserRepository userRepository, IFollowRepository followRepository, IUnitOfWork uow, ICurrentUserService currentUser, INotificationService notification)
    {
        _context = appDbContext;
        _notificationRepo = notificationRepository;
        _userRepo = userRepository;
        _followRepo = followRepository;
        _uow = uow;
        _currentUser = currentUser;
        _notification = notification;
    }

    public async Task FollowAsync(Guid currentUserId, Guid targetUserId, CancellationToken ct)
    {
        // 0️⃣ O'zingizni follow qilishni taqiqlash
        if (currentUserId == targetUserId)
            throw new InvalidOperationException("O'zingizni follow qila olmaysiz");

        // 1️⃣ Oldin follow qilinganmi?
        if (await _followRepo.IsFollowingAsync(currentUserId, targetUserId, ct))
            return;

        // 2️⃣ Follow saqlash
        await _followRepo.FollowAsync(currentUserId, targetUserId, ct);

        // 3️⃣ Actor user (follow qilgan odam)
        var actor = await _userRepo.GetByIdAsync(currentUserId, ct);
        if (actor is null)
            throw new Exception("User topilmadi");

        // 4️⃣ Notification yaratish 🎯
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = targetUserId,                 // kimga
            ActorId = currentUserId,               // kimdan
            Title = "Yangi follower",
            Message = $"{actor.DisplayName} sizni follow qildi",
            Type = NotificationType.Follow.ToString(),
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepo.AddAsync(notification, ct);

        // 5️⃣ HAMMASINI BIR MARTA SAQLASH
        await _context.SaveChangesAsync(ct);
    }

    public async Task UnfollowAsync(Guid followingId, CancellationToken ct)
    {
        var followerId = _currentUser.UserId ?? throw new UnauthorizedException();
        await _uow.Follows.UnfollowAsync(followerId, followingId);
        await _uow.SaveChangesAsync(ct);
    }

    public Task<bool> IsFollowingAsync(Guid followingId, CancellationToken ct)
    {
        var followerId = _currentUser.UserId ?? throw new UnauthorizedException();
        return _uow.Follows.IsFollowingAsync(followerId, followingId, ct);
    }
}