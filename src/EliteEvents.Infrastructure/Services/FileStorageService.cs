using EliteEvents.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Infrastructure.Services;

/// <summary>
/// Local file storage service. In production, replace with Azure Blob Storage or AWS S3.
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _uploadPath;

    public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _uploadPath = configuration["FileStorage:UploadPath"] ?? "wwwroot/uploads";
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder)
    {
        try
        {
            var folderPath = Path.Combine(_uploadPath, folder);
            Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            var relativePath = $"/uploads/{folder}/{uniqueFileName}";
            _logger.LogInformation("File uploaded: {FilePath}", relativePath);

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file: {FileName}", fileName);
            throw;
        }
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_uploadPath, filePath.TrimStart('/').Replace("uploads/", ""));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("File deleted: {FilePath}", filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            return Task.FromResult(false);
        }
    }

    public string GetFileUrl(string filePath)
    {
        var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:7001";
        return $"{baseUrl}{filePath}";
    }
}
