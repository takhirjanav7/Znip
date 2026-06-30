using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.DTOs.UserDTOs;
using AsosiyProject.Application.Interfaces.UNITWORK;
using AsosiyProject.Application.SignUp.Registration;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Services.userService;

public class UserService : IUserService
{
    private readonly IAppDbContext _context;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UserService(IAppDbContext appDbContext, IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _context = appDbContext;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<List<SafeUserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .ToListAsync();

        return users.Select(SafeUserDto.FromUser).ToList();
    }

    public async Task<SafeUserDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.UserId == id);

        return user is null ? null : SafeUserDto.FromUser(user);
    }
    public async Task<SafeUserDto?> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        return user is null ? null : SafeUserDto.FromUser(user);
    }
    public async Task<SafeUserDto?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

        return user is null ? null : SafeUserDto.FromUser(user);
    }
    public async Task<bool> IsUsernameTakenAsync(string username)
        => await _uow.Users.IsUsernameTakenAsync(username);

    public async Task<List<SafeUserDto>> GetUsersBySkillAsync(string skillName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return new List<SafeUserDto>();

        var search = skillName.Trim().ToLower();

        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .Where(u => u.Skills.Any(us =>
                us.Skill != null &&
                us.Skill.Name != null &&
                us.Skill.Name.ToLower().Contains(search)))
            .ToListAsync(ct);

        return user.Select(SafeUserDto.FromUser).ToList();
    }

    public async Task<List<User>> GetTopRatedUsersAsync(int count)
        => await _uow.Users.GetTopRatedUsersAsync(count);

    public async Task<List<User>> GetUsersWithProjectsAsync()
        => await _uow.Users.GetUsersWithProjectsAsync();

    public async Task<GetUserDto?> UpdateAsync(UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);

        if (user == null)
            return null;

        // FullName → FirstName + LastName
        if (!string.IsNullOrWhiteSpace(dto.FullName))
        {
            var parts = dto.FullName.Trim().Split(' ', 2);

            user.FirstName = parts[0];
            user.LastName = parts.Length > 1 ? parts[1] : "";
        }

        // Username
        if (!string.IsNullOrWhiteSpace(dto.Username))
            user.UserName = dto.Username;

        // Bio
        if (dto.Bio != null)
            user.Bio = dto.Bio;

        // Profile and Cover images
        if (dto.ProfileImageUrl != null)
            user.ProfilePictureUrl = dto.ProfileImageUrl;

        if (dto.CoverImageUrl != null)
            user.CoverPictureUrl = dto.CoverImageUrl;

        // Location
        if (dto.Location != null)
            user.Location = dto.Location;

        // Social links
        if (dto.WebsiteUrl != null)
            user.WebsiteUrl = dto.WebsiteUrl;

        if (dto.GitHubUrl != null)
            user.GitHubUrl = dto.GitHubUrl;

        if (dto.LinkedInUrl != null)
            user.LinkedInUrl = dto.LinkedInUrl;

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var resultDto = new GetUserDto
        {
            UserId = user.UserId,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Username = user.UserName,
            Email = user.Email,
            Role = user.Role.ToString(),
            ProfileImageUrl = user.ProfilePictureUrl
        };

        return resultDto;
    }

    public async Task<bool> DeleteMyAccountAsync(Guid id, CancellationToken ct = default) => await _uow.Users.SoftDeleteAsync(id, ct);
}