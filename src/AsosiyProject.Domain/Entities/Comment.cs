//namespace AsosiyProject.Domain.Entities;

//public class Comment
//{
//    public Guid Id { get; set; }
//    public Guid PostId { get; set; }
//    public Post Post { get; set; }

//    public Guid UserId { get; set; }
//    public User User { get; set; } = null!;


//    public string Text { get; set; } = null!;
//    public Guid? ParentCommentId { get; set; }
//    public Comment? ParentComment { get; set; }


//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    public DateTime? UpdatedAt { get; set; }


//    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
//    public ICollection<Like> Likes { get; set; } = new List<Like>();
//    public Guid? ProjectId { get; set; }
//    public Project? Project { get; set; }

//}