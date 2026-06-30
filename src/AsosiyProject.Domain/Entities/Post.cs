using System.ComponentModel.DataAnnotations;

namespace AsosiyProject.Domain.Entities;

public class Post
{
    [Key] // Bu xususiyat Post jadvalidagi asosiy kalit (Primary Key) bo'ladi
    public Guid PostId { get; set; }
    public Guid UserId { get; set; } // Kim yozdi
    public virtual User? User { get; set; }
    public string? Caption { get; set; } // Izoh
    public string MediaUrl { get; set; } // Rasm/Video yo'li
    public MediaType MediaType { get; set; } // "Image" or "Video"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } 

    // Statistikalar (Performance uchun)
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    // Navigation Properties
    // public User User { get; set; } // User entitysi bor deb hisoblaymiz
    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    public ICollection<PostView> Views { get; set; } = new List<PostView>();
}

public enum MediaType
{
    Image = 0,
    Video = 1
}