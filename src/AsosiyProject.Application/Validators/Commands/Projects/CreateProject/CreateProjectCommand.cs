using MediatR;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.Validators.Commands.Projects.CreateProject;

public record CreateProjectCommand(
    string Title,
    string? Description,          
    string? RepositoryUrl,        // GitHub, GitLab link
    string? LiveDemoUrl,          // sayt linki
    IFormFile? Thumbnail,         // rasm (10MB gacha)
    List<Guid>? SkillIds = null   // qaysi texnologiyalar ishlatilgan
) : IRequest<Guid>;               // qaytaradi → yaratilgan Project.Id