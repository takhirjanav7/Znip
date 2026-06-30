using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> b)
    {
        b.ToTable("UserSkills");
        b.HasKey(us => new { us.UserId, us.SkillId });

        b.HasOne(us => us.User)
            .WithMany(u => u.Skills)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(us => us.Skill)
            .WithMany(s => s.Users)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Property(us => us.ProficiencyLevel)
            .HasDefaultValue(50);

        b.Property(us => us.AddedAt)
            .HasDefaultValueSql("NOW()");

        b.Property(us => us.YearsOfExperience);
    }
}