using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.Common.Interfaces;

public interface IFileService
{
    Task<FileUploadResult> UploadAsync(IFormFile file, CancellationToken ct = default);
    Task<List<FileUploadResult>> UploadMultipleAsync(IFormFile[] files, CancellationToken ct = default);
    Task DeleteAsync(string fileUrlOrPath, CancellationToken ct = default);
    Task DeleteMultipleAsync(IEnumerable<string> fileUrls, CancellationToken ct = default);
    Task<bool> ExistsAsync(string fileUrlOrPath);
}

public record FileUploadResult(
    string FileName,
    string FileUrl,
    string FileType,
    long FileSize
);