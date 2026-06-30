using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.USERSKILL;

public interface IUserSkillRepository
{
    Task AddSkillAsync(Guid userId, Guid skillId, int level, CancellationToken ct = default);
    Task RemoveSkillAsync(Guid userId, Guid skillId);
    Task<List<UserSkill>> GetUserSkillsAsync(Guid userId, CancellationToken ct = default);
    Task UpdateLevelAsync(Guid userId, Guid skillId, int newLevel);
}