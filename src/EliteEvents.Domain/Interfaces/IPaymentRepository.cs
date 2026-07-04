using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Payment-specific operations.
/// </summary>
public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByPaymentNumberAsync(string paymentNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPaymentsByBookingAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPaymentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<string> GeneratePaymentNumberAsync(CancellationToken cancellationToken = default);
}
