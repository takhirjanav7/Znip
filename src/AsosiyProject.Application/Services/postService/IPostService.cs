using AsosiyProject.Application.DTOs.PostDTOs;

namespace AsosiyProject.Application.Interfaces.Services;

public interface IPostService
{
    Task<Guid> CreatePostAsync(Guid userId, CreatePostDto dto);
    Task<bool> ToggleLikeAsync(Guid userId, Guid postId); // Like bosish/ochirish
    Task IncreaseViewCountAsync(Guid postId, Guid userId); 
    Task AddCommentAsync(Guid postId, Guid userId, string text);

    Task<List<PostViewDto>> GetAllPostsAsync(Guid userId, int page, int pageSize);
    Task<PostViewDto> GetPostByIdAsync(Guid id, Guid userId);
}