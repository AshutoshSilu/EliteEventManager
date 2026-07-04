using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a customer profile linked to a user account.
/// </summary>
public class Customer : AuditableEntity
{
    public Guid UserId { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public int? StateId { get; set; }
    public int? CountryId { get; set; }
    public string? ZipCode { get; set; }
    public string? CompanyName { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual City? City { get; set; }
    public virtual State? State { get; set; }
    public virtual Country? Country { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
