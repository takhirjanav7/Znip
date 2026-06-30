using AsosiyProject.Application.Commands;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Services.searchService;

public class SearchService : ISearchService
{
    private readonly IAppDbContext _context;
    public SearchService(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<SearchResult> SearchAsync(string query, int page, int pageSize, CancellationToken ct)
    {
        var q = query.Trim().ToLower();
        var users = await _context.Users.Where(u => u.FullName.ToLower().Contains(q) || u.UserName.Contains(q)).Take(10).ToListAsync(ct);
        var skills = await _context.Skills.Where(s => s.Name.ToLower().Contains(q)).Take(10).ToListAsync(ct);

        return new SearchResult(users, skills, users.Count + skills.Count);
    }
}
