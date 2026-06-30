namespace AsosiyProject.Domain.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;        // masalan: "React", "Docker"
    public string? Category { get; set; }            // Language, Framework, Tool, Database
    public string? IconUrl { get; set; }

    public ICollection<UserSkill> Users { get; set; } = new List<UserSkill>();
}