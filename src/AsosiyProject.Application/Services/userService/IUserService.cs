using AsosiyProject.Application.DTOs.UserDTOs;
using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.userService;

public interface IUserService
{
    Task<List<SafeUserDto>> GetAllAsync();
    Task<SafeUserDto?> GetByIdAsync(Guid id);
    Task<SafeUserDto?> GetByEmailAsync(string email);
    Task<SafeUserDto?> GetByUsernameAsync(string username);
    Task<bool> IsUsernameTakenAsync(string username);
    Task<List<SafeUserDto>> GetUsersBySkillAsync(string skillName, CancellationToken ct = default);
    Task<List<User>> GetTopRatedUsersAsync(int count);
    Task<List<User>> GetUsersWithProjectsAsync();
    Task<GetUserDto> UpdateAsync(UpdateUserDto dto);
    Task<bool> DeleteMyAccountAsync(Guid id, CancellationToken ct = default);
}