//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace AsosiyProject.Infrastructure.Persistence.Configurations;

//public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
//{
//    public void Configure(EntityTypeBuilder<ProjectUser> builder)
//    {
//        builder.HasKey(pu => new { pu.ProjectId, pu.UserId });

//        builder.HasOne(pu => pu.Project)
//            .WithMany(p => p.Contributors)
//            .HasForeignKey(pu => pu.ProjectId)
//            .OnDelete(DeleteBehavior.Cascade);

//        builder.HasOne(pu => pu.User)
//            .WithMany(u => u.ProjectContributions)
//            .HasForeignKey(pu => pu.UserId)
//            .OnDelete(DeleteBehavior.Cascade);

//        builder.Property(pu => pu.JoinedAt).HasDefaultValueSql("NOW()");
//    }
//}