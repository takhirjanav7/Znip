
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.activityService;

public interface IActivityService
{
    Task LogAsync(string action, Guid? targetId = null, string? targetType = null, CancellationToken ct = default);
    Task<List<Activity>> GetUserActivityAsync(Guid userId, int take = 50, CancellationToken ct = default);
}