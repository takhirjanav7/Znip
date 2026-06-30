namespace AsosiyProject.Domain.Entities;

public class Like
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Biror narsa yo Post, yo Project bo‘lishi mumkin
    public Guid? PostId { get; set; }
    public Post? Post { get; set; }

  
    //public Guid? CommentId { get; set; }
    //public PostComment? Comment { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}