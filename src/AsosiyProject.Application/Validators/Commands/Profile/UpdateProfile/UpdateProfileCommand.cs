using MediatR;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.Validators.Commands.Profile.UpdateProfile;

public record UpdateProfileCommand(
    string? FullName,
    string? Bio,
    string? Location,
    string? WebsiteUrl,
    string? GitHubUrl,
    string? LinkedInUrl,
    IFormFile? ProfilePicture
) : IRequest<Unit>;