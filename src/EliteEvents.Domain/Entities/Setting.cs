using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a system configuration setting.
/// </summary>
public class Setting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public string DataType { get; set; } = "String";
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}
