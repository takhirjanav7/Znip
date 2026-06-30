namespace AsosiyProject.Application.DTOs.CommentDTOs;

public class CreateCommentDto
{
    // PostId URL dan olinadi (/posts/{id}/comments), shuning uchun bu yerda shart emas
    public string Text { get; set; } // Izoh matni
}