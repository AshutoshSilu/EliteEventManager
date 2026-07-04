using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Booking-specific operations.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetBookingsByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<int> GetTodaysBookingCountAsync(CancellationToken cancellationToken = default);
    Task<string> GenerateBookingNumberAsync(CancellationToken cancellationToken = default);
}
