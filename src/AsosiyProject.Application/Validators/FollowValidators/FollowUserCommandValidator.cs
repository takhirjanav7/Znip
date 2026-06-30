using AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;
using FluentValidation;

namespace AsosiyProject.Application.Validators.FollowValidators;

public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Foydalanuvchi topilmadi!")
            .NotEqual(x => x.TargetUserId).WithMessage("O‘zingizni follow qila olmaysiz!");
    }
}