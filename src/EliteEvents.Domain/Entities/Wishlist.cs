using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a customer's wishlist item.
/// </summary>
public class Wishlist : BaseEntity
{
    public int CustomerId { get; set; }
    public string EntityType { get; set; } = string.Empty; // Event, Venue, Package
    public int EntityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
}
