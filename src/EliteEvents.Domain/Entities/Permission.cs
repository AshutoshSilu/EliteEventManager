using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a system permission.
/// </summary>
public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
