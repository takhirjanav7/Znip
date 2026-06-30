//using AsosiyProject.Application.Common.Interfaces;
//using AsosiyProject.Application.Interfaces.UNITWORK;
//using AsosiyProject.Application.Validators.Commands.Posts.CreatePost;
//using AsosiyProject.Domain.Entities;
//using MediatR;

//namespace AsosiyProject.Application.Features.Posts.Commands.CreatePost;

//public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly ICurrentUserService _currentUser;
//    private readonly IFileService _fileService;

//    public CreatePostCommandHandler(
//        IUnitOfWork unitOfWork,
//        ICurrentUserService currentUser,
//        IFileService fileService)
//    {
//        _unitOfWork = unitOfWork;
//        _currentUser = currentUser;
//        _fileService = fileService;
//    }

//    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken ct)
//    {
//        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

//        // Yangi post yaratish
//        var post = new Post
//        {
//            PostId = Guid.NewGuid(),
//            Content = request.Content.Trim(),
//            Type = request.ProjectId.HasValue ? PostType.ProjectShowcase : PostType.Regular,
//            UserId = userId,
//            ProjectId = request.ProjectId,
//            CreatedAt = DateTime.UtcNow,
//            IsArchived = false
//        };

//        // Fayllarni yuklash (agar bor bo‘lsa)
//        if (request.Files != null && request.Files.Any())
//        {
//            var uploadedFiles = await _fileService.UploadMultipleAsync(request.Files, ct);

//            post.Files = uploadedFiles.Select(f => new PostFile
//            {
//                Id = Guid.NewGuid(),
//                FileName = f.FileName,
//                FileUrl = f.FileUrl,
//                ContentType = f.FileType,
//                FileSize = f.FileSize,
//                FileType = DetermineFileType(f.FileType), // Image, Video, Document
//                PostId = post.PostId,
//                Post = post
//            }).ToList();
//        }

//        // Repository orqali qo‘shish
//        await _unitOfWork.Posts.AddAsync(post, ct);

//        // Bitta joydan saqlash
//        await _unitOfWork.SaveChangesAsync(ct);

//        return post.PostId;
//    }

//    // Fayl turini aniqlash (ixtiyoriy, chiroyli bo‘lishi uchun)
//    private static string DetermineFileType(string contentType)
//    {
//        return contentType switch
//        {
//            var x when x.StartsWith("image/") => "Image",
//            var x when x.StartsWith("video/") => "Video",
//            "application/pdf" => "Document",
//            "application/zip" or "application/x-rar-compressed" => "Archive",
//            _ => "File"
//        };
//    }
//}