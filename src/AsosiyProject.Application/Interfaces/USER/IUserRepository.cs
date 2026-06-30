
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Interfaces.USER;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> IsUsernameTakenAsync(string username, CancellationToken ct = default);
    Task CreateAsync(User user, CancellationToken ct = default);
    Task<User> UpdateAsync(User user);
    Task<bool> SoftDeleteAsync(Guid userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<List<User>> GetUsersBySkillAsync(string skillName, CancellationToken ct = default);
    Task<List<User>> GetTopRatedUsersAsync(int count, CancellationToken ct = default);
    Task<List<User>> GetUsersWithProjectsAsync(CancellationToken ct = default);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken ct = default);



    // Follow-related methods
    Task AddFollowAsync(Follow follow, CancellationToken ct = default);
    Task<List<Follow>> GetUserFollowingAsync(Guid userId, CancellationToken ct = default);
    Task<List<Follow>> GetUserFollowersAsync(Guid userId, CancellationToken ct = default);
    Task<bool> RemoveFollowAsync(Guid followerId, Guid followingId, CancellationToken ct = default);
}