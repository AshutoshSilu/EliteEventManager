using EliteEvents.Application.DTOs.Report;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Report and analytics endpoints for the admin dashboard.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ManagerOrAdmin")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Get dashboard KPI data.
    /// </summary>
    [HttpGet("dashboard-kpis")]
    public async Task<IActionResult> GetDashboardKpis()
    {
        var result = await _reportService.GetDashboardKpisAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get revenue report for a date range.
    /// </summary>
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] ReportFilterDto filter)
    {
        var result = await _reportService.GetRevenueReportAsync(filter);
        return Ok(result);
    }

    /// <summary>
    /// Get booking report for a date range.
    /// </summary>
    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookingReport([FromQuery] ReportFilterDto filter)
    {
        var result = await _reportService.GetBookingReportAsync(filter);
        return Ok(result);
    }

    /// <summary>
    /// Get monthly sales chart data for a specific year.
    /// </summary>
    [HttpGet("monthly-sales/{year:int}")]
    public async Task<IActionResult> GetMonthlySales(int year)
    {
        var result = await _reportService.GetMonthlySalesAsync(year);
        return Ok(result);
    }
}
