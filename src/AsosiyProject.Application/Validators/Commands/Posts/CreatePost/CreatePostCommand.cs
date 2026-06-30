using MediatR;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.Validators.Commands.Posts.CreatePost;

public record CreatePostCommand(
    string Content,
    IFormFile[]? Files = null,           
    Guid? ProjectId = null               
) : IRequest<Guid>;