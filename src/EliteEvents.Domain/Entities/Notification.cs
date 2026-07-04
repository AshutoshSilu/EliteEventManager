using EliteEvents.Domain.Common;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a notification sent to a user.
/// </summary>
public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
