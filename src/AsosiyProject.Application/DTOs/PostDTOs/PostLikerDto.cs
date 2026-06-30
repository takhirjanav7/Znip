namespace AsosiyProject.Application.DTOs.PostDTOs;

public class PostLikerDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public bool IsFollowing { get; set; } // Siz bu odamni follow qilganmisiz? (Button chiqishi uchun)
}