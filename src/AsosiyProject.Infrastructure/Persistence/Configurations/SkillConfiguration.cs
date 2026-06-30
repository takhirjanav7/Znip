using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> b)
    {
        b.ToTable("Skills");
        b.HasKey(s => s.Id);
        b.Property(s => s.Name).IsRequired().HasMaxLength(100);
        b.Property(s => s.Category).HasMaxLength(50);
        b.Property(s => s.IconUrl).HasMaxLength(500);

        b.HasIndex(s => s.Name).IsUnique();
    }
}