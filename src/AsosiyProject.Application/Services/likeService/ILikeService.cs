namespace AsosiyProject.Application.Services.likeService;

public interface ILikeService
{
    Task LikePostAsync(Guid postId, Guid userId, CancellationToken ct = default);
    Task UnlikePostAsync(Guid postId, Guid userId, CancellationToken ct = default);
    Task<bool> IsPostLikedByUserAsync(Guid postId, Guid userId, CancellationToken ct = default);
    Task<int> GetLikeCountAsync(Guid postId, CancellationToken ct = default);
    Task<List<Guid>> GetLikersAsync(Guid postId, int skip = 0, int take = 50, CancellationToken ct = default);
}