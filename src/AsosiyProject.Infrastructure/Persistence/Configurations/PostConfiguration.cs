using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.PostId);

        // Bog'lanish: Bitta postda ko'p likelar bo'ladi
        builder.HasMany(p => p.Likes)
               .WithOne()
               .HasForeignKey(pl => pl.PostId)
               .OnDelete(DeleteBehavior.Cascade); // Post o'chsa, likelar ham o'chib ketadi

        // Agar Comment va View ni ham xuddi shunday bog'lamoqchi bo'lsangiz:
        builder.HasMany(p => p.Comments)
               .WithOne(c => c.Post)
               .HasForeignKey(c => c.PostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Views)
               .WithOne()
               .HasForeignKey(v => v.PostId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}