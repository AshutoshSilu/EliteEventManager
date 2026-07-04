using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Booking repository with specialized query methods.
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    public async Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Customer).ThenInclude(c => c.User)
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Package)
            .Include(b => b.Details).ThenInclude(d => d.Vendor)
            .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber, cancellationToken);
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Customer).ThenInclude(c => c.User)
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Package)
            .Include(b => b.Details).ThenInclude(d => d.Vendor)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Include(b => b.Package)
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Customer).ThenInclude(c => c.User)
            .Where(b => b.Status == status)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Customer).ThenInclude(c => c.User)
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .Where(b => b.EventDate >= startDate && b.EventDate <= endDate)
            .OrderBy(b => b.EventDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTodaysBookingCountAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return await _dbSet.CountAsync(b => b.EventDate == today, cancellationToken);
    }

    public async Task<string> GenerateBookingNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var count = await _dbSet.CountAsync(cancellationToken) + 1;
        return $"BK{today}{count:D4}";
    }
}
