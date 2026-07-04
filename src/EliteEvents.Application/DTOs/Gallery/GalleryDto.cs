namespace EliteEvents.Application.DTOs.Gallery;

public class GalleryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string MediaType { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AlbumName { get; set; }
    public int? EventId { get; set; }
    public string? EventTitle { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GalleryCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string MediaType { get; set; } = "Image";
    public string MediaUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AlbumName { get; set; }
    public int? EventId { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }
}

public class GalleryUpdateDto : GalleryCreateDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
