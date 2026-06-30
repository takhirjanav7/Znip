namespace AsosiyProject.Domain.Entities;

public class  UserSkill
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    public int ProficiencyLevel { get; set; } // 1-5 yoki 1-100
    public int? YearsOfExperience { get; set; }
    public DateTime AddedAt { get; set; }
}