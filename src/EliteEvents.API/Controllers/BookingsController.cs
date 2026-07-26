using System.Security.Claims;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Booking;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Booking management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Get all bookings with pagination. Admin/Manager only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _bookingService.GetAllAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Get booking by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get booking by booking number.
    /// </summary>
    [HttpGet("number/{bookingNumber}")]
    public async Task<IActionResult> GetByNumber(string bookingNumber)
    {
        var result = await _bookingService.GetByBookingNumberAsync(bookingNumber);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new booking. Customer only.
    /// </summary>
    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var customerId = await _bookingService.GetCustomerIdByUserIdAsync(userId);
        if (customerId == 0)
            return BadRequest(new { success = false, message = "Customer profile not found." });

        var result = await _bookingService.CreateAsync(dto, customerId);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Update booking status (approve, confirm, etc.). Manager/Admin only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusUpdateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _bookingService.UpdateStatusAsync(id, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Approve or Deny a booking (Customer accessible).
    /// </summary>
    [HttpPatch("{id:int}/customer-action")]
    public async Task<IActionResult> CustomerAction(int id, [FromBody] BookingStatusUpdateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _bookingService.UpdateStatusAsync(id, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Cancel a booking.
    /// </summary>
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] string reason)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _bookingService.CancelBookingAsync(id, reason, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get current customer's bookings.
    /// </summary>
    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var customerId = await _bookingService.GetCustomerIdByUserIdAsync(userId);
        var result = await _bookingService.GetCustomerBookingsAsync(customerId);
        return Ok(result);
    }

    /// <summary>
    /// Get today's booking count. Admin dashboard KPI.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("today-count")]
    public async Task<IActionResult> GetTodaysCount()
    {
        var result = await _bookingService.GetTodaysBookingCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get bookings by date range.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("by-date")]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var result = await _bookingService.GetBookingsByDateRangeAsync(startDate, endDate);
        return Ok(result);
    }
}
