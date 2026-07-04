using EliteEvents.Domain.Common;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a customer booking with status and financial details.
/// </summary>
public class Booking : AuditableEntity
{
    public string BookingNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? EventId { get; set; }
    public int? VenueId { get; set; }
    public int? PackageId { get; set; }
    public DateOnly EventDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int GuestCount { get; set; } = 1;
    public string? SpecialRequests { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public int? CouponId { get; set; }
    public string? Notes { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancelReason { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual Event? Event { get; set; }
    public virtual Venue? Venue { get; set; }
    public virtual Package? Package { get; set; }
    public virtual Coupon? Coupon { get; set; }
    public virtual User? Approver { get; set; }
    public virtual ICollection<BookingDetail> Details { get; set; } = new List<BookingDetail>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual Invoice? Invoice { get; set; }
}
