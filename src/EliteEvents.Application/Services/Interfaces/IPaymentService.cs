using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Payment;

namespace EliteEvents.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<PaymentDto>> GetByIdAsync(int id);
    Task<ApiResponse<PagedResult<PaymentDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<PaymentDto>> ProcessPaymentAsync(PaymentCreateDto dto, int customerId);
    Task<ApiResponse<PaymentDto>> RefundPaymentAsync(PaymentRefundDto dto);
    Task<ApiResponse<IReadOnlyList<PaymentDto>>> GetByBookingAsync(int bookingId);
    Task<ApiResponse<IReadOnlyList<PaymentDto>>> GetByCustomerAsync(int customerId);
    Task<ApiResponse<InvoiceDto>> GetInvoiceAsync(int bookingId);
    Task<ApiResponse<InvoiceDto>> GenerateInvoiceAsync(int bookingId);
}
