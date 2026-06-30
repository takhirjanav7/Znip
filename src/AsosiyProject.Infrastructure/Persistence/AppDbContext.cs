using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence;

public class AppDbContext : DbContext, Application.Commands.IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserSkill> UserSkills { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    //public DbSet<ProjectUser> ProjectUsers { get; set; } = null!;
    //public DbSet<ProjectFile> ProjectFiles { get; set; } = null!;
    //public DbSet<Project> Projects { get; set; } = null!;
    //public DbSet<ProjectSkill> ProjectSkills { get; set; } = null!;
    //public DbSet<PostFile> PostFiles { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Like> Likes { get; set; } = null!;
    public DbSet<Follow> Follows { get; set; } = null!;
    //public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

    public DbSet<PostLike> PostLikes { get; set; } = null!;

    public DbSet<PostView> PostViews { get; set; } = null!;

    public DbSet<PostComment> PostComments { get; set; } = null!;


    // IAppDbContext interfeysining SaveChangesAsync metodini implement qilamiz
    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await base.SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MUHIM: Identity jadvallarini sozlash uchun baza metodini chaqirish kerak
        base.OnModelCreating(modelBuilder);

        //// Barcha Fluent API konfiguratsiyalarini yig'ish
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageReadConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConversationMemberConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConversationConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(FollowConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(LikeConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectUserConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectSkillConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkillConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserSkillConfiguration).Assembly);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Warninglarni o‘chirish (faqat logni tozalash uchun)
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk => fk.PrincipalEntityType.ClrType == typeof(User))
            .ToList()
            .ForEach(fk => fk.IsRequired = false);

        // Barcha DateTime → UTC (PostgreSQL uchun mukammal!)
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetColumnType("timestamp with time zone");
        }
    }
}
