namespace AsosiyProject.Application.Interfaces.LIKE;

public interface ILikeRepository
{
    // Post uchun
    Task LikePostAsync(Guid userId, Guid postId, CancellationToken ct = default);
    Task UnlikePostAsync(Guid userId, Guid postId);
    Task<bool> HasLikedAsync(Guid userId, Guid postId, CancellationToken ct = default);
    Task<int> GetLikesCountAsync(Guid postId, CancellationToken ct = default);

    // Project uchun
    Task LikeProjectAsync(Guid userId, Guid projectId, CancellationToken ct = default);
    Task UnlikeProjectAsync(Guid userId, Guid projectId);
    Task<bool> HasLikedProjectAsync(Guid userId, Guid projectId, CancellationToken ct = default);
    Task<int> GetProjectLikesCountAsync(Guid projectId, CancellationToken ct = default);
}