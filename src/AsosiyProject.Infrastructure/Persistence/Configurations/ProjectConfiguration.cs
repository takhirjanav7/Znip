//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace AsosiyProject.Infrastructure.Persistence.Configurations;

//public class ProjectConfiguration : IEntityTypeConfiguration<Project>
//{
//    public void Configure(EntityTypeBuilder<Project> b)
//    {
//        b.ToTable("Projects");
//        b.HasKey(p => p.ProjectId);
//        b.Property(p => p.ProjectId).ValueGeneratedOnAdd();

//        b.Property(p => p.Title).IsRequired().HasMaxLength(200);
//        b.Property(p => p.Description).HasMaxLength(10000);
//        b.Property(p => p.RepositoryUrl).HasMaxLength(500);
//        b.Property(p => p.LiveDemoUrl).HasMaxLength(500);
//        b.Property(p => p.ThumbnailUrl).HasMaxLength(500);
//        b.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
//        b.Property(p => p.IsPublic).HasDefaultValue(true);

//        b.HasOne(p => p.Owner).WithMany(u => u.Projects).HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Cascade);

//        b.HasMany(p => p.Contributors).WithOne(pc => pc.Project).HasForeignKey(pc => pc.ProjectId).OnDelete(DeleteBehavior.Cascade);
//        b.HasMany(p => p.Files).WithOne(f => f.Project).HasForeignKey(f => f.ProjectId).OnDelete(DeleteBehavior.Cascade);
//        b.HasMany(p => p.ProjectSkills).WithOne(ps => ps.Project).HasForeignKey(ps => ps.ProjectId).OnDelete(DeleteBehavior.Cascade);
//        b.HasMany(p => p.Likes).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId).OnDelete(DeleteBehavior.Cascade);
//        b.HasMany(p => p.Comments).WithOne(c => c.Project).HasForeignKey(c => c.ProjectId).OnDelete(DeleteBehavior.Cascade);
//    }
//}