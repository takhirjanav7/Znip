using AsosiyProject.Application.Validators.Commands.Projects.CreateProject;
using FluentValidation;

namespace AsosiyProject.Application.Validators.ProjectValidators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Loyiha nomi bo‘sh bo‘lmasligi kerak!")
            .Length(3, 200).WithMessage("Nomi 3-200 belgidan iborat bo‘lsin!");

        RuleFor(x => x.Description)
            .MaximumLength(5000).When(x => x.Description != null)
            .WithMessage("Tavsif juda uzun!");

        RuleFor(x => x.RepositoryUrl)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.RepositoryUrl))
            .WithMessage("GitHub/GitLab link to‘g‘ri bo‘lsin!");

        RuleFor(x => x.LiveDemoUrl)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.LiveDemoUrl))
            .WithMessage("Live demo link to‘g‘ri bo‘lsin!");

        RuleFor(x => x.Thumbnail)
            .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
            .WithMessage("Thumbnail 10MB dan kichik bo‘lsin!")
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .WithMessage("Faqat rasm yuklash mumkin!");

        RuleFor(x => x.SkillIds)
            .Must(list => list == null || list.Count <= 20)
            .WithMessage("Maksimal 20 ta skill tanlash mumkin!");
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}