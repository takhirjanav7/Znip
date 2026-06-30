using AsosiyProject.Application.SignUp.Registration;
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.skillService;

public interface ISkillService
{
    Task<List<Skill>> GetAllAsync(CancellationToken ct = default);
    Task<Skill?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Skill> CreateAsync(CreateSkillDto dto, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateSkillDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<List<Skill>> GetPopularSkillsAsync(int count, CancellationToken ct = default);
    Task<Skill?> GetSkillWithUsersAsync(string skillName, CancellationToken ct = default);
    Task<bool> IsSkillLinkedToUserAsync(Guid userId, string skillName, CancellationToken ct = default);
}