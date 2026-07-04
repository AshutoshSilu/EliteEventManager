using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Booking;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

/// <summary>
/// Handles booking operations including creation, status updates, and cancellation.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<BookingDto>> GetByIdAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(id);
        if (booking == null)
            return ApiResponse<BookingDto>.FailResponse("Booking not found.");

        var dto = _mapper.Map<BookingDto>(booking);
        return ApiResponse<BookingDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<BookingDto>> GetByBookingNumberAsync(string bookingNumber)
    {
        var booking = await _unitOfWork.Bookings.GetByBookingNumberAsync(bookingNumber);
        if (booking == null)
            return ApiResponse<BookingDto>.FailResponse("Booking not found.");

        var dto = _mapper.Map<BookingDto>(booking);
        return ApiResponse<BookingDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<BookingListDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Bookings.Query();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            query = query.Where(b => b.BookingNumber.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(parameters.FilterBy) && !string.IsNullOrWhiteSpace(parameters.FilterValue))
        {
            if (parameters.FilterBy.ToLower() == "status")
            {
                query = query.Where(b => b.Status.ToString() == parameters.FilterValue);
            }
        }

        query = parameters.SortBy?.ToLower() switch
        {
            "date" => parameters.SortDirection == "desc" ? query.OrderByDescending(b => b.EventDate) : query.OrderBy(b => b.EventDate),
            "amount" => parameters.SortDirection == "desc" ? query.OrderByDescending(b => b.TotalAmount) : query.OrderBy(b => b.TotalAmount),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<BookingListDto>>(items);
        var result = new PagedResult<BookingListDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<BookingListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<BookingDto>> CreateAsync(BookingCreateDto dto, int customerId)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var bookingNumber = await _unitOfWork.Bookings.GenerateBookingNumberAsync();

            var booking = new Booking
            {
                BookingNumber = bookingNumber,
                CustomerId = customerId,
                EventId = dto.EventId,
                VenueId = dto.VenueId,
                PackageId = dto.PackageId,
                EventDate = dto.EventDate,
                GuestCount = dto.GuestCount,
                SpecialRequests = dto.SpecialRequests,
                Notes = dto.Notes,
                Status = BookingStatus.Pending
            };

            // Calculate totals
            decimal subTotal = 0;
            foreach (var detail in dto.Details)
            {
                var bookingDetail = new BookingDetail
                {
                    VendorId = detail.VendorId,
                    ServiceName = detail.ServiceName,
                    Description = detail.Description,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice,
                    TotalPrice = detail.Quantity * detail.UnitPrice
                };
                booking.Details.Add(bookingDetail);
                subTotal += bookingDetail.TotalPrice;
            }

            // If package selected, add package price
            if (dto.PackageId.HasValue)
            {
                // Fetch package price - simplified
                var packages = await _unitOfWork.Bookings.Query()
                    .Where(b => b.PackageId == dto.PackageId).ToListAsync();
            }

            booking.SubTotal = subTotal;
            booking.TaxAmount = subTotal * 0.18m; // 18% GST
            booking.TotalAmount = booking.SubTotal + booking.TaxAmount - booking.DiscountAmount;

            // Apply coupon if provided
            if (!string.IsNullOrWhiteSpace(dto.CouponCode))
            {
                // Coupon validation logic would go here
            }

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Booking created: {BookingNumber}", booking.BookingNumber);

            var result = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(result, "Booking created successfully.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error creating booking");
            return ApiResponse<BookingDto>.FailResponse("An error occurred while creating the booking.");
        }
    }

    public async Task<ApiResponse<BookingDto>> UpdateStatusAsync(int id, BookingStatusUpdateDto dto, Guid approvedBy)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking == null)
            return ApiResponse<BookingDto>.FailResponse("Booking not found.");

        if (Enum.TryParse<BookingStatus>(dto.Status, out var status))
        {
            booking.Status = status;
            booking.Notes = dto.Notes;

            if (status == BookingStatus.Confirmed)
            {
                booking.ApprovedBy = approvedBy;
                booking.ApprovedAt = DateTime.UtcNow;
            }

            booking.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(result, "Booking status updated.");
        }

        return ApiResponse<BookingDto>.FailResponse("Invalid status.");
    }

    public async Task<ApiResponse> CancelBookingAsync(int id, string reason, Guid userId)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking == null)
            return ApiResponse.FailResponse("Booking not found.");

        if (booking.Status == BookingStatus.Cancelled)
            return ApiResponse.FailResponse("Booking is already cancelled.");

        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancelReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Booking cancelled successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<BookingListDto>>> GetCustomerBookingsAsync(int customerId)
    {
        var bookings = await _unitOfWork.Bookings.GetCustomerBookingsAsync(customerId);
        var dtos = _mapper.Map<IReadOnlyList<BookingListDto>>(bookings);
        return ApiResponse<IReadOnlyList<BookingListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<BookingListDto>>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByDateRangeAsync(startDate, endDate);
        var dtos = _mapper.Map<IReadOnlyList<BookingListDto>>(bookings);
        return ApiResponse<IReadOnlyList<BookingListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<int>> GetTodaysBookingCountAsync()
    {
        var count = await _unitOfWork.Bookings.GetTodaysBookingCountAsync();
        return ApiResponse<int>.SuccessResponse(count);
    }

    private static Task<List<T>> ToListAsync<T>(IQueryable<T> source) =>
        Task.FromResult(source.ToList());
}
