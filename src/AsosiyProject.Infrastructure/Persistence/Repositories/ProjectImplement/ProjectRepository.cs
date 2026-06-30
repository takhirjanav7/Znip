//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.ProjectImplement;

//public class ProjectRepository : IProjectRepository
//{
//    private readonly AppDbContext _context;
//    public ProjectRepository(AppDbContext context) => _context = context;

//    public async Task AddAsync(Project project, CancellationToken ct)
//        => await _context.Projects.AddAsync(project, ct);

//    public async Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct)
//        => await _context.Projects
//            .Include(p => p.Owner)
//            .Include(p => p.Contributors).ThenInclude(c => c.User)
//            .Include(p => p.Files)
//            .Include(p => p.ProjectSkills).ThenInclude(ps => ps.Skill)
//            .Include(p => p.Likes)
//            .Include(p => p.Comments)
//            .FirstOrDefaultAsync(p => p.ProjectId == id, ct);

//    public async Task<List<Project>> GetUserProjectsAsync(Guid userId, CancellationToken ct)
//        => await _context.Projects
//            .Where(p => p.OwnerId == userId || p.Contributors.Any(c => c.UserId == userId))
//            .Include(p => p.Owner)
//            .OrderByDescending(p => p.CreatedAt)
//            .ToListAsync(ct);

//    public Task UpdateAsync(Project project)
//    {
//        _context.Projects.Update(project);
//        return Task.CompletedTask;
//    }

//    public Task DeleteAsync(Project project)
//    {
//        _context.Projects.Remove(project);
//        return Task.CompletedTask;
//    }

//    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
//    => await _context.Projects
//        .FirstOrDefaultAsync(p => p.ProjectId == id, ct);
//}