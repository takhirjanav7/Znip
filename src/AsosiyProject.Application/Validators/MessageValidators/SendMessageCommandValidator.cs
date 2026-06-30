//using AsosiyProject.Application.Commands;
//using FluentValidation;

//namespace AsosiyProject.Application.Validators.MessageValidators;

//public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
//{
//    public SendMessageCommandValidator()
//    {
//        RuleFor(x => x.Content)
//            .NotEmpty().When(x => x.Files == null || x.Files.Length == 0)
//            .WithMessage("Xabar yoki fayl bo‘lishi kerak!");

//        RuleFor(x => x.Files)
//            .Must(f => f == null || f.Length <= 5)
//            .WithMessage("Maksimal 5 ta fayl!");

//        RuleForEach(x => x.Files)
//            .Must(f => f.Length <= 50 * 1024 * 1024)
//            .WithMessage("Fayl 50MB dan kichik bo‘lsin!");
//    }
//}