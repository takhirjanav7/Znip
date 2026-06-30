using AsosiyProject.Application.Commands;
using AsosiyProject.Application.DTOs.PostDTOs;
using AsosiyProject.Application.Interfaces.POST;
using AsosiyProject.Application.Interfaces.Services;
using AsosiyProject.Application.Services.notificationService;
using AsosiyProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;
    private readonly IAppDbContext _context;
    private readonly INotificationService _notifService;

    public PostService(IAppDbContext appDbContext, IPostRepository repository, INotificationService notificationService)
    {
        _notifService = notificationService;
        _context = appDbContext;
        _repository = repository;
    }

    public async Task<Guid> CreatePostAsync(Guid userId, CreatePostDto dto)
    {
        // 1. Fayl nomini generatsiya qilish (bir xil nom bo'lib qolmasligi uchun)
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);

        // 2. Saqlash yo'li (Project papkasidagi wwwroot/uploads)
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // Agar papka bo'lmasa, yaratamiz
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        // 3. Faylni fizik xotiraga saqlash
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        // 4. Bazaga yozish uchun URL tayyorlash
        // (Masalan: /uploads/rasm.jpg)
        var fileUrl = $"/uploads/{fileName}";

        // 5. Post entitysini yaratish
        var post = new Post
        {
            UserId = userId,
            Caption = dto.Caption,
            MediaUrl = fileUrl, // Bazaga faqat fayl yo'li ketadi
            MediaType = dto.MediaType,
            CreatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return post.PostId; // yoki post.Id
    }

    // Like bosish yoki qaytarib olish (Toggle)
    public async Task<bool> ToggleLikeAsync(Guid postId, Guid userId)
    {
        var post = await _repository.GetByIdAsync(postId);
        if (post == null) throw new Exception("Post topilmadi");

        var existingLike = await _repository.GetLikeAsync(postId, userId);

        if (existingLike != null)
        {
            // Like bor ekan, demak olib tashlaymiz (Unlike)
            await _repository.RemoveLikeAsync(existingLike);
            post.LikeCount--; // Countni kamaytiramiz
        }
        else
        {
            // Like yo'q, yangi qo'shamiz
            var newLike = new PostLike { PostId = postId, UserId = userId };
            await _repository.AddLikeAsync(newLike);
            post.LikeCount++; // Countni oshiramiz

            // TODO: Shu yerda NotificationService.Send(...) chaqiriladi
            if (post.UserId != userId)
            {
                await _notifService.SendAsync(
                    userId: post.UserId,        // Kimga (Post egasiga)
                    title: "Yangi Like ❤️",
                    message: "Sizning postingizga like bosildi",
                    type: "Like",               // Frontend uchun tip
                    actorId: userId,            // Kim qildi (Like bosgan odam)
                    postId: postId              // Qaysi postga
                );
            }
        }

        await _repository.UpdateAsync(post);
        await _repository.SaveChangesAsync();
        return existingLike == null; // True qaytsa Like bosildi, False bo'lsa Unlike
    }

    // Ko'rishlar sonini oshirish
    public async Task IncreaseViewCountAsync(Guid postId, Guid userId)
    {
        // Oddiy yondashuv: Har safar ko'rganda yozib qo'yamiz
        // Optimallashtirish uchun: Redis ishlatish yoki user qayta ko'rsa sanamaslik mumkin

        var post = await _repository.GetByIdAsync(postId);
        if (post != null)
        {
            var view = new PostView { Id = Guid.NewGuid(), PostId = postId, UserId = userId };
            await _repository.AddViewAsync(view);

            post.ViewCount++;
            await _repository.UpdateAsync(post);
            await _repository.SaveChangesAsync();
        }
    }

    // Comment yozish
    public async Task AddCommentAsync(Guid postId, Guid userId, string text)
    {
        var post = await _repository.GetByIdAsync(postId);
        if (post == null) throw new Exception("Post topilmadi");

        var comment = new PostComment
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            UserId = userId,
            Text = text
        };

        await _repository.AddCommentAsync(comment);
        post.CommentCount++; // Countni yangilash

        // 🔥 O'ZGARISH: Notification yuborish qismi shu yerda 🔥
        if (post.UserId != userId) // O'ziga o'zi yozsa xabar bormasin
        {
            // Comment matnini qisqartirib ko'rsatamiz (masalan 20 ta harf)
            string shortText = text.Length > 20 ? text.Substring(0, 20) + "..." : text;

            await _notifService.SendAsync(
                userId: post.UserId,
                title: "Yangi Izoh 💬",
                message: $"Sizning postingizga izoh qoldirildi: {shortText}",
                type: "Comment",
                actorId: userId,
                postId: postId
            );
        }

        await _repository.UpdateAsync(post);
        await _repository.SaveChangesAsync();
    }

    public async Task<List<PostViewDto>> GetAllPostsAsync(Guid userId, int page, int pageSize)
    {
        var posts = await _context.Posts
        .AsNoTracking() // Faqat o'qish uchun (tez ishlaydi)
        .OrderByDescending(p => p.CreatedAt) // Eng yangilari tepada
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new PostViewDto
        {
            Id = p.PostId,
            Caption = p.Caption,
            MediaUrl = p.MediaUrl,
            MediaType = p.MediaType.ToString(),
            LikeCount = p.LikeCount,
            CommentCount = p.CommentCount,
            ViewCount = p.ViewCount,
            CreatedAt = p.CreatedAt,

            // Author ma'lumotlari
            AuthorId = p.UserId,
            AuthorName = p.User != null ? p.User.UserName : "Unknown",
            AuthorAvatarUrl = p.User != null ? p.User.ProfilePictureUrl : null,

            // 🔥 MEN LIKE BOSGANMANMI?
            // Likes jadvalidan qidiradi: shu postda mening IDim bormi?
            IsLiked = p.Likes.Any(l => l.UserId == userId)
        })
        .ToListAsync();

        return posts;
    }

    public async Task<PostViewDto> GetPostByIdAsync(Guid id, Guid userId)
    {
        var post = await _context.Posts
        .AsNoTracking()
        .Where(p => p.PostId == id)
        .Select(p => new PostViewDto
        {
            Id = p.PostId,
            Caption = p.Caption,
            MediaUrl = p.MediaUrl,
            MediaType = p.MediaType.ToString(),
            LikeCount = p.LikeCount,
            CommentCount = p.CommentCount,
            ViewCount = p.ViewCount,
            CreatedAt = p.CreatedAt,
            AuthorId = p.UserId,
            AuthorName = p.User != null ? p.User.UserName : "Unknown",
            AuthorAvatarUrl = p.User != null ? p.User.ProfilePictureUrl : null,

            // IsLiked tekshiruvi
            IsLiked = p.Likes.Any(l => l.UserId == userId)
        })
        .FirstOrDefaultAsync();

        if (post == null)
            throw new Exception("Post topilmadi!"); // Yoki null qaytarish mumkin

        return post;
    }
}