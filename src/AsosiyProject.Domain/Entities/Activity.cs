namespace AsosiyProject.Domain.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Action { get; set; } = null!;        // "Liked", "Commented", "Followed"
    public string TargetId { get; set; } = null!;      // PostId, ProjectId, UserId
    public string TargetType { get; set; } = null!;    // "Post", "Project", "User"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}