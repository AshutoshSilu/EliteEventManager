namespace EliteEvents.Domain.Common;

/// <summary>
/// Base entity with common properties for all entities.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}

/// <summary>
/// Base entity with GUID primary key.
/// </summary>
public abstract class BaseEntityGuid
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
