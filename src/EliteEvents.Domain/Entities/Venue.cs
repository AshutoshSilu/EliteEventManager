using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an event venue with location and capacity details.
/// </summary>
public class Venue : AuditableEntity, ISoftDeletable
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int Capacity { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Facilities { get; set; }
    public string? Rules { get; set; }
    public string? CoverImageUrl { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual City? City { get; set; }
    public virtual ICollection<VenueImage> Images { get; set; } = new List<VenueImage>();
    public virtual ICollection<VenueAvailability> Availability { get; set; } = new List<VenueAvailability>();
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
