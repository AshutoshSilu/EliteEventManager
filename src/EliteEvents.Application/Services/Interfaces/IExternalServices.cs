using EliteEvents.Domain.Entities;

namespace EliteEvents.Application.Services.Interfaces;

/// <summary>
/// JWT token generation and validation service.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateToken(string token);
}

/// <summary>
/// Password hashing and verification service.
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

/// <summary>
/// Email sending service.
/// </summary>
public interface IEmailService
{
    Task SendEmailVerificationAsync(string email, string token);
    Task SendPasswordResetAsync(string email, string token);
    Task SendBookingConfirmationAsync(string email, string bookingNumber, decimal amount);
    Task SendPaymentConfirmationAsync(string email, string paymentNumber, decimal amount);
    Task SendNotificationEmailAsync(string email, string subject, string body);
    Task SendWelcomeLetterAsync(string email, string employeeName, string employeeCode, string position, string? mobileNumber);
}

/// <summary>
/// File storage service for uploads.
/// </summary>
public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder);
    Task<bool> DeleteFileAsync(string filePath);
    string GetFileUrl(string filePath);
}
