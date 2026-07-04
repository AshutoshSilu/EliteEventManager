using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a frequently asked question.
/// </summary>
public class FAQ : BaseEntity
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string? Category { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
