using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a city lookup.
/// </summary>
public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int StateId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual State State { get; set; } = null!;
}
