using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a line item or vendor assignment within a booking.
/// </summary>
public class BookingDetail : BaseEntity
{
    public int BookingId { get; set; }
    public int? VendorId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual Vendor? Vendor { get; set; }
}
