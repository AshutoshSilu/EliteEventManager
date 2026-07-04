using EliteEvents.Domain.Common;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an event with scheduling and pricing details.
/// </summary>
public class Event : AuditableEntity, ISoftDeletable
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int? VenueId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int? MaxAttendees { get; set; }
    public int CurrentAttendees { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public string? CoverImageUrl { get; set; }
    public string? Tags { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public Guid? OrganizerId { get; set; }
    public bool IsDeleted { get; set; }

    // Computed
    public int? AvailableSeats => MaxAttendees.HasValue ? MaxAttendees.Value - CurrentAttendees : null;

    // Navigation properties
    public virtual EventCategory Category { get; set; } = null!;
    public virtual Venue? Venue { get; set; }
    public virtual User? Organizer { get; set; }
    public virtual ICollection<EventImage> Images { get; set; } = new List<EventImage>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Gallery> GalleryItems { get; set; } = new List<Gallery>();
}
