using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        // Jadval nomi
        builder.ToTable("PostLikes");

        // Composite Key: Bir user bir postga faqat 1 marta like bosishi uchun
        // Bu bazada unikal indeks hosil qiladi
        builder.HasKey(pl => new { pl.PostId, pl.UserId });
    }
}