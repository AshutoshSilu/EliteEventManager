using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a country lookup.
/// </summary>
public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? PhoneCode { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<State> States { get; set; } = new List<State>();
}
