using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Validators.Commands.Profile.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileService _fileService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public UpdateProfileCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUser,
        IFileService fileService,
        IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _currentUser = currentUser;
        _fileService = fileService;
        _hubContext = hubContext;
    }

    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId, ct)
            ?? throw new KeyNotFoundException("Foydalanuvchi topilmadi!");

        // Yangi ma'lumotlarni o‘zgartirish
        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            var parts = request.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            user.FirstName = parts[0];
            user.LastName = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";
        }

        if (request.Bio != null)
            user.Bio = request.Bio.Trim();

        if (request.Location != null)
            user.Location = request.Location.Trim();

        if (request.WebsiteUrl != null)
            user.WebsiteUrl = request.WebsiteUrl.Trim();

        if (request.GitHubUrl != null)
            user.GitHubUrl = request.GitHubUrl.Trim();

        if (request.LinkedInUrl != null)
            user.LinkedInUrl = request.LinkedInUrl.Trim();

        // Profil rasmini yuklash
        if (request.ProfilePicture != null)
        {
            // Eski rasmini o‘chirish (ixtiyoriy)
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                await _fileService.DeleteAsync(user.ProfilePictureUrl, ct);

            var uploaded = await _fileService.UploadAsync(request.ProfilePicture, ct);
            user.ProfilePictureUrl = uploaded.FileUrl;
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        // Real-time bildirishnoma (followerslarga)
        await _hubContext.Clients.Group($"Followers_{userId}")
            .SendAsync("ProfileUpdated", new
            {
                UserId = userId,
                FullName = user.FullName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
            });

        return Unit.Value;
    }
}