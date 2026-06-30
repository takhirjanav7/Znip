//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.DTOs.ProjectDTOs;
//using AsosiyProject.Application.Services.projectService;
//using Microsoft.AspNetCore.Mvc;

//namespace AsosiyProject.Api.Endpoints;

//public static class ProjectEndpoints
//{
//    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/api/projects").WithTags("Projects");

//        // 1. Barcha loyihalarni olish (keyin qo‘shamiz, hozircha comment)
//        // group.MapGet("/", ...);

//        // 2. Bitta loyiha olish
//        group.MapGet("/{id:guid}", async (Guid id, IProjectService projectService) =>
//        {
//            var project = await projectService.GetByIdAsync(id);
//            return project is not null ? Results.Ok(project) : Results.NotFound();
//        });

//        // 3. Yangi loyiha yaratish (SIZNING USULINGIZ BO‘YICHA!
//        group.MapPost("/", async (
//            [FromForm] ProjectCreateRequest req,
//            IProjectService projectService,
//            ICurrentUserService currentUser) =>
//                {
//                    if (!currentUser.IsAuthenticated)
//                        return Results.Unauthorized();

//                    var projectId = await projectService.CreateAsync(
//                        req.Title,
//                        req.Description,
//                        req.RepoUrl,
//                        req.DemoUrl,
//                        req.SkillIds,
//                        req.Thumbnail,
//                        req.Files,
//                        CancellationToken.None);

//                    return Results.Created($"/api/projects/{projectId}", new { Id = projectId });
//                })
//        .RequireAuthorization()
//        .DisableAntiforgery();


//        // 4. Loyihaga like bosish
//        group.MapPost("/{id:guid}/like", async (Guid id, IProjectService projectService, ICurrentUserService currentUser) =>
//        {
//            if (!currentUser.IsAuthenticated) return Results.Unauthorized();
//            await projectService.LikeAsync(id);
//            return Results.Ok(new { Message = "Liked!" });
//        })
//        .RequireAuthorization();

//        // 5. Loyihani o‘chirish
//        group.MapDelete("/{id:guid}", async (Guid id, IProjectService projectService, ICurrentUserService currentUser) =>
//        {
//            if (!currentUser.IsAuthenticated) return Results.Unauthorized();
//            await projectService.DeleteAsync(id);
//            return Results.NoContent();
//        })
//        .RequireAuthorization();
//    }
//}