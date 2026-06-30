namespace AsosiyProject.Application.Services.followService;

public interface IFollowService
{
    Task FollowAsync(Guid currentUserId, Guid targetUserId, CancellationToken ct);
    Task UnfollowAsync(Guid followingId, CancellationToken ct = default);
    Task<bool> IsFollowingAsync(Guid followingId, CancellationToken ct = default);
}