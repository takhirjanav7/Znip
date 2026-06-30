using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.searchService;

public interface ISearchService
{
    Task<SearchResult> SearchAsync(string query, int page = 1, int pageSize = 20, CancellationToken ct = default);
}

public record SearchResult(
    List<User> Users,
    List<Skill> Skill,
    int TotalCount
);