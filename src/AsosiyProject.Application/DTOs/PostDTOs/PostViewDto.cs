namespace AsosiyProject.Application.DTOs.PostDTOs;

public class PostViewDto
{
    public Guid Id { get; set; }
    public string Caption { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } // "Image" yoki "Video"

    // Statistikalar
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    // Foydalanuvchi bilan bog'liq qism (Juda muhim!)
    public bool IsLiked { get; set; } // Men like bosganmanmi?
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}