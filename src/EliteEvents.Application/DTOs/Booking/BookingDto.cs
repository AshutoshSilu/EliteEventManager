namespace EliteEvents.Application.DTOs.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public int? EventId { get; set; }
    public string? EventTitle { get; set; }
    public int? VenueId { get; set; }
    public string? VenueName { get; set; }
    public int? PackageId { get; set; }
    public string? PackageName { get; set; }
    public DateOnly EventDate { get; set; }
    public int GuestCount { get; set; }
    public string? SpecialRequests { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BookingDetailDto> Details { get; set; } = new();
}

public class BookingListDto
{
    public int Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? EventTitle { get; set; }
    public string? VenueName { get; set; }
    public DateOnly EventDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class BookingCreateDto
{
    public int? EventId { get; set; }
    public int? VenueId { get; set; }
    public int? PackageId { get; set; }
    public DateOnly EventDate { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int GuestCount { get; set; } = 1;
    public string? SpecialRequests { get; set; }
    public string? CouponCode { get; set; }
    public string? Notes { get; set; }
    public List<BookingDetailCreateDto> Details { get; set; } = new();
}

public class BookingDetailDto
{
    public int Id { get; set; }
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class BookingDetailCreateDto
{
    public int? VendorId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}

public class BookingStatusUpdateDto
{
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? CancelReason { get; set; }
}
