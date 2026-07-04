using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a customer testimonial displayed on the site.
/// </summary>
public class Testimonial : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public string? Designation { get; set; }
    public string? Company { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public string? PhotoUrl { get; set; }
    public bool IsApproved { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
