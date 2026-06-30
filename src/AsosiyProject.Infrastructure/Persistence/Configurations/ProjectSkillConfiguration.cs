//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace AsosiyProject.Infrastructure.Persistence.Configurations;

//public class ProjectSkillConfiguration : IEntityTypeConfiguration<ProjectSkill>
//{
//    public void Configure(EntityTypeBuilder<ProjectSkill> b)
//    {
//        b.ToTable("ProjectSkills");
//        b.HasKey(ps => new { ps.ProjectId, ps.SkillId });
//        b.HasOne(ps => ps.Project).WithMany(p => p.ProjectSkills).HasForeignKey(ps => ps.ProjectId).OnDelete(DeleteBehavior.Cascade);
//        b.HasOne(ps => ps.Skill).WithMany().HasForeignKey(ps => ps.SkillId).OnDelete(DeleteBehavior.Cascade);
//    }
//}