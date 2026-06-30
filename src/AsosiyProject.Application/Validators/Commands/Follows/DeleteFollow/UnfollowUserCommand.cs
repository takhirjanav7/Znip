using MediatR;

namespace AsosiyProject.Application.Validators.Commands.Follows.DeleteFollow;

public record UnfollowUserCommand(
    Guid TargetUserId
    ) :IRequest<Unit>;