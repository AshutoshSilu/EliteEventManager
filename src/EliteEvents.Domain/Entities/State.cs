using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a state/province lookup.
/// </summary>
public class State : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int CountryId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Country Country { get; set; } = null!;
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
