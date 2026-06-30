//using AsosiyProject.Application.Interfaces.LIKE;
//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.LikeImplement;

//public class LikeRepository : ILikeRepository
//{
//    private readonly AppDbContext _context;
//    public LikeRepository(AppDbContext context) => _context = context;

//    public async Task LikePostAsync(Guid userId, Guid postId, CancellationToken ct)
//    {
//        var like = new Like { UserId = userId, PostId = postId, LikedAt = DateTime.UtcNow };
//        await _context.Likes.AddAsync(like, ct);
//    }

//    public Task UnlikePostAsync(Guid userId, Guid postId)
//    {
//        var like = _context.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);
//        if (like != null) _context.Likes.Remove(like);
//        return Task.CompletedTask;
//    }

//    public Task<bool> HasLikedAsync(Guid userId, Guid postId, CancellationToken ct)
//        => _context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId, ct);

//    public Task<int> GetLikesCountAsync(Guid postId, CancellationToken ct)
//        => _context.Likes.CountAsync(l => l.PostId == postId, ct);


//    // Project uchun like qo‘shish
//    public async Task LikeProjectAsync(Guid userId, Guid projectId, CancellationToken ct = default)
//    {
//        var like = new Like
//        {
//            UserId = userId,
//            ProjectId = projectId,
//            PostId = null,
//            LikedAt = DateTime.UtcNow
//        };
//        await _context.Likes.AddAsync(like, ct);
//    }

//    // Project uchun like o‘chirish
//    public async Task UnlikeProjectAsync(Guid userId, Guid projectId)
//    {
//        var like = await _context.Likes
//            .FirstOrDefaultAsync(l => l.UserId == userId && l.ProjectId == projectId);
//        if (like != null)
//            _context.Likes.Remove(like);
//    }

//    // Project yoqtirganmi?
//    public async Task<bool> HasLikedProjectAsync(Guid userId, Guid projectId, CancellationToken ct = default)
//    {
//        return await _context.Likes
//            .AnyAsync(l => l.UserId == userId && l.ProjectId == projectId, ct);
//    }

//    // Projectga nechta like?
//    public async Task<int> GetProjectLikesCountAsync(Guid projectId, CancellationToken ct = default)
//    {
//        return await _context.Likes
//            .CountAsync(l => l.ProjectId == projectId, ct);
//    }
//}