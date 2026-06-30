//using AsosiyProject.Application.Commands;
//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Domain.Entities;
//using MediatR;

//namespace AsosiyProject.Application.Validators.Commands.Projects.CreateProject;

//public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
//{
//    private readonly IAppDbContext _context;
//    private readonly ICurrentUserService _currentUser;
//    private readonly IFileService _fileService;

//    public CreateProjectCommandHandler(
//        IAppDbContext context,
//        ICurrentUserService currentUser,
//        IFileService fileService)
//    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
//        _context = context;
//        _currentUser = currentUser;
//        _fileService = fileService;
//    }

//    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken ct)
//    {
//        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

//        var project = new Project
//        {
//            ProjectId = Guid.NewGuid(),
//            Title = request.Title,
//            Description = request.Description,
//            RepositoryUrl = request.RepositoryUrl,
//            LiveDemoUrl = request.LiveDemoUrl,
//            OwnerId = userId,
//            CreatedAt = DateTime.UtcNow
//        };

//        // Thumbnail yuklash
//        if (request.Thumbnail != null)
//        {
//            var uploaded = await _fileService.UploadAsync(request.Thumbnail, ct);
//            project.ThumbnailUrl = uploaded.FileUrl;
//        }

//        // Skilllarni bog‘lash
//        if (request.SkillIds != null && request.SkillIds.Any())
//        {
//            project.ProjectSkills = request.SkillIds.Select(skillId => new ProjectSkill
//            {
//                ProjectId = project.ProjectId,
//                SkillId = skillId
//            }).ToList();
//        }

//        _context.Projects.Add(project);
//        await _context.SaveChangesAsync(ct);

//        return project.ProjectId;
//    }
//}