using AsosiyProject.Application.SignUp.Registration;

namespace AsosiyProject.Application.DTOs.SkillDTOs;

public record ProfileSkillsResponse(
    IReadOnlyList<GetSkillWithUsersDto> Skills,
    int TotalSkills,
    string TopCategory = "Frontend" // eng ko‘p skill bor kategoriya
);