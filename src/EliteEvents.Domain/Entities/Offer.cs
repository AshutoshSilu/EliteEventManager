using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a promotional offer.
/// </summary>
public class Offer : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public string? EntityType { get; set; } // Event, Venue, Package
    public int? EntityId { get; set; }
    public string? ImageUrl { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
