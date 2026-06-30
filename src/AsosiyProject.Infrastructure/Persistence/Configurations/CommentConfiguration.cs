//using AsosiyProject.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace AsosiyProject.Infrastructure.Persistence.Configurations;

//public class CommentConfiguration : IEntityTypeConfiguration<Comment>
//{
//    public void Configure(EntityTypeBuilder<Comment> b)
//    {
//        b.ToTable("Comments");
//        b.HasKey(c => c.Id);
//        b.Property(c => c.Id).ValueGeneratedOnAdd();

//        b.Property(c => c.Text).IsRequired().HasMaxLength(5000);
//        b.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");

//        b.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
//        b.HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);
//        b.HasOne(c => c.Project).WithMany(p => p.Comments).HasForeignKey(c => c.ProjectId).OnDelete(DeleteBehavior.SetNull);

//        b.HasOne(c => c.ParentComment)
//         .WithMany(c => c.Replies)
//         .HasForeignKey(c => c.ParentCommentId)
//         .OnDelete(DeleteBehavior.NoAction);

//        b.HasMany(c => c.Likes).WithOne().HasForeignKey("CommentId").OnDelete(DeleteBehavior.Cascade);
//    }
//}