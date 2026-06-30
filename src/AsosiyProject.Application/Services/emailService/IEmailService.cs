using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.Services.emailService;

public interface IEmailService
{
    Task SendVerificationEmailAsync(User user, string token, CancellationToken ct = default);
    Task SendPasswordResetAsync(User user, string token, CancellationToken ct = default);
    Task SendInviteAsync(string email, Guid projectId, string inviterName, CancellationToken ct = default);
    Task SendWelcomeEmailAsync(User user, CancellationToken ct = default);
}