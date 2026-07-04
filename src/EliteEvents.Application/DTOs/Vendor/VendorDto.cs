namespace EliteEvents.Application.DTOs.Vendor;

public class VendorDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? CityName { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerEvent { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }
}

public class VendorListDto
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public decimal Rating { get; set; }
    public int TotalReviews { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }
}

public class VendorCreateDto
{
    public string BusinessName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerEvent { get; set; }
    public string? LogoUrl { get; set; }
}

public class VendorUpdateDto : VendorCreateDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}

public class VendorCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
    public int VendorCount { get; set; }
}
