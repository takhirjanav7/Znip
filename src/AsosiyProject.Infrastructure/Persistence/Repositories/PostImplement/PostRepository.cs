using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Interfaces.POST;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.PostImplement;

public class PostRepository : IPostRepository
{
    private readonly IAppDbContext _context;

    public PostRepository(IAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
    }

    public async Task AddCommentAsync(PostComment comment)
    {
        await _context.PostComments.AddAsync(comment);
    }

    public async Task AddLikeAsync(PostLike like)
    {
        await _context.PostLikes.AddAsync(like);
    }

    public async Task AddViewAsync(PostView view)
    {
        await _context.PostViews.AddAsync(view);
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        // Agar Like/Commentlarni ham birga olish kerak bo'lsa .Include() ishlatiladi.
        return await _context.Posts
            // qo'shmoqchiman.
            .FirstOrDefaultAsync(p => p.PostId == id);
    }

    public async Task<PostLike?> GetLikeAsync(Guid postId, Guid userId)
    {
        return await _context.PostLikes
                             .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);
    }

    public Task RemoveLikeAsync(PostLike like)
    {
        _context.PostLikes.Remove(like);
        return Task.CompletedTask; // Remove methodi async emas, shuning uchun shunday qaytaramiz
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        return Task.CompletedTask;
    }
}