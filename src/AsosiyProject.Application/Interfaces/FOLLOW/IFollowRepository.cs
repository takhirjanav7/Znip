namespace AsosiyProject.Application.Interfaces.FOLLOW;

public interface IFollowRepository
{
    Task FollowAsync(Guid followerId, Guid followingId, CancellationToken ct = default);
    Task UnfollowAsync(Guid followerId, Guid followingId);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken ct = default);
    Task<int> GetFollowersCountAsync(Guid userId, CancellationToken ct = default);
    Task<int> GetFollowingCountAsync(Guid userId, CancellationToken ct = default);
    Task<List<Guid>> GetFollowerIdsAsync(Guid userId, CancellationToken ct = default);
}