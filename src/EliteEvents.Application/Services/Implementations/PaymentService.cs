using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Payment;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PaymentDto>> GetByIdAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        if (payment == null)
            return ApiResponse<PaymentDto>.FailResponse("Payment not found.");

        var dto = _mapper.Map<PaymentDto>(payment);
        return ApiResponse<PaymentDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<PaymentDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Payments.Query();

        query = parameters.SortBy?.ToLower() switch
        {
            "amount" => parameters.SortDirection == "desc" ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount),
            "date" => parameters.SortDirection == "desc" ? query.OrderByDescending(p => p.PaymentDate) : query.OrderBy(p => p.PaymentDate),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<PaymentDto>>(items);
        var result = new PagedResult<PaymentDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<PaymentDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<PaymentDto>> ProcessPaymentAsync(PaymentCreateDto dto, int customerId)
    {
        var entity = _mapper.Map<Payment>(dto);
        entity.CustomerId = customerId;
        entity.Status = PaymentStatus.Completed;
        entity.PaymentDate = DateTime.UtcNow;
        entity.PaymentNumber = await _unitOfWork.Payments.GeneratePaymentNumberAsync();

        await _unitOfWork.Payments.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Payment processed: {PaymentNumber}", entity.PaymentNumber);

        var result = _mapper.Map<PaymentDto>(entity);
        return ApiResponse<PaymentDto>.SuccessResponse(result, "Payment processed successfully.");
    }

    public async Task<ApiResponse<PaymentDto>> RefundPaymentAsync(PaymentRefundDto dto)
    {
        var entity = await _unitOfWork.Payments.GetByIdAsync(dto.PaymentId);
        if (entity == null)
            return ApiResponse<PaymentDto>.FailResponse("Payment not found.");

        entity.RefundAmount = dto.RefundAmount;
        entity.RefundReason = dto.RefundReason;
        entity.RefundDate = DateTime.UtcNow;
        entity.Status = PaymentStatus.Refunded;

        _unitOfWork.Payments.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<PaymentDto>(entity);
        return ApiResponse<PaymentDto>.SuccessResponse(result, "Payment refunded successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<PaymentDto>>> GetByBookingAsync(int bookingId)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByBookingAsync(bookingId);
        var dtos = _mapper.Map<IReadOnlyList<PaymentDto>>(payments);
        return ApiResponse<IReadOnlyList<PaymentDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<PaymentDto>>> GetByCustomerAsync(int customerId)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByCustomerAsync(customerId);
        var dtos = _mapper.Map<IReadOnlyList<PaymentDto>>(payments);
        return ApiResponse<IReadOnlyList<PaymentDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<InvoiceDto>> GetInvoiceAsync(int bookingId)
    {
        // Simplified - return a generated invoice based on booking
        var payments = await _unitOfWork.Payments.GetPaymentsByBookingAsync(bookingId);
        var totalPaid = payments.Sum(p => p.Amount);

        var invoice = new InvoiceDto
        {
            BookingId = bookingId,
            TotalAmount = totalPaid,
            PaidAmount = totalPaid,
            DueAmount = 0,
            Status = "Paid",
            IssuedAt = DateTime.UtcNow
        };

        return ApiResponse<InvoiceDto>.SuccessResponse(invoice);
    }

    public async Task<ApiResponse<InvoiceDto>> GenerateInvoiceAsync(int bookingId)
    {
        return await GetInvoiceAsync(bookingId);
    }
}
