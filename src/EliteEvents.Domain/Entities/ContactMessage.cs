using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a message submitted via the contact form.
/// </summary>
public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "New"; // New, Read, Replied, Closed
    public Guid? RepliedBy { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string? Reply { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
