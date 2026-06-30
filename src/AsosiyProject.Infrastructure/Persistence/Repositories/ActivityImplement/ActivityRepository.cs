using AsosiyProject.Application.Interfaces.ACTIVITY;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.ActivityImplement;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;
    public ActivityRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Activity activity, CancellationToken ct = default)
        => await _context.Activities.AddAsync(activity, ct);

    public async Task<List<Activity>> GetRecentAsync(Guid userId, int take = 50, CancellationToken ct = default)
        => await _context.Activities
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
}