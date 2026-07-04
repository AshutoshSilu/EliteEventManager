using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents venue availability on a specific date.
/// </summary>
public class VenueAvailability : BaseEntity
{
    public int VenueId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Venue Venue { get; set; } = null!;
}
