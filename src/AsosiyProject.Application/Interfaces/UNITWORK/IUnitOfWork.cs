using AsosiyProject.Application.Interfaces.ACTIVITY;
using AsosiyProject.Application.Interfaces.FOLLOW;
using AsosiyProject.Application.Interfaces.LIKE;
using AsosiyProject.Application.Interfaces.NOTIFICATION;
using AsosiyProject.Application.Interfaces.POST;
using AsosiyProject.Application.Interfaces.SKILL;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Interfaces.USERSKILL;

namespace AsosiyProject.Application.Interfaces.UNITWORK;

public interface IUnitOfWork : IDisposable
{
    IActivityRepository Activities { get; }
    IPostRepository Posts { get; }
    //ICommentRepository Comments { get; }
    ILikeRepository Likes { get; }
    IFollowRepository Follows { get; }
    //IProjectRepository Projects { get; }
    //IProjectUserRepository ProjectUsers { get; }
    ISkillRepository Skills { get; }
    IUserSkillRepository UserSkills { get; }
    //IProjectSkillRepository ProjectSkills { get; }
    //IMessageRepository Messages { get; }
    //IConversationRepository Conversations { get; }
    INotificationRepository Notifications { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}