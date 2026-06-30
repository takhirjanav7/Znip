namespace AsosiyProject.Application.SignUp.Registration;

public record GetSkillWithUsersDto( 
    Guid SkillId,
    string Name,
    string? Category,
    string? IconUrl,
    int ProficiencyLevel,
    int? YearsOfExperience
)
{
    public string ProficiencyText => ProficiencyLevel switch
    {
        1 => "Boshlang‘ich",
        2 => "O‘rta",
        3 => "Yaxshi",
        4 => "Professional",
        5 => "Ekspert",
        _ => "Noma'lum"
    };

    public string ExperienceText => YearsOfExperience switch
    {
        null => "",
        0 => "< 1 yil",
        1 => "1 yil",
        var y when y >= 2 && y <= 4 => $"{y} yil",
        _ => "5+ yil"
    };
}