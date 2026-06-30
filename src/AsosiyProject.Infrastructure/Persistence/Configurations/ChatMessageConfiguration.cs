using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsosiyProject.Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        // Primary Key
        builder.HasKey(m => m.MessageId);

        // Xabar matni uchun cheklov
        builder.Property(m => m.Message)
            .IsRequired()
            .HasMaxLength(2000);

        // Sender (Yuboruvchi) bilan bog'lanish
        builder.HasOne(m => m.Receiver)
               .WithMany(u => u.ReceivedMessages)
               .HasForeignKey(m => m.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

        // 5. Indexlar (Tezkor qidiruv uchun)
        // Chat tarixini olishda SenderId va ReceiverId bo'yicha filtrlanadi, 
        // shuning uchun index qo'shish tavsiya etiladi
        builder.HasIndex(m => m.SenderId);
        builder.HasIndex(m => m.ReceiverId);
        builder.HasIndex(m => m.Timestamp);
    }
}