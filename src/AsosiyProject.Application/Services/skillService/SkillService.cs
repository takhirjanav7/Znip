using AsosiyProject.Application.Commands;
using AsosiyProject.Application.SignUp.Registration;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Services.skillService;

public class SkillService : ISkillService
{
    private readonly IAppDbContext _context;

    public SkillService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Skill>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Skills
            .AsNoTracking()
            .Include(s => s.Users)
                .ThenInclude(us => us.User)
            .ToListAsync(ct);     
    }
    public async Task<Skill?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // skill ni id boyicha userlar bilan birga oling
        return await _context.Skills
            .AsNoTracking()
            .Include(s => s.Users)
                .ThenInclude(us => us.User)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Skill> CreateAsync(CreateSkillDto dto, CancellationToken ct = default)
    {
        var name = dto.Name.Trim();

        var existingSkill = await _context.Skills
            .FirstOrDefaultAsync(s => EF.Functions.ILike(s.Name, name), ct);

        Skill skill;

        if (existingSkill != null)
        {
            skill = existingSkill;
        }
        else
        {
            skill = new Skill
            {
                Name = name,
                Category = dto.Category,
                IconUrl = dto.IconUrl
            };
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync(ct);
        }


        var userSkillExists = await _context.UserSkills
        .AnyAsync(us => us.UserId == dto.UserId && us.SkillId == skill.Id, ct);

        if (!userSkillExists)
        {
            var userSkill = new UserSkill
            {
                UserId = dto.UserId,
                SkillId = skill.Id,
                ProficiencyLevel = dto.ProficiencyLevel,
                YearsOfExperience = dto.YearsOfExperience ?? 0,
                AddedAt = DateTime.UtcNow
            };
            _context.UserSkills.Add(userSkill);
            await _context.SaveChangesAsync(ct);
        }

        return skill;
    }

    public async Task UpdateAsync(Guid id, UpdateSkillDto dto, CancellationToken ct = default)
    {
        var skill = await _context.Skills.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Skill with ID {id} not found");

        skill.Name = dto.Name?.Trim() ?? skill.Name;
        skill.Category = dto.Category ?? skill.Category;
        skill.IconUrl = dto.IconUrl ?? skill.IconUrl;

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var skill = await _context.Skills.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Skill with ID {id} not found");

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync(ct);
    }

    // Eng mashhur skill'lar (eng ko‘p foydalanuvchida borlari)
    public async Task<List<Skill>> GetPopularSkillsAsync(int count, CancellationToken ct = default)
        => await _context.Skills
            .AsNoTracking()
            .Include(s => s.Users)
            .OrderByDescending(s => s.Users.Count)
            .Take(count)
            .ToListAsync(ct);

    // Skill + unga ega bo‘lgan userlar
    public async Task<Skill?> GetSkillWithUsersAsync(string skillName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return null;

        var search = skillName.Trim().ToLower();

        return await _context.Skills
            .AsNoTracking()
            .Include(s => s.Users)
                .ThenInclude(us => us.User)
            .FirstOrDefaultAsync(s => EF.Functions.ILike(s.Name, $"%{search}%"), ct);
    }
    // Userda shu skill bormi?
    public async Task<bool> IsSkillLinkedToUserAsync(Guid userId, string skillName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return false;

        var search = $"%{skillName.Trim()}%";

        return await _context.UserSkills
            .AnyAsync(us =>
                us.UserId == userId &&
                EF.Functions.ILike(us.Skill.Name, search), ct);
    }
}