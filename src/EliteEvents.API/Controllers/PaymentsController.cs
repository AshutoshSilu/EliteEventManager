using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Payment;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Payment processing and invoice management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _paymentService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _paymentService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Process a payment for a booking.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentCreateDto dto)
    {
        var customerId = 1; // Placeholder - resolve from user
        var result = await _paymentService.ProcessPaymentAsync(dto, customerId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Process a refund. Admin/Manager only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost("refund")]
    public async Task<IActionResult> Refund([FromBody] PaymentRefundDto dto)
    {
        var result = await _paymentService.RefundPaymentAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get payments for a specific booking.
    /// </summary>
    [HttpGet("booking/{bookingId:int}")]
    public async Task<IActionResult> GetByBooking(int bookingId)
    {
        var result = await _paymentService.GetByBookingAsync(bookingId);
        return Ok(result);
    }

    /// <summary>
    /// Get current customer's payment history.
    /// </summary>
    [HttpGet("my-payments")]
    public async Task<IActionResult> GetMyPayments()
    {
        var customerId = 1; // Placeholder
        var result = await _paymentService.GetByCustomerAsync(customerId);
        return Ok(result);
    }

    /// <summary>
    /// Get invoice for a booking.
    /// </summary>
    [HttpGet("invoice/{bookingId:int}")]
    public async Task<IActionResult> GetInvoice(int bookingId)
    {
        var result = await _paymentService.GetInvoiceAsync(bookingId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Generate invoice for a booking.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost("invoice/{bookingId:int}")]
    public async Task<IActionResult> GenerateInvoice(int bookingId)
    {
        var result = await _paymentService.GenerateInvoiceAsync(bookingId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
