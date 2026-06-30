namespace AsosiyProject.Application.DTOs.MessageDTOs;

//public record ChatMessageResultDto(Guid Id, Guid SenderId, string Message, DateTime Timestamp, bool IsRead);

public class ChatMessageResultDto
{
    public Guid MessageId { get; set; }
    public Guid SenderId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }

    public ChatMessageResultDto()
    {
        
    }
}