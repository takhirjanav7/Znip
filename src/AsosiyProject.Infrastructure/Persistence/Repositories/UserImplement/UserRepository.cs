using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Infrastructure.Persistence.Repositories.UserImplement;

public class UserRepository : IUserRepository 
{
    private readonly AppDbContext _context;
    private readonly List<User> _users = new();
    private readonly List<Follow> _follows = new();

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.UserId == id && !u.IsDeleted, ct);
    }
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.UserName == username, ct);
    }
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);
    }
    public async Task<bool> IsUsernameTakenAsync(string username, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.UserName == username, ct);

    public async Task CreateAsync(User user, CancellationToken ct = default)
        => await _context.Users.AddAsync(user, ct);

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
        => await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync(ct);

    public async Task<List<User>> GetUsersBySkillAsync(string skillName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return new List<User>();

        var search = skillName.Trim().ToLower();

        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
                .ThenInclude(us => us.Skill)
            .Where(u => u.Skills.Any(us =>
                us.Skill != null &&
                us.Skill.Name != null &&
                us.Skill.Name.ToLower().Contains(search)))
            .ToListAsync(ct);
    }

    public async Task<List<User>> GetTopRatedUsersAsync(int count, CancellationToken ct = default)
        => await _context.Users
            .AsNoTracking()
            .Where(u => u.TotalRatings > 0)
            .OrderByDescending(u => u.Rating)
            .ThenByDescending(u => u.TotalRatings)
            .Take(count)
            .ToListAsync(ct);

    //    public async Task<List<User>> GetTopRatedUsersAsync(int count, CancellationToken ct = default)
    //    {
    //        return await _context.Users
    //            .AsNoTracking()
    //            .OrderByDescending(u => u.TotalRatings > 0 ? u.Rating : 0)           // bahosi bo‘lganlar oldin
    //            .ThenByDescending(u => u.TotalRatings)                               // ko‘p baho olganlar yuqori
    //            .ThenByDescending(u => u.CreatedAt)                                  // teng bo‘lsa — eski userlar
    //            .Take(count)
    //            .ToListAsync(ct);
    //    }
    //    Bu eng zo‘r variant — chunki:

    //Agar baho bo‘lsa — odatdagidek ishlaydi
    //Agar baho yo‘q bo‘lsa — eng eski(birinchi ro‘yxatdan chiqqan) userlar chiqadi
    //Hech qachon bo‘sh ro‘yxat qaytmaydi(agar userlar bo‘lsa)

    //Misol:


    //User            Rating  TotalRatings    CreatedAt       Chiqishtartibi 

    //Abdulaziz       0           0           2025-01-01      1-o‘rin
    //Vali            4.9         50          2025-03-01      1-o‘rin(bahosi bor)

    //Sarvar          0           0           2025-02-01      2-o‘rin

    public async Task<List<User>> GetUsersWithProjectsAsync(CancellationToken ct = default)
        => await _context.Users
            .AsNoTracking()
            .Where(u => u.Posts.Any())
            .OrderByDescending(u => u.Posts.Count)
            .ToListAsync(ct);

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken ct = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u =>
                u.UserName == usernameOrEmail ||
                u.Email == usernameOrEmail, ct);
    }

    public async Task<bool> SoftDeleteAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user == null)
            return false;

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task AddFollowAsync(Follow follow, CancellationToken ct = default)
    {
        await _context.Follows.AddAsync(follow, ct);
        var saved = await _context.SaveChangesAsync(ct);
        if (saved <= 0)
        {
            throw new InvalidOperationException("Failed to save Follow to database (SaveChanges returned 0).");
        }
    }

    public async Task<List<Follow>> GetUserFollowingAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.Follows
            .Include(f => f.Following)
                .ThenInclude(u => u.Posts)
                    .ThenInclude(p => p.Likes)
            .Include(f => f.Following)
                .ThenInclude(u => u.Posts)
                    .ThenInclude(p => p.Comments)
            .Include(f => f.Follower)
            .Where(f => f.FollowerId == userId)
            .ToListAsync(ct);
    }

    public async Task<List<Follow>> GetUserFollowersAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.Follows
            .Include(f => f.Following)
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == userId)
            .ToListAsync(ct);
    }

    public async Task<bool> RemoveFollowAsync(Guid followerId, Guid followingId, CancellationToken ct = default)
    {
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, ct);
        if (follow == null) return false;

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}