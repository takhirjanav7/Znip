using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.SKILL;

public interface ISkillRepository
{
    Task<List<Skill>> GetAllAsync(CancellationToken ct = default);
    Task<Skill?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Skill>> SearchAsync(string query, CancellationToken ct = default);
}