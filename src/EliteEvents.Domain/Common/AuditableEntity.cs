namespace EliteEvents.Domain.Common;

/// <summary>
/// Auditable entity with tracking fields for creation and modification.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Auditable entity with GUID primary key.
/// </summary>
public abstract class AuditableEntityGuid : BaseEntityGuid
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
