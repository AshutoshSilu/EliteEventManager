using EliteEvents.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EliteEvents.Infrastructure.Services;

/// <summary>
/// Email service implementation using MailKit SMTP.
/// </summary>
public class EmailService : IEmailService
{
    static EmailService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

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

    public async Task SendWelcomeLetterAsync(string email, string employeeName, string employeeCode, string position, string? mobileNumber)
    {
        var subject = "Welcome to ATAV Events";
        var body = $@"
            <h2>Welcome to ATAV Events!</h2>
            <p>Dear {employeeName},</p>
            <p>Congratulations and welcome to the ATAV Events team. We are excited to have you with us.</p>
            <p><strong>Employee Code:</strong> {employeeCode}<br/>
               <strong>Position:</strong> {position}<br/>
               <strong>Contact Number:</strong> {(string.IsNullOrWhiteSpace(mobileNumber) ? "N/A" : mobileNumber)}</p>
            <p>Please find your welcome letter attached.</p>
            <p>Regards,<br/>ATAV Events HR Team</p>";

        var attachmentBytes = BuildWelcomeLetterPdf(employeeName, employeeCode, position, mobileNumber);
        await SendEmailAsync(email, subject, body, "ATAV-Welcome-Letter.pdf", "application/pdf", attachmentBytes);
    }

    private async Task SendEmailAsync(
        string to,
        string subject,
        string htmlBody,
        string? attachmentFileName = null,
        string? attachmentContentType = null,
        byte[]? attachmentBytes = null)
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

            if (attachmentBytes is { Length: > 0 } && !string.IsNullOrWhiteSpace(attachmentFileName))
            {
                bodyBuilder.Attachments.Add(
                    attachmentFileName,
                    attachmentBytes,
                    ContentType.Parse(attachmentContentType ?? "application/octet-stream"));
            }

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

    private static byte[] BuildWelcomeLetterPdf(string employeeName, string employeeCode, string position, string? mobileNumber)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(28);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(style => style.FontFamily("Helvetica").FontSize(11).FontColor(Colors.Grey.Darken4));

                page.Header().Height(88).Container().Background(Colors.Teal.Darken2).PaddingHorizontal(22).PaddingVertical(18).Column(column =>
                {
                    column.Item().Text("ATAV Events").FontSize(24).SemiBold().FontColor(Colors.White);
                    column.Item().PaddingTop(6).Text("Welcome Letter").FontSize(12).FontColor(Colors.Teal.Lighten4).Italic();
                    column.Item().PaddingTop(10).Text($"Date: {DateTime.UtcNow:dd MMM yyyy}").FontSize(10).FontColor(Colors.Teal.Lighten4);
                    column.Item().PaddingTop(4).AlignRight().Text("WELCOME").FontSize(28).Bold().FontColor(Colors.White);
                });

                page.Content().PaddingTop(18).Column(column =>
                {
                    column.Spacing(16);

                    column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(18).Column(content =>
                    {
                        content.Spacing(10);
                        content.Item().Text($"Dear {employeeName},").FontSize(14).SemiBold().FontColor(Colors.Teal.Darken3);
                        content.Item().Text("Welcome to the ATAV Events team. Your role, creativity, and energy will help us shape memorable experiences for every client and celebration.")
                            .FontSize(11).LineHeight(1.4f);

                        content.Item().PaddingTop(4).BorderLeft(4).BorderColor(Colors.Pink.Darken1).PaddingLeft(12).Column(info =>
                        {
                            info.Item().Text("Employee Snapshot").FontSize(12).Bold().FontColor(Colors.Pink.Darken1);
                            info.Item().Text($"Employee Code: {employeeCode}").FontSize(11);
                            info.Item().Text($"Position: {position}").FontSize(11);
                            info.Item().Text($"Contact Number: {(string.IsNullOrWhiteSpace(mobileNumber) ? "N/A" : mobileNumber)}").FontSize(11);
                        });
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().PaddingRight(10).Container().Border(1).BorderColor(Colors.Teal.Lighten3).Padding(14).Column(left =>
                        {
                            left.Spacing(6);
                            left.Item().Text("Onboarding Note").FontSize(12).Bold().FontColor(Colors.Teal.Darken2);
                            left.Item().Text("Please review your employee details, keep this letter for your records, and contact HR if any correction is needed.").FontSize(10.5f).LineHeight(1.35f);
                        });

                        row.RelativeItem().PaddingLeft(10).Container().Background(Colors.Pink.Lighten5).Border(1).BorderColor(Colors.Pink.Lighten3).Padding(14).Column(right =>
                        {
                            right.Spacing(6);
                            right.Item().Text("ATAV Values").FontSize(12).Bold().FontColor(Colors.Pink.Darken2);
                            right.Item().Text("Creative thinking, attention to detail, and service excellence.").FontSize(10.5f).LineHeight(1.35f);
                        });
                    });

                    column.Item().PaddingTop(8).AlignRight().Text("Warm regards,\nHR Team\nATAV Events").FontSize(11).SemiBold();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("ATAV Events ").SemiBold().FontColor(Colors.Teal.Darken2);
                    text.Span("| Welcome Letter").FontColor(Colors.Grey.Darken1);
                });
            });
        }).GeneratePdf();
    }
}
