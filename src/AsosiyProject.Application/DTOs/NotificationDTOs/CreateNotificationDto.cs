using AsosiyProject.Application.DTOs.UserDTOs;

namespace AsosiyProject.Application.SignUp.Registration;

public record CreateNotificationDto( 
    Guid UserId,
    string Title,
    string Message,
    string Type,                    // "Like", "Comment", "Follow", "Mention", "ProjectInvite"...
    string TargetUrl,               // masalan: "/post/123", "/project/456", "/profile/ali"
    UserSmallDto Actor,             // kim qilgan (like bosgan, comment yozgan)
    DateTime CreatedAt,
    bool IsRead = false
); 