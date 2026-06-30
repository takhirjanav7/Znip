using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Services;
using AsosiyProject.Application.Services.notificationService;
using AsosiyProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsosiyProject.Api.Endpoints;

public static class FollowEndpoints
{
    public static void MapFollowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/follow").WithTags("Follow");

        // Follow qilish (async, DB orqali)
        //group.MapPost("/follow/{followerId}/{followingId}",
        //    async ([FromRoute] Guid followerId,
        //           [FromRoute] Guid followingId,
        //           [FromServices] IUserRepository repo
        //           /*[FromServices] INotificationService notifService*/) =>
        //    {
        //        var follower = await repo.GetByIdAsync(followerId);
        //        var following = await repo.GetByIdAsync(followingId);

        //        if (follower == null || following == null)
        //            return Results.NotFound("User not found");

        //        var existingFollow = (await repo.GetUserFollowingAsync(followerId))
        //                                 .FirstOrDefault(f => f.FollowingId == followingId);
        //        if (existingFollow != null)
        //            return Results.BadRequest("Already following");

        //        var follow = new Follow
        //        {
        //            FollowerId = follower.UserId,
        //            //Follower = follower,
        //            FollowingId = following.UserId,
        //            //Following = following,
        //            FollowedAt = DateTime.UtcNow
        //        };

        //        await repo.AddFollowAsync(follow);

        //        //// 🔔 Notification qo‘shish
        //        //await notifService.SendAsync(
        //        //    userId: following.UserId,       // kimga notification boradi
        //        //    title: "New Follower",
        //        //    message: $"{follower.UserName} sizni follow qildi",
        //        //    type: "Follow",
        //        //    actorId: follower.UserId
        //        //);

        //        return Results.Ok("Followed successfully");
        //    });

        group.MapPost("/{followingId:guid}", async (
        Guid followingId,
        HttpContext httpContext, // DIQQAT: HttpContent emas, HttpContext bo'lishi kerak
        [FromServices] IUserRepository repo,
        [FromServices] INotificationService notifService) =>
        {
            // 1. Current User ID ni tokendan olish
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            // Agar token noto'g'ri bo'lsa yoki ID bo'lmasa
            if (userIdClaim == null)
            {
                return Results.Unauthorized();
            }

            Guid currentUserId = Guid.Parse(userIdClaim.Value); // Bu followerId bo'ladi

            // O'ziga o'zi follow qilishni oldini olish
            if (currentUserId == followingId)
            {
                return Results.BadRequest("You cannot follow yourself.");
            }

            // 2. Userlarni bazadan tekshirish
            var follower = await repo.GetByIdAsync(currentUserId);
            var following = await repo.GetByIdAsync(followingId);

            if (follower == null || following == null)
                return Results.NotFound("User not found");

            // 3. Allaqachon follow qilinganligini tekshirish
            var existingFollow = (await repo.GetUserFollowingAsync(currentUserId))
                                     .FirstOrDefault(f => f.FollowingId == followingId);

            if (existingFollow != null)
                return Results.BadRequest("Already following");

            // 4. Follow yaratish
            var follow = new Follow
            {
                FollowerId = currentUserId,
                FollowingId = followingId,
                FollowedAt = DateTime.UtcNow
            };

            await repo.AddFollowAsync(follow);

            // 5. Notification yuborish
            await notifService.SendAsync(
                userId: following.UserId,
                title: "New Follower",
                message: $"{follower.UserName} sizni follow qildi",
                type: "Follow",
                actorId: follower.UserId
            );

            return Results.Ok("Followed successfully");
        });


        // Unfollow qilish (async, DB orqali)
        group.MapDelete("/unfollow/{followerId}/{followingId}",
            async ([FromRoute] Guid followerId,
                   [FromRoute] Guid followingId,
                   [FromServices] IUserRepository repo) =>
            {
                var removed = await repo.RemoveFollowAsync(followerId, followingId);
                if (!removed)
                    return Results.NotFound("Follow relation not found");

                return Results.Ok("Unfollowed successfully");
            });
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        // User feed
        //group.MapGet("/feed/{username}",                                                                                          
        //    async ([FromRoute] string username,
        //     [FromServices] FeedService feedService,
        //     [FromServices] IUserRepository repo) =>
        //    {   
        //        var user = await repo.GetByUsernameAsync(username);
        //        if (user == null)
        //            return Results.NotFound("User not found");

        //        var feed = await feedService.GenerateFeed(user);
        //        var result = feed.Select(p => new
        //        {
        //            p.Content,
        //            Likes = p.Likes.Count,
        //            Comments = p.Comments.Count,
        //            p.CreatedAt
        //        });
        //        return Results.Ok(result);
        //    });

        // Foydalanuvchining Following ro'yxati
        group.MapGet("/user/{userId}/following",
            async ([FromRoute] Guid userId, [FromServices] IUserRepository repo) =>
            {
                var following = (await repo.GetUserFollowingAsync(userId))
                                        .Select(f => f.Following?.UserName)
                                        .Where(n => n != null)
                                        .ToList();
                return Results.Ok(following);
            });

        // Foydalanuvchining Followers ro'yxati
        group.MapGet("/user/{userId}/followers",
            async ([FromRoute] Guid userId, [FromServices] IUserRepository repo) =>
            {
                var followers = (await repo.GetUserFollowersAsync(userId))
                                        .Select(f => f.Follower?.UserName)
                                        .Where(n => n != null)
                                        .ToList();
                return Results.Ok(followers);
            });
    }
}