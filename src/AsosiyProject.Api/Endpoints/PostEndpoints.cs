using AsosiyProject.Application.DTOs.CommentDTOs;
using AsosiyProject.Application.DTOs.PostDTOs;
using AsosiyProject.Application.Interfaces.Services;
using AsosiyProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsosiyProject.Api.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/posts").WithTags("Post").RequireAuthorization(); // Token kerak


        // 1. HAMMA POSTLARNI OLISH (Feed)
        // URL: /api/posts?page=1&size=10
        group.MapGet("/", async ([AsParameters] PaginationParams p, [FromServices] IPostService service, HttpContext http) =>
        {
            // User ID ni olamiz (IsLiked ni bilish uchun)
            var userIdString = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Results.Unauthorized();
            var userId = Guid.Parse(userIdString);

            // Default qiymatlar: page=1, size=10
            int pageNumber = p.Page > 0 ? p.Page : 1;
            int pageSize = p.Size > 0 ? p.Size : 10;

            var posts = await service.GetAllPostsAsync(userId, pageNumber, pageSize);
            return Results.Ok(posts);
        });

        // 2. BITTA POSTNI OLISH
        // URL: /api/posts/{id}
        group.MapGet("/{id}", async (Guid id, [FromServices] IPostService service, HttpContext http) =>
        {
            var userIdString = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Results.Unauthorized();
            var userId = Guid.Parse(userIdString);

            try
            {
                var post = await service.GetPostByIdAsync(id, userId);
                return Results.Ok(post);
            }
            catch
            {
                return Results.NotFound("Post topilmadi");
            }
        });

        // Post yaratish
        group.MapPost("/", async ([FromForm] string? caption, [FromForm] MediaType mediaType, IFormFile file, [FromServices] IPostService service, HttpContext http) =>
        {
            // 1. Avval tokendan "id" claimsni qidiramiz
            var userIdString = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Agar id topilmasa, demak user login qilmagan yoki token noto'g'ri
            if (string.IsNullOrEmpty(userIdString))
            {
                return Results.Unauthorized();
            }

            // 3. Endi bemalol parse qilsa bo'ladi
            var userId = Guid.Parse(userIdString);

            // ... qolgan kodlar (fayl tekshirish va h.k.)
            if (file == null || file.Length == 0) return Results.BadRequest("Fayl yuklanmadi!");

            var dto = new CreatePostDto { Caption = caption, MediaType = mediaType, File = file };
            var postId = await service.CreatePostAsync(userId, dto);

            return Results.Ok(new { PostId = postId });
        })
        .DisableAntiforgery(); // <--- MUHIM: Buni qo'shish kerak!

        // Like bosish (Toggle)
        group.MapPost("/{id}/like", async (Guid id, [FromServices] IPostService service, HttpContext http) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            var isLiked = await service.ToggleLikeAsync(id, userId);

            return Results.Ok(new { IsLiked = isLiked, Message = isLiked ? "Like bosildi" : "Like olindi" });
        });

        // Comment yozish
        group.MapPost("/{id}/comment", async (Guid id, [FromBody] CreateCommentDto dto, [FromServices] IPostService service, HttpContext http) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return Results.Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            await service.AddCommentAsync(id, userId, dto.Text);

            return Results.Ok("Comment qo'shildi");
        });

        // View Count (Frontend postni ochganda shu yerga so'rov yuboradi)
        group.MapPost("/{id}/view", async (Guid id, [FromServices] IPostService service, HttpContext http) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return Results.Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            await service.IncreaseViewCountAsync(id, userId);

            return Results.Ok();
        });
    }
}

public class PaginationParams
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}