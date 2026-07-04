using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a service included in a package.
/// </summary>
public class PackageService : BaseEntity
{
    public int PackageId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsIncluded { get; set; } = true;
    public int SortOrder { get; set; }

    // Navigation properties
    public virtual Package Package { get; set; } = null!;
}
