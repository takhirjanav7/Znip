    using AsosiyProject.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Infrastructure.Services;

public class LocalFileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _uploadPath;

    public LocalFileService(IWebHostEnvironment env)
    {
        _env = env;

        // Agar WebRootPath (wwwroot) null bo'lsa (ba'zan testlarda shunday bo'ladi), 
        // loyiha papkasini olish uchun ContentRootPath ishlatiladi.
        var rootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        _uploadPath = Path.Combine(rootPath, "uploads");

        // Papkani bir marta konstruktorda yaratib qo'yamiz
        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);
    }

    public async Task<FileUploadResult> UploadAsync(IFormFile file, CancellationToken ct = default)
    {
        string extension = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid()}{extension}";
        
        string fullPath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        string fileUrl = "/uploads/" + fileName;

        return new FileUploadResult(
            FileName: file.FileName,
            FileUrl: fileUrl,
            FileType: file.ContentType, // Masalan: "image/png"
            FileSize: file.Length
        );
    }

    public async Task<List<FileUploadResult>> UploadMultipleAsync(IFormFile[] files, CancellationToken ct = default)
    {
        var results = new List<FileUploadResult>();
        foreach (var file in files)
        {
            results.Add(await UploadAsync(file, ct));
        }
        return results;
    }

    public async Task DeleteAsync(string fileUrlOrPath, CancellationToken ct = default)
    {
        try
        {
            var fileName = Path.GetFileName(fileUrlOrPath);
            var filePath = Path.Combine(_uploadPath, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        catch { /* ignore */ }
        await Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string fileUrlOrPath)
    {
        var fileName = Path.GetFileName(fileUrlOrPath);
        var filePath = Path.Combine(_uploadPath, fileName);
        return Task.FromResult(File.Exists(filePath));
    }

    public async Task DeleteMultipleAsync(IEnumerable<string> fileUrls, CancellationToken ct = default)
    {
        foreach (var url in fileUrls)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                await DeleteAsync(url, ct);
            }
        }
    }
}