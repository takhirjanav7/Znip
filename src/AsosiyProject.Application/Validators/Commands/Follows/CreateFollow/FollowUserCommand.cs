using MediatR;

namespace AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;

public record FollowUserCommand(
    Guid TargetUserId   // kimni follow qilmoqchimiz
) : IRequest<Unit>;