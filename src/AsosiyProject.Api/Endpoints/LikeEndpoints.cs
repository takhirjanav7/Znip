//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Services.likeService;

//namespace AsosiyProject.Api.Endpoints;

//public static class LikeEndpoints
//{
//    public static void MapLikeEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/api/posts")
//                       .WithTags("Likes")
//                       .RequireAuthorization();

//        // 1. POSTGA LIKE BOSISH
//        group.MapPost("/{postId:guid}/like", async (
//            Guid postId,
//            ILikeService likeService,
//            ICurrentUserService currentUser) =>
//        {
//            await likeService.LikePostAsync(postId, currentUser.UserId!.Value);
//            var count = await likeService.GetLikeCountAsync(postId);
//            return Results.Ok(new { Message = "Liked!", TotalLikes = count });
//        })
//        .WithName("LikePost")
//        .Produces(200);

//        // 2. POSTDAN LIKE NI OLISH (Unlike)
//        group.MapDelete("/{postId:guid}/like", async (
//            Guid postId,
//            ILikeService likeService,
//            ICurrentUserService currentUser) =>
//        {
//            await likeService.UnlikePostAsync(postId, currentUser.UserId!.Value);
//            var count = await likeService.GetLikeCountAsync(postId);
//            return Results.Ok(new { Message = "Unliked", TotalLikes = count });
//        })
//        .WithName("UnlikePost")
//        .Produces(200);

//        // 3. POSTNING LIKE SONINI OLISH
//        group.MapGet("/{postId:guid}/likes/count", async (
//            Guid postId,
//            ILikeService likeService) =>
//        {
//            var count = await likeService.GetLikeCountAsync(postId);
//            return Results.Ok(new { PostId = postId, TotalLikes = count });
//        })
//        .WithName("GetLikeCount")
//        .Produces(200);

//        // 4. FOYDALANUVCHI BU POSTGA LIKE BOSGANMI? (frontend uchun)
//        group.MapGet("/{postId:guid}/likes/me", async (
//            Guid postId,
//            ILikeService likeService,
//            ICurrentUserService currentUser) =>
//        {
//            var isLiked = await likeService.IsPostLikedByUserAsync(postId, currentUser.UserId!.Value);
//            return Results.Ok(new { PostId = postId, IsLiked = isLiked });
//        })
//        .WithName("IsPostLiked")
//        .Produces(200);

//        // 5. POSTGA KIMLAR LIKE BOSGAN (pagination bilan)
//        group.MapGet("/{postId:guid}/likes", async (
//            Guid postId,
//            ILikeService likeService,
//            int skip = 0,
//            int take = 50) =>
//        {
//            var userIds = await likeService.GetLikersAsync(postId, skip, take);
//            return Results.Ok(new { PostId = postId, Likers = userIds, Total = userIds.Count });
//        })
//        .WithName("GetLikers")
//        .Produces(200);
//    }
//}