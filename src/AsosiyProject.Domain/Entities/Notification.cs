namespace AsosiyProject.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Like, Comment, Follow, Mention
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ActorId { get; set; }      // kim qilgan (like, follow)
    public Guid? PostId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? CommentId { get; set; }
}