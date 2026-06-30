//using Microsoft.EntityFrameworkCore;

//namespace AsosiyProject.Infrastructure.Persistence.Repositories.CommentImplement;

//public class CommentRepository : ICommentRepository
//{
//    private readonly AppDbContext _context;
//    public CommentRepository(AppDbContext context) => _context = context;

//    public async Task AddAsync(Comment comment, CancellationToken ct)
//        => await _context.Comments.AddAsync(comment, ct);

//    public async Task<Comment?> GetByIdWithRepliesAsync(Guid id, CancellationToken ct)
//        => await _context.Comments
//            .Include(c => c.User)
//            .Include(c => c.Replies).ThenInclude(r => r.User)
//            .Include(c => c.Likes)
//            .FirstOrDefaultAsync(c => c.Id == id, ct);

//    public Task DeleteAsync(Comment comment)
//    {
//        _context.Comments.Remove(comment);
//        return Task.CompletedTask;
//    }
//}