using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a gallery item (image or video).
/// </summary>
public class Gallery : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string MediaType { get; set; } = "Image"; // Image, Video
    public string MediaUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AlbumName { get; set; }
    public int? EventId { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Event? Event { get; set; }
}
