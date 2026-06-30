using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Jadval nomi
        builder.ToTable("AppUsers");

        // Asosiy fieldlar
        builder.Property(u => u.FirstName)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(u => u.LastName)
               .HasMaxLength(200);

        builder.Property(u => u.Bio)
               .HasMaxLength(1000);

        builder.Property(u => u.Location)
               .HasMaxLength(150);

        builder.Property(u => u.WebsiteUrl)
               .HasMaxLength(500);

        builder.Property(u => u.GitHubUrl)
               .HasMaxLength(500);

        builder.Property(u => u.LinkedInUrl)
               .HasMaxLength(500);

        builder.Property(u => u.ProfilePictureUrl)
               .HasMaxLength(1000);

        builder.Property(u => u.CoverPictureUrl)
               .HasMaxLength(1000);

        // YANGI — RATING TIZIMI (ENG PROFESSIONAL!)
        builder.Property(u => u.Rating)
               .HasColumnType("decimal(3,2)")
               .HasPrecision(3, 2)
               .HasDefaultValue(0.0);

        builder.Property(u => u.TotalRatings)
               .HasDefaultValue(0);

        // Vaqtlar
        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("NOW()");

        builder.Property(u => u.UpdatedAt)
               .HasDefaultValueSql("NOW()")
               .ValueGeneratedOnAddOrUpdate();

        // Refresh Token
        builder.Property(u => u.RefreshToken)
               .HasMaxLength(500);

        builder.Property(u => u.RefreshTokenExpiry)
               .HasColumnType("timestamp");

        // Holatlar
        builder.Property(u => u.IsVerified)
               .HasDefaultValue(false);

        // Unique Indexlar
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        // Performance uchun indexlar
        builder.HasIndex(u => u.Rating);
        builder.HasIndex(u => u.TotalRatings);

        // Relationships — hammasi Cascade bilan
        builder.HasMany(u => u.Posts)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        //builder.HasMany(u => u.Projects)
        //       .WithOne(p => p.Owner)
        //       .HasForeignKey(p => p.OwnerId)
        //       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Comments)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Likes)
               .WithOne(l => l.User)
               .HasForeignKey(l => l.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Following)
               .WithOne(f => f.Follower)
               .HasForeignKey(f => f.FollowerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Followers)
               .WithOne(f => f.Following)
               .HasForeignKey(f => f.FollowingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Skills)
               .WithOne(us => us.User)
               .HasForeignKey(us => us.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Notifications)
               .WithOne(n => n.User)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        //// ProjectContributions ham bor
        //builder.HasMany(u => u.ProjectContributions)
        //       .WithOne(pu => pu.User)
        //       .HasForeignKey(pu => pu.UserId)
        //       .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}