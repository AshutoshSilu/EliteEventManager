namespace EliteEvents.Domain.Entities;

/// <summary>
/// Many-to-many join entity between Role and Permission.
/// </summary>
public class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }

    // Navigation properties
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
