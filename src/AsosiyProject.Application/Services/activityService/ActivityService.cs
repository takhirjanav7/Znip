using AsosiyProject.Application.Common.Exceptions;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.activityService;

public class ActivityService : IActivityService
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public ActivityService(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task LogAsync(string action, Guid? targetId, string? targetType, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var activity = new Activity
        {
            UserId = userId,
            Action = action,
            TargetId = targetId.HasValue ? targetId.Value.ToString() : string.Empty,
            TargetType = targetType?.ToString() ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };
        await _uow.Activities.AddAsync(activity, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public Task<List<Activity>> GetUserActivityAsync(Guid userId, int take, CancellationToken ct)
        => _uow.Activities.GetRecentAsync(userId, take, ct);
}