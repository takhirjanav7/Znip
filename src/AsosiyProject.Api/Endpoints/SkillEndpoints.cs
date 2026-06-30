using AsosiyProject.Application.Services.skillService;
using AsosiyProject.Application.SignUp.Registration;

namespace AsosiyProject.Api.Endpoints;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/skills").WithTags("Skills");

        // 1. Barcha skill'larni olish
        group.MapGet("/", async (ISkillService skillService) =>
        {
            var skills = await skillService.GetAllAsync();
            return Results.Ok(skills);
        });

        // 2. Skill ID bo'yicha olish
        group.MapGet("/{id:guid}", async (Guid id, ISkillService skillService) =>
        {
            var skill = await skillService.GetByIdAsync(id);
            return skill is not null ? Results.Ok(skill) : Results.NotFound();
        });

        // 3. Yangi skill qo'shish
        group.MapPost("/", async (CreateSkillDto dto, ISkillService skillService) =>
        {
            await skillService.CreateAsync(dto);
            return Results.Created($"/api/skills", dto);
        });

        // 4. Skill'ni yangilash
        group.MapPut("/{id:guid}", async (Guid id, UpdateSkillDto dto, ISkillService skillService) =>
        {
            await skillService.UpdateAsync(id, dto);
            return Results.Ok(dto);
        });

        // 5. Skill'ni o'chirish
        group.MapDelete("/{id:guid}", async (Guid id, ISkillService skillService) =>
        {
            await skillService.DeleteAsync(id);
            return Results.Ok(new { message = "Skill muvaffaqiyatli o'chirildi." });
        });

        // 6. Eng mashhur skill'lar
        group.MapGet("/popular/{count:int}", async (int count, ISkillService skillService) =>
        {
            var skills = await skillService.GetPopularSkillsAsync(count);
            return Results.Ok(skills);
        });

        // 7. Skill va unga ega bo'lgan userlar — TO‘G‘RI URL!
        group.MapGet("/{skillName}/users", async (string skillName, ISkillService skillService) =>
        {
            var result = await skillService.GetSkillWithUsersAsync(skillName);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        // 8. Userda skill bormi?
        group.MapGet("/check/{userId:guid}/{skillName}", async (Guid userId, string skillName, ISkillService skillService) =>
        {
            var exists = await skillService.IsSkillLinkedToUserAsync(userId, skillName);
            return Results.Ok(new { UserId = userId, Skill = skillName, HasSkill = exists });
        });
    }
}
