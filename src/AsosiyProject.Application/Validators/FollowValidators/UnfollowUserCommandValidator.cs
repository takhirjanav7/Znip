using AsosiyProject.Application.Validators.Commands.Follows.DeleteFollow;
using FluentValidation;

namespace AsosiyProject.Application.Validators.FollowValidators;

public class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
{
    public UnfollowUserCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Foydalanuvchi ID bo‘sh bo‘lmasligi kerak!")
            .NotEqual(Guid.Empty).WithMessage("Noto‘g‘ri ID!");
    }
}