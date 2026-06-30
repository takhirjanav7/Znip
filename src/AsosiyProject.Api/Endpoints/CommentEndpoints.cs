//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Services.commentService;

//namespace AsosiyProject.Api.Endpoints;

//public static class CommentEndpoints
//{
//    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/api/comments")
//                       .WithTags("Comments")
//                       .RequireAuthorization();

//        // POSTGA COMMENT
//        group.MapPost("/post/{postId:guid}", async (Guid postId, CreateCommentDto dto, ICommentService service, ICurrentUserService user) =>
//        {
//            var id = await service.CreateCommentAsync(postId: postId, text: dto.Text, parentCommentId: dto.ParentCommentId);
//            return Results.Created($"/api/comments/{id}", new { Id = id });
//        });

//        // PROJECTGA COMMENT
//        group.MapPost("/project/{projectId:guid}", async (Guid projectId, CreateCommentDto dto, ICommentService service, ICurrentUserService user) =>
//        {
//            var id = await service.CreateCommentAsync(projectId: projectId, text: dto.Text, parentCommentId: dto.ParentCommentId);
//            return Results.Created($"/api/comments/{id}", new { Id = id });
//        });

//        // COMMENTLARNI OLISH
//        group.MapGet("/", async (Guid? postId, Guid? projectId, ICommentService service, int skip = 0, int take = 20) =>
//        {
//            var comments = await service.GetCommentsAsync(postId, projectId, skip, take);
//            return Results.Ok(comments);
//        });

//        // COMMENTNI TAHRIRLASH
//        group.MapPut("/{commentId:guid}", async (Guid commentId, UpdateCommentDto dto, ICommentService service, ICurrentUserService user) =>
//        {
//            await service.EditCommentAsync(commentId, dto.Text, user.UserId!.Value);
//            return Results.Ok();
//        });

//        // COMMENTNI O‘CHIRISH
//        group.MapDelete("/{commentId:guid}", async (Guid commentId, ICommentService service, ICurrentUserService user) =>
//        {
//            await service.DeleteCommentAsync(commentId, user.UserId!.Value);
//            return Results.NoContent();
//        });

//        // COMMENTGA LIKE
//        group.MapPost("/{commentId:guid}/like", async (Guid commentId, ICommentService service, ICurrentUserService user) =>
//        {
//            await service.LikeCommentAsync(commentId, user.UserId!.Value);
//            var count = await service.GetCommentLikeCountAsync(commentId);
//            return Results.Ok(new { Likes = count });
//        });

//        // COMMENTDAN LIKE OLISH
//        group.MapDelete("/{commentId:guid}/like", async (Guid commentId, ICommentService service, ICurrentUserService user) =>
//        {
//            await service.UnlikeCommentAsync(commentId, user.UserId!.Value);
//            var count = await service.GetCommentLikeCountAsync(commentId);
//            return Results.Ok(new { Likes = count });
//        });
//    }
//}

//public record CreateCommentDto(string Text, Guid? ParentCommentId = null);
//public record UpdateCommentDto(string Text);