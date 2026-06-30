//namespace AsosiyProject.Domain.Entities;

//public class Project
//{
//    public Guid ProjectId { get; set; }
//    public string Title { get; set; } = null!;
//    public string? Description { get; set; }
//    public string? RepositoryUrl { get; set; }
//    public string? LiveDemoUrl { get; set; }
//    public string? ThumbnailUrl { get; set; }
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    public bool IsPublic { get; set; } = true;

//    public Guid OwnerId { get; set; }
//    public User Owner { get; set; } = null!;

//    public ICollection<ProjectUser> Contributors { get; set; } = new List<ProjectUser>();
//    public ICollection<ProjectFile> Files { get; set; } = new List<ProjectFile>();
//    public ICollection<Post> ShowcasePosts { get; set; } = new List<Post>();
//    public ICollection<Like> Likes { get; set; } = new List<Like>();
//    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
//    public List<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
//}


//public class    ProjectSkill
//{
//    public Guid ProjectId { get; set; }
//    public Project Project { get; set; } = null!;
//    public Guid SkillId { get; set; }
//    public Skill Skill { get; set; } = null!;
//}