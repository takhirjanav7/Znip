//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.ProjectUserImplement;

//public class ProjectUserRepository : IProjectUserRepository
//{
//    private readonly AppDbContext _context;
//    public ProjectUserRepository(AppDbContext context)
//    {
//        _context = context;
//    }
//    public Task AddAsync(ProjectUser entity, CancellationToken ct = default)
//    {
//        throw new NotImplementedException();
//    }

//    public void Delete(ProjectUser entity)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<ProjectUser?> GetByProjectAndUserAsync(Guid projectId, Guid userId, CancellationToken ct = default)
//    => await _context.ProjectUsers
//        .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId, ct);
//}
