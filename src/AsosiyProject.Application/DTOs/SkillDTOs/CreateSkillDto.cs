namespace AsosiyProject.Application.SignUp.Registration;

public class CreateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int ProficiencyLevel { get; set; } = 0;
    public int? YearsOfExperience { get; set; }

    public Guid UserId { get; set; }
}