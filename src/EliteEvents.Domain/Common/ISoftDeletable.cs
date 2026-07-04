namespace EliteEvents.Domain.Common;

/// <summary>
/// Interface for entities that support soft delete.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
