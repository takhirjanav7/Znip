using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.POST;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);

    // Like uchun maxsus metodlar
    Task<PostLike?> GetLikeAsync(Guid postId, Guid userId);
    Task AddLikeAsync(PostLike like);
    Task RemoveLikeAsync(PostLike like);

    // View uchun
    Task AddViewAsync(PostView view);

    // Comment uchun
    Task AddCommentAsync(PostComment comment);

    Task SaveChangesAsync();
}