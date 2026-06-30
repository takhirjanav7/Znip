//using AsosiyProject.Domain.Entities;
//using Microsoft.AspNetCore.Http;

//namespace AsosiyProject.Application.Services.projectService;

//public interface IProjectService
//{
//    Task<Guid> CreateAsync(string title, string? description, string? repoUrl,
//        string? demoUrl, Guid[]? skillIds, IFormFile? thumbnail, IFormFile[]? files, CancellationToken ct);

//    Task AddContributorAsync(Guid projectId, Guid userId, string role = "Contributor", CancellationToken ct = default);
//    Task RemoveContributorAsync(Guid projectId, Guid userId, CancellationToken ct = default);
//    Task AddSkillsAsync(Guid projectId, Guid[] skillIds, CancellationToken ct = default);
//    Task LikeAsync(Guid projectId, CancellationToken ct = default);
//    Task DeleteAsync(Guid projectId, CancellationToken ct = default);
//    Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct = default);
//}