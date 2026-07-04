using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an event package with pricing and services.
/// </summary>
public class Package : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? Duration { get; set; }
    public int? MaxGuests { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPopular { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    // Navigation properties
    public virtual EventCategory? Category { get; set; }
    public virtual ICollection<PackageService> Services { get; set; } = new List<PackageService>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
