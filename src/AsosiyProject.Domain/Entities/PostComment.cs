namespace AsosiyProject.Domain.Entities;

public class PostComment
{
    public Guid Id { get; set; } 
    public Guid PostId { get; set; }
    public virtual Post? Post { get; set; }
    public Guid UserId { get; set; } // Kim yozdi
    public virtual User? User { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}