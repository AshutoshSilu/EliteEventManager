using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Report;

namespace EliteEvents.Application.Services.Interfaces;

public interface IReportService
{
    Task<ApiResponse<DashboardKpiDto>> GetDashboardKpisAsync();
    Task<ApiResponse<IReadOnlyList<RevenueReportDto>>> GetRevenueReportAsync(ReportFilterDto filter);
    Task<ApiResponse<IReadOnlyList<BookingReportDto>>> GetBookingReportAsync(ReportFilterDto filter);
    Task<ApiResponse<IReadOnlyList<MonthlySalesDto>>> GetMonthlySalesAsync(int year);
}
