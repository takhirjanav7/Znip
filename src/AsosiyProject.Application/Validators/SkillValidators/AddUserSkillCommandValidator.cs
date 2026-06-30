using AsosiyProject.Application.Validators.Commands.Skills.AddUserSkill;
using FluentValidation;

namespace AsosiyProject.Application.Validators.SkillValidators;

public class AddUserSkillCommandValidator : AbstractValidator<AddUserSkillCommand>
{
    public AddUserSkillCommandValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("Skill tanlanmagan!")
            .NotEqual(Guid.Empty).WithMessage("Noto‘g‘ri skill ID!");

        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(1, 5)
            .WithMessage("Daraja 1 (Beginner) dan 5 (Expert) gacha bo‘lishi kerak!");
    }
}