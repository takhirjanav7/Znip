namespace AsosiyProject.Application.SignUp.Registration;

public record GetSkillDto(
    Guid SkillId,
    string Name,                    // masalan: "React", "Docker", "PostgreSQL"
    string? Category,               // "Frontend", "Backend", "DevOps", "Database"
    string? IconUrl,                
    int UsersCount                  // necha kishi bu skillni qo‘shgan (trending uchun)
);