using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Commands;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserSkill> UserSkills { get; }
    DbSet<Skill> Skills { get; }
    //DbSet<ProjectUser> ProjectUsers { get; }
    //DbSet<ProjectFile> ProjectFiles { get; }
    //DbSet<Project> Projects { get; } 
    //DbSet<PostFile> PostFiles { get; }
    DbSet<Post> Posts { get; }
    DbSet<PostLike> PostLikes { get; }
    DbSet<PostView> PostViews { get; }
    DbSet<PostComment> PostComments { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Like> Likes { get; }
    DbSet<Follow> Follows { get; }
    //DbSet<Comment> Comments { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}