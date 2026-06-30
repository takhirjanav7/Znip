using AsosiyProject.Application.Validators.Commands.Posts.CreatePost;
using FluentValidation;

namespace AsosiyProject.Application.Validators.PostValidators;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Post bo‘sh bo‘lmasligi kerak!")
            .MinimumLength(1)
            .MaximumLength(5000).WithMessage("Post juda uzun! (max 5000 belgidan)");

        RuleFor(x => x.Files)
            .Must(files => files == null || files.Length <= 10)
            .WithMessage("Maksimal 10 ta rasm/video yuklash mumkin!");

        RuleForEach(x => x.Files)
            .Must(file => file.Length <= 20 * 1024 * 1024) // 20MB
            .WithMessage("Har bir fayl 20MB dan kichik bo‘lsin!")
            .Must(file => new[] { "image/", "video/" }.Any(file.ContentType.StartsWith))
            .WithMessage("Faqat rasm va video yuklash mumkin!");
    }
}