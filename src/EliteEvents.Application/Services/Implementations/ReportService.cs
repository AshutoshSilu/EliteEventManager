using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Report;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReportService> _logger;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReportService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<DashboardKpiDto>> GetDashboardKpisAsync()
    {
        var totalUsers = await _unitOfWork.Users.CountAsync();
        var totalBookings = await _unitOfWork.Bookings.CountAsync();
        var totalRevenue = await _unitOfWork.Payments.GetTotalRevenueAsync();

        var upcomingEvents = await _unitOfWork.Events.CountAsync(e => e.StartDate >= DateTime.UtcNow && !e.IsDeleted);
        var activeVendors = await _unitOfWork.Vendors.CountAsync(v => v.IsActive);

        var kpi = new DashboardKpiDto
        {
            TotalUsers = totalUsers,
            TotalBookings = totalBookings,
            TotalRevenue = totalRevenue,
            UpcomingEvents = upcomingEvents,
            ActiveVendors = activeVendors
        };

        return ApiResponse<DashboardKpiDto>.SuccessResponse(kpi);
    }

    public async Task<ApiResponse<IReadOnlyList<RevenueReportDto>>> GetRevenueReportAsync(ReportFilterDto filter)
    {
        var report = new List<RevenueReportDto>();
        return ApiResponse<IReadOnlyList<RevenueReportDto>>.SuccessResponse(report);
    }

    public async Task<ApiResponse<IReadOnlyList<BookingReportDto>>> GetBookingReportAsync(ReportFilterDto filter)
    {
        var report = new List<BookingReportDto>();
        return ApiResponse<IReadOnlyList<BookingReportDto>>.SuccessResponse(report);
    }

    public async Task<ApiResponse<IReadOnlyList<MonthlySalesDto>>> GetMonthlySalesAsync(int year)
    {
        var months = Enumerable.Range(1, 12).Select(m => new MonthlySalesDto
        {
            MonthNum = m,
            MonthName = new DateTime(year, m, 1).ToString("MMMM"),
            BookingCount = 0,
            Revenue = 0
        }).ToList();

        return ApiResponse<IReadOnlyList<MonthlySalesDto>>.SuccessResponse(months);
    }
}
