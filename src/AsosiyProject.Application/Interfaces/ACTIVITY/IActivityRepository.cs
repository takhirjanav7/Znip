using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.ACTIVITY;

public interface IActivityRepository
{
    Task AddAsync(Activity activity, CancellationToken ct = default);
    Task<List<Activity>> GetRecentAsync(Guid userId, int take = 50, CancellationToken ct = default);
}