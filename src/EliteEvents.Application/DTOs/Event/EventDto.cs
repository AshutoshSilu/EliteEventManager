namespace EliteEvents.Application.DTOs.Event;

public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int? VenueId { get; set; }
    public string? VenueName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? MaxAttendees { get; set; }
    public int CurrentAttendees { get; set; }
    public int? AvailableSeats { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? Tags { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<EventImageDto> Images { get; set; } = new();
}

public class EventListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? VenueName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int? AvailableSeats { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
}

public class EventCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int? VenueId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int? MaxAttendees { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Tags { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
}

public class EventUpdateDto : EventCreateDto
{
    public int Id { get; set; }
}

public class EventImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}

public class EventCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int EventCount { get; set; }
}
