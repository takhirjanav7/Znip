using AsosiyProject.Application.Interfaces.SKILL;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.SkillImplement;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _context;
    public SkillRepository(AppDbContext context) => _context = context;

    public async Task<List<Skill>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Skills
            .AsNoTracking()
            .Include(s => s.Users)
                .ThenInclude(us => us.User)
            .ToListAsync(ct);
    }

    public Task<Skill?> GetByIdAsync(Guid id, CancellationToken ct)
        => _context.Skills.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<List<Skill>> SearchAsync(string query, CancellationToken ct)
        => await _context.Skills
            .Where(s => s.Name.Contains(query) || (s.Category != null && s.Category.Contains(query)))
            .Take(20)
            .ToListAsync(ct);
}