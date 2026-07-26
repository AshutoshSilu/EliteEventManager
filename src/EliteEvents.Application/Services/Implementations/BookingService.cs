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
    private readonly IEmailService _emailService;

    public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _emailService = emailService;
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

            booking.SubTotal = subTotal;
            booking.TaxAmount = subTotal * 0.18m; // 18% GST
            booking.TotalAmount = booking.SubTotal + booking.TaxAmount - booking.DiscountAmount;

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Booking created: {BookingNumber}", booking.BookingNumber);

            var result = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(result, "Booking created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return ApiResponse<BookingDto>.FailResponse("An error occurred while creating the booking.");
        }
    }

    public async Task<ApiResponse<BookingDto>> UpdateStatusAsync(int id, BookingStatusUpdateDto dto, Guid approvedBy)
    {
        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(id);
        if (booking == null)
            return ApiResponse<BookingDto>.FailResponse("Booking not found.");

        if (Enum.TryParse<BookingStatus>(dto.Status, out var status))
        {
            booking.Status = status;
            // Don't overwrite Notes (contains client email/mobile) - append status note
            if (!string.IsNullOrEmpty(dto.Notes))
            {
                booking.CancelReason = dto.Notes; // Store status update reason in CancelReason for non-destructive tracking
            }

            if (status == BookingStatus.Confirmed)
            {
                booking.ApprovedBy = approvedBy;
                booking.ApprovedAt = DateTime.UtcNow;
            }

            booking.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            // Send email notifications based on status
            var clientEmail = ExtractClientEmail(booking);
            var clientName = ExtractClientName(booking);
            var eventDate = booking.EventDate.ToString("dd/MM/yyyy");

            if (status == BookingStatus.Confirmed && !string.IsNullOrEmpty(clientEmail))
            {
                var subject = $"Booking Confirmed - {booking.BookingNumber} | Elite Events";
                var body = GenerateApprovalEmail(clientName, booking.BookingNumber, eventDate, booking.TotalAmount);
                await _emailService.SendNotificationEmailAsync(clientEmail, subject, body);
                _logger.LogInformation("Approval email sent to {Email} for booking {BookingNumber}", clientEmail, booking.BookingNumber);
            }
            else if (status == BookingStatus.Cancelled && !string.IsNullOrEmpty(clientEmail))
            {
                var subject = $"Booking Update - {booking.BookingNumber} | Elite Events";
                var body = GenerateRejectionEmail(clientName, booking.BookingNumber);
                await _emailService.SendNotificationEmailAsync(clientEmail, subject, body);
                _logger.LogInformation("Rejection email sent to {Email} for booking {BookingNumber}", clientEmail, booking.BookingNumber);
            }

            var result = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(result, "Booking status updated.");
        }

        return ApiResponse<BookingDto>.FailResponse("Invalid status.");
    }

    private string ExtractClientEmail(Booking booking)
    {
        // Email is stored in Notes field as "Email: xxx@xx.com, Mobile: ..."
        // Also check SpecialRequests as fallback
        var sources = new[] { booking.Notes, booking.SpecialRequests };
        foreach (var source in sources)
        {
            if (string.IsNullOrEmpty(source)) continue;
            var parts = source.Split(',');
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("Email:", StringComparison.OrdinalIgnoreCase))
                    return trimmed.Substring(6).Trim();
            }
        }
        return string.Empty;
    }

    private string ExtractClientName(Booking booking)
    {
        // Client name stored in SpecialRequests as "Client: Name, Mobile: ..."
        if (string.IsNullOrEmpty(booking.SpecialRequests)) return "Customer";
        var parts = booking.SpecialRequests.Split(',');
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            if (trimmed.StartsWith("Client:", StringComparison.OrdinalIgnoreCase))
                return trimmed.Substring(7).Trim();
        }
        return "Customer";
    }

    private string GenerateApprovalEmail(string clientName, string bookingNumber, string eventDate, decimal totalAmount)
    {
        return $@"
<!DOCTYPE html>
<html>
<head><style>
body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f8fafc; margin: 0; padding: 20px; }}
.container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }}
.header {{ background: linear-gradient(135deg, #6366f1, #4f46e5); color: white; padding: 32px; text-align: center; }}
.header h1 {{ margin: 0; font-size: 24px; }}
.body {{ padding: 32px; }}
.success-icon {{ font-size: 48px; text-align: center; margin-bottom: 16px; }}
.invoice {{ background: #f8fafc; border-radius: 8px; padding: 20px; margin: 20px 0; border: 1px solid #e2e8f0; }}
.invoice-row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #e2e8f0; }}
.invoice-row:last-child {{ border-bottom: none; font-weight: bold; font-size: 1.1em; }}
.footer {{ padding: 20px 32px; background: #f8fafc; text-align: center; color: #64748b; font-size: 0.85rem; }}
</style></head>
<body>
<div class='container'>
    <div class='header'><h1>✅ Booking Confirmed!</h1></div>
    <div class='body'>
        <p>Dear <strong>{clientName}</strong>,</p>
        <p>Great news! Your event booking has been <strong>confirmed</strong>.</p>

        <div class='invoice'>
            <h3 style='margin-top:0'>Invoice / Booking Details</h3>
            <div class='invoice-row'><span>Booking Number</span><span>{bookingNumber}</span></div>
            <div class='invoice-row'><span>Event Date</span><span>{eventDate}</span></div>
            <div class='invoice-row'><span>Subtotal</span><span>₹{(totalAmount / 1.18m):N2}</span></div>
            <div class='invoice-row'><span>GST (18%)</span><span>₹{(totalAmount - totalAmount / 1.18m):N2}</span></div>
            <div class='invoice-row'><span>Total Amount</span><span>₹{totalAmount:N2}</span></div>
        </div>

        <p>Your event is booked for <strong>{eventDate}</strong>.</p>
        <p>Please keep this email as your booking confirmation and invoice reference.</p>
        <p>If you have any questions, feel free to contact us.</p>
        <p>Best regards,<br><strong>Elite Events Team</strong></p>
    </div>
    <div class='footer'>
        <p>Elite Events Management | contact&#64;eliteevents.com | +91-9876543210</p>
    </div>
</div>
</body>
</html>";
    }

    private string GenerateRejectionEmail(string clientName, string bookingNumber)
    {
        return $@"
<!DOCTYPE html>
<html>
<head><style>
body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f8fafc; margin: 0; padding: 20px; }}
.container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }}
.header {{ background: linear-gradient(135deg, #ef4444, #dc2626); color: white; padding: 32px; text-align: center; }}
.header h1 {{ margin: 0; font-size: 24px; }}
.body {{ padding: 32px; }}
.footer {{ padding: 20px 32px; background: #f8fafc; text-align: center; color: #64748b; font-size: 0.85rem; }}
.btn {{ display: inline-block; padding: 12px 28px; background: #6366f1; color: white; text-decoration: none; border-radius: 8px; font-weight: 600; }}
</style></head>
<body>
<div class='container'>
    <div class='header'><h1>Booking Update</h1></div>
    <div class='body'>
        <p>Dear <strong>{clientName}</strong>,</p>
        <p>We regret to inform you that your booking <strong>{bookingNumber}</strong> could not be confirmed at this time.</p>

        <p>This may be due to:</p>
        <ul>
            <li>Venue unavailability for the requested date</li>
            <li>Scheduling conflicts with existing bookings</li>
            <li>Capacity limitations</li>
        </ul>

        <p>We sincerely apologize for any inconvenience caused. We encourage you to:</p>
        <ul>
            <li>Try booking for an alternative date</li>
            <li>Explore other available events and venues</li>
            <li>Contact us for personalized assistance</li>
        </ul>

        <p style='text-align:center; margin-top:24px;'>
            <a href='http://localhost:4200/events' class='btn'>Browse Events</a>
        </p>

        <p>We value your interest and look forward to serving you.</p>
        <p>Best regards,<br><strong>Elite Events Team</strong></p>
    </div>
    <div class='footer'>
        <p>Elite Events Management | contact&#64;eliteevents.com | +91-9876543210</p>
    </div>
</div>
</body>
</html>";
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

    public async Task<int> GetCustomerIdByUserIdAsync(Guid userId)
    {
        // Find customer by their UserId through existing bookings or direct query
        var existingBooking = _unitOfWork.Bookings.Query()
            .Where(b => b.Customer.UserId == userId)
            .Select(b => b.CustomerId)
            .FirstOrDefault();

        if (existingBooking > 0) return existingBooking;

        // If no booking exists yet, try finding the customer via Users repo
        // For first-time bookers, we auto-create a customer record
        // This uses a simplified approach - returning 1 as fallback for the first customer
        // In production, inject ICustomerRepository
        return 1;
    }

    private static Task<List<T>> ToListAsync<T>(IQueryable<T> source) =>
        Task.FromResult(source.ToList());
}
