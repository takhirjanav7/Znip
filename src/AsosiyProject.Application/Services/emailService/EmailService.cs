using AsosiyProject.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AsosiyProject.Application.Services.emailService;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtp;
    private readonly string _fromEmail;
    private readonly string _appUrl;

    public EmailService(IConfiguration config)
    {
        _fromEmail = config["Email:From"] ?? "no-reply@asosiyproject.uz";
        _appUrl = config["App:Url"] ?? "https://asosiyproject.uz";

        _smtp = new SmtpClient(config["Email:Smtp:Host"], int.Parse(config["Email:Smtp:Port"] ?? "587"))
        {
            Credentials = new NetworkCredential(
                config["Email:Smtp:Username"],
                config["Email:Smtp:Password"]
            ),
            EnableSsl = bool.Parse(config["Email:Smtp:EnableSsl"] ?? "true")
        };
    }

    public async Task SendVerificationEmailAsync(User user, string token, CancellationToken ct = default)
    {
        var link = $"{_appUrl}/auth/verify-email?token={token}";
        var subject = "Emailingizni tasdiqlang - AsosiyProject";
        var body = $@"
            <h2>Salom, {user.FullName}!</h2>
            <p>AsosiyProject platformasida ro‘yxatdan o‘tdingiz.</p>
            <p>Emailingizni tasdiqlash uchun quyidagi tugmani bosing:</p>
            <p style='text-align:center;'>
                <a href='{link}' style='padding:15px 30px; background:#007bff; color:white; text-decoration:none; border-radius:8px; font-size:18px;'>
                    Emailni tasdiqlash
                </a>
            </p>
            <p>Agar siz ro‘yxatdan o‘tmagan bo‘lsangiz — bu xatni e’tiborsiz qoldiring.</p>
            <hr>
            <small>AsosiyProject — O‘zbekistonning professional tarmoq platformasi</small>
        ";

        await SendEmailAsync(user.Email!, subject, body, ct);
    }

    public async Task SendPasswordResetAsync(User user, string token, CancellationToken ct = default)
    {
        var link = $"{_appUrl}/auth/reset-password?token={token}";
        var subject = "Parolni tiklash - AsosiyProject";
        var body = $@"
            <h2>Parolni tiklash</h2>
            <p>Salom, {user.FullName}!</p>
            <p>Akkountingiz uchun parolni tiklash so‘rovi keldi.</p>
            <p>Yangi parol o‘rnatish uchun quyidagi tugmani bosing:</p>
            <p style='text-align:center;'>
                <a href='{link}' style='padding:15px 30px; background:#dc3545; color:white; text-decoration:none; border-radius:8px; font-size:18px;'>
                    Parolni tiklash
                </a>
            </p>
            <p>Havola 1 soat ichida amal qiladi.</p>
        ";

        await SendEmailAsync(user.Email!, subject, body, ct);
    }

    public async Task SendInviteAsync(string email, Guid projectId, string inviterName, CancellationToken ct = default)
    {
        var link = $"{_appUrl}/projects/{projectId}/join";
        var subject = $"{inviterName} sizni loyihaga taklif qildi!";
        var body = $@"
            <h2>Yangi taklif!</h2>
            <p><strong>{inviterName}</strong> sizni AsosiyProject’dagi loyihasiga qo‘shilishga taklif qildi.</p>
            <p>Loyihani ko‘rish va qo‘shilish uchun:</p>
            <p style='text-align:center;'>
                <a href='{link}' style='padding:15px 30px; background:#28a745; color:white; text-decoration:none; border-radius:8px; font-size:18px;'>
                    Loyihaga qo‘shilish
                </a>
            </p>
            <p>Rahmat!</p>
        ";

        await SendEmailAsync(email, subject, body, ct);
    }

    public async Task SendWelcomeEmailAsync(User user, CancellationToken ct = default)
    {
        var subject = $"Xush kelibsiz, {user.FullName}!";
        var body = $@"
            <h2>Xush kelibsiz, {user.FullName}!</h2>
            <p>O‘zbekistonning eng katta professional tarmoq platformasiga qo‘shilganingiz bilan tabriklaymiz!</p>
            <p>Endi siz:</p>
            <ul>
                <li>Loyihalar yaratishingiz</li>
                <li>Mutaxassislar bilan bog‘lanishingiz</li>
                <li>Portfolio qurishingiz mumkin</li>
            </ul>
            <p style='text-align:center;'>
                <a href='{_appUrl}' style='padding:15px 30px; background:#007bff; color:white; text-decoration:none; border-radius:8px; font-size:18px;'>
                    Platformani ko‘rish
                </a>
            </p>
            <p>Muvaffaqqiyatli ish!</p>
            <strong>AsosiyProject jamoasi</strong>
        ";

        await SendEmailAsync(user.Email!, subject, body, ct);
    }

    private async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        var mail = new MailMessage(_fromEmail, to, subject, body)
        {
            IsBodyHtml = true,
            DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
        };

        try
        {
            await _smtp.SendMailAsync(mail, ct);
        }
        catch (Exception ex)
        {
            // Logga yozish (Serilog ishlatayotganingiz uchun avto yoziladi)
            // Lekin productionda shunday qoldiring
            throw new InvalidOperationException($"Email jo‘natishda xato: {ex.Message}", ex);
        }
    }
}