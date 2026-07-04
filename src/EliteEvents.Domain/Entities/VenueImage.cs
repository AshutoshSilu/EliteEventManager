using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an image associated with a venue.
/// </summary>
public class VenueImage : BaseEntity
{
    public int VenueId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Venue Venue { get; set; } = null!;
}
