//using MediatR;

//namespace AsosiyProject.Application.Validators.Commands.Comments.CreateComment;

//public record CreateCommentCommand(
//    Guid PostId,
//    string Text,
//    Guid? ParentCommentId = null   // reply uchun (agar commentga javob bo‘lsa)
//) : IRequest<Guid>;                 // qaytaradi → yaratilgan Comment.Id