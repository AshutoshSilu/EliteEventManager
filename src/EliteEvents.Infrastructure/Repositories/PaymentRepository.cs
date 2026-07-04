using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Payment repository with specialized query methods.
/// </summary>
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context) { }

    public async Task<Payment?> GetByPaymentNumberAsync(string paymentNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Customer).ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.PaymentNumber == paymentNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsByBookingAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.BookingId == bookingId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Booking)
            .Include(p => p.Customer).ThenInclude(c => c.User)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.Status == PaymentStatus.Completed);

        if (startDate.HasValue)
            query = query.Where(p => p.PaymentDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(p => p.PaymentDate <= endDate.Value);

        return await query.SumAsync(p => p.Amount, cancellationToken);
    }

    public async Task<string> GeneratePaymentNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var count = await _dbSet.CountAsync(cancellationToken) + 1;
        return $"PAY{today}{count:D4}";
    }
}
