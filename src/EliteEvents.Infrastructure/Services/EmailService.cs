using EliteEvents.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EliteEvents.Infrastructure.Services;

/// <summary>
/// Email service implementation using MailKit SMTP.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailVerificationAsync(string email, string token)
    {
        var baseUrl = _configuration["App:FrontendUrl"] ?? "http://localhost:4200";
        var verifyUrl = $"{baseUrl}/auth/verify-email?token={token}";

        var subject = "Verify Your Email - Elite Events";
        var body = $@"
            <h2>Welcome to Elite Events!</h2>
            <p>Please verify your email address by clicking the link below:</p>
            <a href='{verifyUrl}' style='padding: 12px 24px; background: #6366f1; color: white; text-decoration: none; border-radius: 6px;'>
                Verify Email
            </a>
            <p>If you didn't create an account, please ignore this email.</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string token)
    {
        var baseUrl = _configuration["App:FrontendUrl"] ?? "http://localhost:4200";
        var resetUrl = $"{baseUrl}/auth/reset-password?token={Uri.EscapeDataString(token)}";

        var subject = "Reset Password - Elite Events";
        var body = $@"
            <h2>Password Reset Request</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{resetUrl}' style='padding: 12px 24px; background: #6366f1; color: white; text-decoration: none; border-radius: 6px;'>
                Reset Password
            </a>
            <p>This link expires in 1 hour. If you didn't request this, please ignore this email.</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendBookingConfirmationAsync(string email, string bookingNumber, decimal amount)
    {
        var subject = $"Booking Confirmed - {bookingNumber} | Elite Events";
        var body = $@"
            <h2>Booking Confirmed!</h2>
            <p>Your booking <strong>{bookingNumber}</strong> has been confirmed.</p>
            <p>Total Amount: <strong>₹{amount:N2}</strong></p>
            <p>Thank you for choosing Elite Events!</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPaymentConfirmationAsync(string email, string paymentNumber, decimal amount)
    {
        var subject = $"Payment Received - {paymentNumber}";
        var body = $@"
            <h2>Payment Successful!</h2>
            <p>Payment <strong>{paymentNumber}</strong> of <strong>₹{amount:N2}</strong> has been received.</p>
            <p>Thank you!</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendNotificationEmailAsync(string email, string subject, string body)
    {
        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var username = _configuration["Email:Username"] ?? "";
            var password = _configuration["Email:Password"] ?? "";
            var fromAddress = _configuration["Email:FromAddress"] ?? "noreply@eliteevents.com";
            var fromName = _configuration["Email:FromName"] ?? "Elite Events";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("SMTP credentials not configured. Email to {Email} with subject '{Subject}' was not sent. Configure Email:Username and Email:Password in appsettings.json", to, subject);
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Email}: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}: {Subject}", to, subject);
            // Don't throw - email failure shouldn't break the flow
        }
    }
}
