using AsosiyProject.Application.Interfaces.USERSKILL;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.UserSkillImplement;

public class UserSkillRepository : IUserSkillRepository
{
    private readonly AppDbContext _context;
    public UserSkillRepository(AppDbContext context) => _context = context;

    public async Task AddSkillAsync(Guid userId, Guid skillId, int level, CancellationToken ct)
    {
        var userSkill = new UserSkill
        {
            UserId = userId,
            SkillId = skillId,
            ProficiencyLevel = level,
            AddedAt = DateTime.UtcNow
        };
        await _context.UserSkills.AddAsync(userSkill, ct);
    }

    public Task RemoveSkillAsync(Guid userId, Guid skillId)
    {
        var userSkill = _context.UserSkills.FirstOrDefault(us => us.UserId == userId && us.SkillId == skillId);
        if (userSkill != null) _context.UserSkills.Remove(userSkill);
        return Task.CompletedTask;
    }

    public async Task<List<UserSkill>> GetUserSkillsAsync(Guid userId, CancellationToken ct)
        => await _context.UserSkills
            .Include(us => us.Skill)
            .Where(us => us.UserId == userId)
            .ToListAsync(ct);

    public Task UpdateLevelAsync(Guid userId, Guid skillId, int newLevel)
    {
        var userSkill = _context.UserSkills.FirstOrDefault(us => us.UserId == userId && us.SkillId == skillId);
        if (userSkill != null) userSkill.ProficiencyLevel = newLevel;
        return Task.CompletedTask;
    }
}