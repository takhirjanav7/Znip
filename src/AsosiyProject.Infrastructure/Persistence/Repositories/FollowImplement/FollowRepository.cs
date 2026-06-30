using AsosiyProject.Application.Interfaces.FOLLOW;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.FollowImplement;

// 4. FollowRepository.cs
public class FollowRepository : IFollowRepository
{
    private readonly AppDbContext _context;
    public FollowRepository(AppDbContext context) => _context = context;

    public async Task FollowAsync(Guid followerId, Guid followingId, CancellationToken ct)
    {
        var follow = new Follow { FollowerId = followerId, FollowingId = followingId, FollowedAt = DateTime.UtcNow };
        await _context.Follows.AddAsync(follow, ct);
    }

    public Task UnfollowAsync(Guid followerId, Guid followingId)
    {
        var follow = _context.Follows.FirstOrDefault(f => f.FollowerId == followerId && f.FollowingId == followingId);
        if (follow != null) _context.Follows.Remove(follow);
        return Task.CompletedTask;
    }

    public Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken ct)
        => _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, ct);

    public Task<int> GetFollowersCountAsync(Guid userId, CancellationToken ct)
        => _context.Follows.CountAsync(f => f.FollowingId == userId, ct);

    public Task<int> GetFollowingCountAsync(Guid userId, CancellationToken ct)
        => _context.Follows.CountAsync(f => f.FollowerId == userId, ct);

    public async Task<List<Guid>> GetFollowerIdsAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.Follows
            .Where(f => f.FollowingId == userId)     // bu odamni kimlar follow qilgan?
            .Select(f => f.FollowerId)               // ularning ID’lari
            .ToListAsync(ct);
    }
}