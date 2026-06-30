namespace AsosiyProject.Application.SignUp.Registration;

public record UpdateSkillDto(
    string? Name,
    string? Category,
    string? IconUrl,
    int ProficiencyLevel,
    int? YearsOfExperience = null
);  