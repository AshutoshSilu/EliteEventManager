namespace EliteEvents.Application.DTOs.Venue;

public class VenueDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int Capacity { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Facilities { get; set; }
    public string? Rules { get; set; }
    public string? CoverImageUrl { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public List<VenueImageDto> Images { get; set; } = new();
}

public class VenueListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? CityName { get; set; }
    public int Capacity { get; set; }
    public decimal? PricePerDay { get; set; }
    public string? CoverImageUrl { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public bool IsFeatured { get; set; }
}

public class VenueCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int Capacity { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Facilities { get; set; }
    public string? Rules { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
}

public class VenueUpdateDto : VenueCreateDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}

public class VenueImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class VenueAvailabilityDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public string? Notes { get; set; }
}
