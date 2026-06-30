//using AsosiyProject.Application.Common.Exceptions;
//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Interfaces.UNITWORK;
//using AsosiyProject.Application.Services.notificationService;
//using AsosiyProject.Domain.Entities;    
//using Microsoft.AspNetCore.Http;

//namespace AsosiyProject.Application.Services.projectService;

//public class ProjectService : IProjectService
//{
//    private readonly IUnitOfWork _uow;
//    private readonly ICurrentUserService _currentUser;
//    private readonly IFileService _fileService;
//    private readonly INotificationService _notification;

//    public ProjectService(IUnitOfWork uow, ICurrentUserService currentUser,
//        IFileService fileService, INotificationService notification)
//    {
//        _uow = uow;
//        _currentUser = currentUser;
//        _fileService = fileService;
//        _notification = notification;
//    }

//    public async Task<Guid> CreateAsync(string title, string? description, string? repoUrl,
//        string? demoUrl, Guid[]? skillIds, IFormFile? thumbnail, IFormFile[]? files, CancellationToken ct)
//    {
//        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

//        var project = new Project
//        {
//            ProjectId = Guid.NewGuid(),
//            Title = title.Trim(),
//            Description = description?.Trim(),
//            RepositoryUrl = repoUrl?.Trim(),
//            LiveDemoUrl = demoUrl?.Trim(),
//            OwnerId = userId,
//            CreatedAt = DateTime.UtcNow,
//            IsPublic = true
//        };

//        // Thumbnail
//        if (thumbnail != null)
//        {
//            var thumb = await _fileService.UploadAsync(thumbnail, ct);
//            project.ThumbnailUrl = thumb.FileUrl;
//        }

//        // Fayllar
//        if (files?.Length > 0)
//        {
//            var uploaded = await _fileService.UploadMultipleAsync(files, ct);
//            project.Files = uploaded.Select(f => new ProjectFile
//            {
//                Id = Guid.NewGuid(),
//                FileName = f.FileName,
//                FileUrl = f.FileUrl,
//                ContentType = f.FileType,
//                FileSize = f.FileSize,
//                ProjectId = project.ProjectId
//            }).ToList();
//        }

//        // Skills
//        if (skillIds?.Length > 0)
//        {
//            project.ProjectSkills = skillIds.Select(s => new ProjectSkill
//            {
//                ProjectId = project.ProjectId,
//                SkillId = s
//            }).ToList();
//        }

//        await _uow.Projects.AddAsync(project, ct);
//        await _uow.SaveChangesAsync(ct);

//        // Notification → followers
//        var followers = await _uow.Follows.GetFollowerIdsAsync(userId, ct);
//        foreach (var followerId in followers)
//        {
//            await _notification.SendAsync(followerId, "Yangi loyiha!",
//                $"{_currentUser.FullName} yangi loyiha yaratdi: {title}", "NewProject", userId, projectId: project.ProjectId, ct: ct);
//        }

//        return project.ProjectId;
//    }

//    public async Task AddContributorAsync(Guid projectId, Guid userId, string role, CancellationToken ct)
//    {
//        var project = await _uow.Projects.GetByIdAsync(projectId, ct) ?? throw new NotFoundException("Project", projectId);
//        if (project.OwnerId != _currentUser.UserId) throw new ForbiddenException();

//        var contrib = new ProjectUser
//        {
//            ProjectId = projectId,
//            UserId = userId,
//            Role = role,
//            JoinedAt = DateTime.UtcNow
//        };

//        await _uow.ProjectUsers.AddAsync(contrib, ct);
//        await _uow.SaveChangesAsync(ct);

//        await _notification.SendAsync(userId, "Siz loyihaga qo‘shildingiz!",
//            $"{_currentUser.FullName} sizni {project.Title} loyihasiga qo‘shdi", "ProjectInvite", _currentUser.UserId, projectId: projectId, ct: ct);
//    }

//    public async Task RemoveContributorAsync(Guid projectId, Guid userId, CancellationToken ct)
//    {
//        var project = await _uow.Projects.GetByIdAsync(projectId, ct) ?? throw new NotFoundException("Project", projectId);
//        if (project.OwnerId != _currentUser.UserId) throw new ForbiddenException();

//        var contrib = await _uow.ProjectUsers.GetByProjectAndUserAsync(projectId, userId, ct);
//        if (contrib != null)
//        {
//            _uow.ProjectUsers.Delete(contrib);
//            await _uow.SaveChangesAsync(ct);
//        }
//    }

//    public async Task AddSkillsAsync(Guid projectId, Guid[] skillIds, CancellationToken ct)
//    {
//        // 1. Project borligini va egasi ekanligini tekshirish
//        var project = await _uow.Projects.GetByIdAsync(projectId, ct)
//            ?? throw new NotFoundException("Project topilmadi");

//        if (project.OwnerId != _currentUser.UserId) 
//            throw new ForbiddenException("Faqat loyiha egasi skill qo‘sha oladi");

//        // 2. Agar hech qanday skill kiritilmagan bo‘lsa — hech nima qilmaymiz
//        if (skillIds == null || skillIds.Length == 0)
//            return;

//        // 3. Barcha skilllarni bir marta qo‘shish — ENG TEZ USUL!
//        var newSkills = skillIds.Select(skillId => new ProjectSkill
//        {
//            ProjectId = projectId,
//            SkillId = skillId
//        });

//        await _uow.ProjectSkills.AddRangeAsync(newSkills, ct);
//        await _uow.SaveChangesAsync(ct);
//    }

//    public async Task LikeAsync(Guid projectId, CancellationToken ct)
//    {
//        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
//        var project = await _uow.Projects.GetByIdAsync(projectId, ct) ?? throw new NotFoundException("Project", projectId);

//        if (!await _uow.Likes.HasLikedProjectAsync(userId, projectId, ct))
//        {
//            await _uow.Likes.LikeProjectAsync(userId, projectId, ct);
//            await _uow.SaveChangesAsync(ct);

//            if (project.OwnerId != userId)
//            {
//                await _notification.SendAsync(project.OwnerId, "Loyiha yoqdi!",
//                    $"{_currentUser.FullName} sizning loyihangizni yoqtirdi", "Like", userId, projectId: projectId, ct: ct);
//            }
//        }
//    }

//    public async Task DeleteAsync(Guid projectId, CancellationToken ct)
//    {
//        var project = await _uow.Projects.GetByIdWithDetailsAsync(projectId, ct) ?? throw new NotFoundException("Project", projectId);
//        if (project.OwnerId != _currentUser.UserId) throw new ForbiddenException();

//        if (project.Files?.Any() == true)
//            await _fileService.DeleteMultipleAsync(project.Files.Select(f => f.FileUrl));

//        await _uow.Projects.DeleteAsync(project);
//        await _uow.SaveChangesAsync(ct);
//    }

//    public Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct)
//        => _uow.Projects.GetByIdWithDetailsAsync(projectId, ct);
//}