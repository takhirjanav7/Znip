namespace AsosiyProject.Application.DTOs.PostDTOs;

public class GetPostDto
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string MediaUrl { get; set; }
    public string Caption { get; set; }


    public int LikeCount { get; set; }
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }


    public bool IsLikedByMe { get; set; } 
    public DateTime CreatedAt { get; set; }
}