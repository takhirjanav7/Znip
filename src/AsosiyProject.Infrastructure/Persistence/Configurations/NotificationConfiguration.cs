using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> b)
    {
        b.ToTable("Notifications");
        b.HasKey(n => n.Id);
        b.Property(n => n.Title).IsRequired().HasMaxLength(200);
        b.Property(n => n.Message).IsRequired().HasMaxLength(1000);
        b.Property(n => n.Type).IsRequired().HasMaxLength(50);
        b.Property(n => n.IsRead).HasDefaultValue(false);
        b.Property(n => n.CreatedAt).HasDefaultValueSql("NOW()");

        b.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}