namespace EliteEvents.Application.DTOs.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateTime? RefundDate { get; set; }
    public string? RefundReason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaymentCreateDto
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

public class PaymentRefundDto
{
    public int PaymentId { get; set; }
    public decimal RefundAmount { get; set; }
    public string RefundReason { get; set; } = string.Empty;
}

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? PaidAt { get; set; }
}
