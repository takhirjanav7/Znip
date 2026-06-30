//using AsosiyProject.Application.Interfaces.USER;
//using AsosiyProject.Domain.Entities;

//namespace AsosiyProject.Application.Services;

//public class FeedService
//{
//    private readonly IUserRepository _userRepo;

//    public FeedService(IUserRepository userRepo)
//    {
//        _userRepo = userRepo;
//    }

//    public async Task<List<Post>> GenerateFeed(User user)
//    {
//        var following = (await _userRepo.GetUserFollowingAsync(user.UserId))
//                                 .Select(f => f.Following)
//                                 .Where(u => u != null)
//                                 .ToList();

//        var feed = new List<Post>();
//        foreach (var followed in following)
//        {
//            if (followed.Posts != null && followed.Posts.Any())
//            {
//                feed.AddRange(followed.Posts.Where(p => !p.IsArchived));
//            }
//        }

//        // Like + Comment asosida sort qilish
//        feed = feed
//            .OrderByDescending(p => (p.Likes?.Count ?? 0) + (p.Comments?.Count ?? 0))
//            .ThenByDescending(p => p.CreatedAt)
//            .ToList();

//        return feed;
//    }
//}