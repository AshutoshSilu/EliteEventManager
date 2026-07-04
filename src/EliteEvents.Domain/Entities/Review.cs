using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a customer review for an event, venue, vendor, or package.
/// </summary>
public class Review : AuditableEntity
{
    public int CustomerId { get; set; }
    public string EntityType { get; set; } = string.Empty; // Event, Venue, Vendor, Package
    public int EntityId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string? ImageUrl { get; set; }
    public string? Reply { get; set; }
    public Guid? RepliedBy { get; set; }
    public DateTime? RepliedAt { get; set; }
    public bool IsApproved { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual User? Replier { get; set; }
}
