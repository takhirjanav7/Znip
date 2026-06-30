namespace AsosiyProject.Application.DTOs.PostDTOs;

public class SharePostDto
{
    public Guid PostId { get; set; } // Qaysi post
    public Guid ReceiverUserId { get; set; } // Kimga yuborilyapti (Chatdagi suhbatdosh)
    public string? Message { get; set; } // Qo'shimcha xabar (ixtiyoriy)
}