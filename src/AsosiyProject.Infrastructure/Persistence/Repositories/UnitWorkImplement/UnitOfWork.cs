using AsosiyProject.Application.Interfaces.ACTIVITY;
using AsosiyProject.Application.Interfaces.FOLLOW;
using AsosiyProject.Application.Interfaces.LIKE;
using AsosiyProject.Application.Interfaces.NOTIFICATION;
using AsosiyProject.Application.Interfaces.POST;
using AsosiyProject.Application.Interfaces.SKILL;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Interfaces.USERSKILL;
using AsosiyProject.Infrastructure.Persistence.Repositories.ActivityImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.FollowImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.NotificationImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.PostImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.SkillImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.UserImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.UserSkillImplement;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.UnitWorkImplement;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IPostRepository Posts { get; }
    //public ICommentRepository Comments { get; }
    public ILikeRepository Likes { get; }
    public IFollowRepository Follows { get; }
    //public IProjectRepository Projects { get; }
    public ISkillRepository Skills { get; }
    public IUserSkillRepository UserSkills { get; }
    //public IProjectSkillRepository ProjectSkills { get; }
    //public IMessageRepository Messages { get; }
    //public IConversationRepository Conversations { get; }
    public INotificationRepository Notifications { get; }
    //public IProjectUserRepository ProjectUsers { get; }
    public IActivityRepository Activities { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Activities = new ActivityRepository(context);
        Posts = new PostRepository(context);
        //Comments = new CommentRepository(context);
        //Likes = new LikeRepository(context);
        Follows = new FollowRepository(context);
        //Projects = new ProjectRepository(context);
        Skills = new SkillRepository(context);
        UserSkills = new UserSkillRepository(context);
        //ProjectUsers = new ProjectUserRepository(context);
        //ProjectSkills = new ProjectSkillRepository(context);
        //Messages = new MessageRepository(context);
        //Conversations = new ConversationRepository(context);
        Notifications = new NotificationRepository(context);
        Users = new UserRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
        => await _context.Database.CurrentTransaction!.CommitAsync(ct);

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
        => await _context.Database.CurrentTransaction!.RollbackAsync(ct);

    public void Dispose() => _context.Dispose();
}