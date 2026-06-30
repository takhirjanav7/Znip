using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.DTOs.UserDTOs;

public class SafeUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public List<UserSkill> Skills { get; set; } = new();

    public string DisplayName => IsDeleted ? "O‘chirilgan akkaunt" : FullName;
    public string DisplayPicture => IsDeleted
        ? "/images/deleted-user.png"
        : (ProfilePictureUrl ?? "/images/default-avatar.png");

    internal bool IsDeleted { get; set; }

    public static SafeUserDto FromUser(User user)
    {
        return new SafeUserDto
        {
            Id = user.UserId,
            UserName = user.UserName ?? "unknown",
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            ProfilePictureUrl = user.ProfilePictureUrl,
            Skills = user.Skills.ToList(),
            IsDeleted = user.IsDeleted
        };
    }
}
