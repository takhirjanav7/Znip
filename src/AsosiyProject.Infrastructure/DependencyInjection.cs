using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Interfaces.FOLLOW;
using AsosiyProject.Application.Interfaces.LIKE;
using AsosiyProject.Application.Interfaces.MESSAGE;
using AsosiyProject.Application.Interfaces.NOTIFICATION;
using AsosiyProject.Application.Interfaces.POST;
using AsosiyProject.Application.Interfaces.Services;
using AsosiyProject.Application.Interfaces.SKILL;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Interfaces.USERSKILL;
using AsosiyProject.Application.Services;
using AsosiyProject.Application.Services.activityService;
using AsosiyProject.Application.Services.chatService;
using AsosiyProject.Application.Services.emailService;
using AsosiyProject.Application.Services.followService;
using AsosiyProject.Application.Services.likeService;
using AsosiyProject.Application.Services.notificationService;
using AsosiyProject.Application.Services.searchService;
using AsosiyProject.Application.Services.Settings;
using AsosiyProject.Application.Services.SignUp.Registration;
using AsosiyProject.Application.Services.skillService;
using AsosiyProject.Application.Services.userService;
using AsosiyProject.Application.Validators.FollowValidators;
using AsosiyProject.Application.Validators.PostValidators;
using AsosiyProject.Application.Validators.ProfileValidators;
using AsosiyProject.Application.Validators.ProjectValidators;
using AsosiyProject.Application.Validators.SkillValidators;
using AsosiyProject.Infrastructure.Persistence.Repositories.FollowImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.MessageImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.NotificationImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.PostImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.SkillImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.UnitWorkImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.UserImplement;
using AsosiyProject.Infrastructure.Persistence.Repositories.UserSkillImplement;
using AsosiyProject.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AsosiyProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. PostgreSQL
        services.AddDbContext<Persistence.AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(Persistence.AppDbContext).Assembly.FullName)));

        // 2. IAppDbContext
        services.AddScoped<Application.Commands.IAppDbContext>(provider =>
            provider.GetRequiredService<Persistence.AppDbContext>());

        // 3. SignalR
        services.AddSignalR();
        services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        // 4. AutoMapper (oddiy usul)
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // 5. MediatR (MUHIM!)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        // 6. Health Checks (faqat PostgreSQL)
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);

        services.AddHttpContextAccessor();

        // Services
        services.AddScoped<IFileService, LocalFileService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();


        // Entity Services
        services.AddScoped<IActivityService, ActivityService>();
        //services.AddScoped<IChatService, ChatService>();
        //services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFollowService, FollowService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IChatService, ChatService>();
        //services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPostService, PostService>();
        //services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IUserService, UserService>();
        //services.AddScoped<FeedService>();


        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        //services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
        //services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        //services.AddScoped<IMessageRepository, MessageRepository>();
        //services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        //services.AddScoped<IConversationRepository, ConversationRepository>();
        //services.AddScoped<ICommentRepository, CommentRepository>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreatePostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<AddUserSkillCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateProfileCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
        //services.AddValidatorsFromAssemblyContaining<SendMessageCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<FollowUserCommandValidator>();
        //services.AddValidatorsFromAssemblyContaining<CreateCommentCommandValidator>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // JwtSettings ni ro'yxatga olish

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}