namespace AsosiyProject.Domain.Entities;

public class Follow
{
    public Guid FollowerId { get; set; }   // kim follow qildi
    public User Follower { get; set; } = null!;

    public Guid FollowingId { get; set; }  // kimni follow qildi
    public User Following { get; set; } = null!;

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}