using EliteEvents.Domain.Common;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a payment transaction.
/// </summary>
public class Payment : AuditableEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime? PaymentDate { get; set; }
    public string? GatewayResponse { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateTime? RefundDate { get; set; }
    public string? RefundReason { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
}
