using EliteEvents.Domain.Common;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an invoice generated for a booking.
/// </summary>
public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public int CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public DateOnly? DueDate { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
    public string? Notes { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
}
