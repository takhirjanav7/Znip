using AsosiyProject.Application.Commands;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Services.likeService;

public class LikeService : ILikeService
{
    private readonly IAppDbContext _context;
    
    public LikeService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task LikePostAsync(Guid postId, Guid userId, CancellationToken ct = default)
    {
        var exists = await _context.Likes
            .AnyAsync(l => l.PostId == postId && l.UserId == userId, ct);

        if (!exists)
        {
            _context.Likes.Add(new Like
            {
                PostId = postId,
                UserId = userId,
                LikedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task UnlikePostAsync(Guid postId, Guid userId, CancellationToken ct = default)
    {
        var like = await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, ct);

        if (like != null)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(ct);
        }
    }

    public Task<bool> IsPostLikedByUserAsync(Guid postId, Guid userId, CancellationToken ct = default)
        => _context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId, ct);

    public Task<int> GetLikeCountAsync(Guid postId, CancellationToken ct = default)
        => _context.Likes.CountAsync(l => l.PostId == postId, ct);

    public async Task<List<Guid>> GetLikersAsync(Guid postId, int skip = 0, int take = 50, CancellationToken ct = default)
        => await _context.Likes
            .Where(l => l.PostId == postId)
            .OrderByDescending(l => l.LikedAt)
            .Skip(skip)
            .Take(take)
            .Select(l => l.UserId)
            .ToListAsync(ct);
}