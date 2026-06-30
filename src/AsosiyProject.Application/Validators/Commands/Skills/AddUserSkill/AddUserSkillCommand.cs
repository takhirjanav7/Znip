using MediatR;

namespace AsosiyProject.Application.Validators.Commands.Skills.AddUserSkill;

public record AddUserSkillCommand(
    Guid SkillId,
    int ProficiencyLevel   
) : IRequest<Unit>;