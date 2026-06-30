using AsosiyProject.Application.Validators.Commands.Profile.UpdateProfile;
using FluentValidation;

namespace AsosiyProject.Application.Validators.ProfileValidators;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(100).When(x => x.FullName != null)
            .WithMessage("Ism va familiya 100 belgidan oshmasin!");

        RuleFor(x => x.Bio)
            .MaximumLength(500).When(x => x.Bio != null)
            .WithMessage("Bio 500 belgidan oshmasin!");

        RuleFor(x => x.Location)
            .MaximumLength(100).When(x => x.Location != null);

        RuleFor(x => x.WebsiteUrl)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
            .WithMessage("Sayt linki to‘g‘ri bo‘lsin!");

        RuleFor(x => x.GitHubUrl)
            .Must(url => string.IsNullOrEmpty(url) || url.Contains("github.com"))
            .When(x => !string.IsNullOrEmpty(x.GitHubUrl))
            .WithMessage("GitHub linki to‘g‘ri bo‘lsin!");

        RuleFor(x => x.LinkedInUrl)
            .Must(url => string.IsNullOrEmpty(url) || url.Contains("linkedin.com"))
            .When(x => !string.IsNullOrEmpty(x.LinkedInUrl))
            .WithMessage("LinkedIn linki to‘g‘ri bo‘lsin!");

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
            .WithMessage("Profil rasmi 5MB dan kichik bo‘lsin!")
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .WithMessage("Faqat rasm yuklash mumkin (jpg, png, webp)!");
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}