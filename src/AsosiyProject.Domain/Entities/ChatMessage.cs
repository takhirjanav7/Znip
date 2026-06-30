namespace AsosiyProject.Domain.Entities;

public class ChatMessage
{
    public Guid MessageId { get; set; } 
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }    
    public bool IsEdited { get; set; }

    // Navigation properties
    public virtual User Sender { get; set; } = null!;
    public virtual User Receiver { get; set; } = null!;
}