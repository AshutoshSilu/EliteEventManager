namespace EliteEvents.Application.DTOs.Report;

public class DashboardKpiDto
{
    public int TotalUsers { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalBookings { get; set; }
    public int TodaysBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PendingPayments { get; set; }
    public int UpcomingEvents { get; set; }
    public int ActiveVendors { get; set; }
}

public class RevenueReportDto
{
    public DateTime Date { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalRefunds { get; set; }
    public decimal NetRevenue { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}

public class MonthlySalesDto
{
    public int MonthNum { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
    public decimal Revenue { get; set; }
}

public class BookingReportDto
{
    public string BookingNumber { get; set; } = string.Empty;
    public DateOnly EventDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? EventTitle { get; set; }
    public string? VenueName { get; set; }
    public string? PackageName { get; set; }
}

public class ReportFilterDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public string? GroupBy { get; set; }
}
