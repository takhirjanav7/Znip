//using AsosiyProject.Application.Validators.Commands.Comments.CreateComment;
//using FluentValidation;

//namespace AsosiyProject.Application.Validators.CommentValidators;

//public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
//{
//    public CreateCommentCommandValidator()
//    {
//        RuleFor(x => x.Text)
//            .NotEmpty().WithMessage("Izoh bo‘sh bo‘lmasligi kerak!")
//            .MinimumLength(1)
//            .MaximumLength(1000).WithMessage("Izoh 1000 belgidan oshmasin!");

//        RuleFor(x => x.PostId)
//            .NotEmpty().WithMessage("Post ID bo‘sh bo‘lmasligi kerak!")
//            .NotEqual(Guid.Empty).WithMessage("Noto‘g‘ri post ID!");

//        RuleFor(x => x.ParentCommentId)
//            .NotEqual(Guid.Empty).When(x => x.ParentCommentId.HasValue)
//            .WithMessage("Noto‘g‘ri reply ID!");
//    }
//}