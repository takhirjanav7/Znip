namespace AsosiyProject.Application.DTOs.CommentDTOs;

public class GetCommentDto
{
    public Guid CommentId { get; set; } // Comment ID (o'chirish uchun kerak)
    public Guid PostId { get; set; }

    // Kim yozdi?
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string UserAvatar { get; set; } // User rasmi dumaloq bo'lib turishi uchun

    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}