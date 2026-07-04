using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Booking;

namespace EliteEvents.Application.Services.Interfaces;

public interface IBookingService
{
    Task<ApiResponse<BookingDto>> GetByIdAsync(int id);
    Task<ApiResponse<BookingDto>> GetByBookingNumberAsync(string bookingNumber);
    Task<ApiResponse<PagedResult<BookingListDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<BookingDto>> CreateAsync(BookingCreateDto dto, int customerId);
    Task<ApiResponse<BookingDto>> UpdateStatusAsync(int id, BookingStatusUpdateDto dto, Guid approvedBy);
    Task<ApiResponse> CancelBookingAsync(int id, string reason, Guid userId);
    Task<ApiResponse<IReadOnlyList<BookingListDto>>> GetCustomerBookingsAsync(int customerId);
    Task<ApiResponse<IReadOnlyList<BookingListDto>>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    Task<ApiResponse<int>> GetTodaysBookingCountAsync();
}
