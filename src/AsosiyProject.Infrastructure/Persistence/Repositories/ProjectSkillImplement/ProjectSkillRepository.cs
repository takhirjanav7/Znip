//namespace AsosiyProject.Infrastructure.Persistence.Repositories.ProjectSkillImplement;

//// 8. ProjectSkillRepository.cs
//public class ProjectSkillRepository : IProjectSkillRepository
//{
//    private readonly AppDbContext _context;
//    public ProjectSkillRepository(AppDbContext context) => _context = context;

//    public async Task AddRangeAsync(IEnumerable<ProjectSkill> skills, CancellationToken ct = default)
//    {
//        await _context.ProjectSkills.AddRangeAsync(skills, ct);
//    }

//    public async Task AddSkillAsync(Guid projectId, Guid skillId, CancellationToken ct)
//    {
//        var ps = new ProjectSkill { ProjectId = projectId, SkillId = skillId };
//        await _context.ProjectSkills.AddAsync(ps, ct);
//    }

//    public Task RemoveSkillAsync(Guid projectId, Guid skillId)
//    {
//        var ps = _context.ProjectSkills.FirstOrDefault(x => x.ProjectId == projectId && x.SkillId == skillId);
//        if (ps != null) _context.ProjectSkills.Remove(ps);
//        return Task.CompletedTask;
//    }
//}