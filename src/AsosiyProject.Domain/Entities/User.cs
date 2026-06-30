using System.ComponentModel.DataAnnotations.Schema;

namespace AsosiyProject.Domain.Entities;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeleteReason { get; set; } = null;
    public string DisplayName => IsDeleted
        ? "O‘chirilgan akkaunt"
        : $"{FirstName} {LastName}".Trim();

    public bool IsOnline { get; set; }
    public DateTime LastSeen { get; set; }

    public string PasswordHash { get; set; }
    public string Salt { get; set; }


    [NotMapped] // Ma'lumotlar bazasiga saqlanmaydi, faqat C#da ishlatiladi
    public string FullName => string.IsNullOrWhiteSpace(LastName)
        ? FirstName
        : $"{FirstName} {LastName}".Trim();

    // Dasturchi profili ma'lumotlari

    // Google hisobi bilan bog'lanish uchun
    public string? GoogleId { get; set; }
    public string? GoogleProfilePictureUrl { get; set; } // Nomini aniqlashtirdik
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? CoverPictureUrl { get; set; }
    public string? SkillsList { get; set; }

    // Statistika va vaqtlar
    public double Rating { get; set; } = 0.0;
    public int TotalRatings { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool EmailConfirmed { get; set; } = false;
    public bool IsVerified { get; set; } = false;

    // Autentifikatsiya tokenlari
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    // Navigatsiya xususiyatlari
    //public virtual ICollection<MessageRead> MessageReads { get; set; } = new List<MessageRead>();
    //public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
    public ICollection<UserSkill> Skills { get; set; } = new List<UserSkill>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}