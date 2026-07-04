using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a vendor/service provider.
/// </summary>
public class Vendor : AuditableEntity
{
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerEvent { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual VendorCategory Category { get; set; } = null!;
    public virtual City? City { get; set; }
    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}
