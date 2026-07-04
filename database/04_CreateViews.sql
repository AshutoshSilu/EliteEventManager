-- ============================================================
-- Elite Event Management System - Views
-- ============================================================

USE [EliteEventDB];
GO

-- ============================================================
-- View: Active Events with Details
-- ============================================================
CREATE OR ALTER VIEW [dbo].[vw_ActiveEvents]
AS
SELECT 
    e.[Id],
    e.[Title],
    e.[Description],
    e.[StartDate],
    e.[EndDate],
    e.[Price],
    e.[DiscountPrice],
    e.[MaxAttendees],
    e.[CurrentAttendees],
    (e.[MaxAttendees] - e.[CurrentAttendees]) AS [AvailableSeats],
    e.[Status],
    e.[CoverImageUrl],
    e.[IsFeatured],
    ec.[Name] AS [CategoryName],
    v.[Name] AS [VenueName],
    v.[Address] AS [VenueAddress],
    v.[Capacity] AS [VenueCapacity]
FROM [dbo].[Events] e
INNER JOIN [dbo].[EventCategories] ec ON e.[CategoryId] = ec.[Id]
LEFT JOIN [dbo].[Venues] v ON e.[VenueId] = v.[Id]
WHERE e.[IsDeleted] = 0 AND e.[IsPublished] = 1;
GO

-- ============================================================
-- View: Booking Summary
-- ============================================================
CREATE OR ALTER VIEW [dbo].[vw_BookingSummary]
AS
SELECT 
    b.[Id],
    b.[BookingNumber],
    b.[EventDate],
    b.[GuestCount],
    b.[TotalAmount],
    b.[Status],
    b.[CreatedAt],
    c.[Id] AS [CustomerId],
    u.[FirstName] + ' ' + u.[LastName] AS [CustomerName],
    u.[Email] AS [CustomerEmail],
    u.[PhoneNumber] AS [CustomerPhone],
    e.[Title] AS [EventTitle],
    v.[Name] AS [VenueName],
    p.[Name] AS [PackageName]
FROM [dbo].[Bookings] b
INNER JOIN [dbo].[Customers] c ON b.[CustomerId] = c.[Id]
INNER JOIN [dbo].[Users] u ON c.[UserId] = u.[Id]
LEFT JOIN [dbo].[Events] e ON b.[EventId] = e.[Id]
LEFT JOIN [dbo].[Venues] v ON b.[VenueId] = v.[Id]
LEFT JOIN [dbo].[Packages] p ON b.[PackageId] = p.[Id];
GO

-- ============================================================
-- View: Revenue Summary
-- ============================================================
CREATE OR ALTER VIEW [dbo].[vw_RevenueSummary]
AS
SELECT 
    YEAR(pay.[PaymentDate]) AS [Year],
    MONTH(pay.[PaymentDate]) AS [Month],
    COUNT(*) AS [TotalTransactions],
    SUM(pay.[Amount]) AS [TotalRevenue],
    SUM(ISNULL(pay.[RefundAmount], 0)) AS [TotalRefunds],
    SUM(pay.[Amount]) - SUM(ISNULL(pay.[RefundAmount], 0)) AS [NetRevenue],
    pay.[PaymentMethod]
FROM [dbo].[Payments] pay
WHERE pay.[Status] = 'Completed'
GROUP BY YEAR(pay.[PaymentDate]), MONTH(pay.[PaymentDate]), pay.[PaymentMethod];
GO

-- ============================================================
-- View: Vendor Performance
-- ============================================================
CREATE OR ALTER VIEW [dbo].[vw_VendorPerformance]
AS
SELECT 
    v.[Id],
    v.[BusinessName],
    vc.[Name] AS [Category],
    v.[Rating],
    v.[TotalReviews],
    COUNT(DISTINCT bd.[BookingId]) AS [TotalBookings],
    SUM(bd.[TotalPrice]) AS [TotalRevenue],
    v.[IsVerified],
    v.[IsActive]
FROM [dbo].[Vendors] v
INNER JOIN [dbo].[VendorCategories] vc ON v.[CategoryId] = vc.[Id]
LEFT JOIN [dbo].[BookingDetails] bd ON bd.[VendorId] = v.[Id]
GROUP BY v.[Id], v.[BusinessName], vc.[Name], v.[Rating], 
         v.[TotalReviews], v.[IsVerified], v.[IsActive];
GO

-- ============================================================
-- View: Dashboard KPIs
-- ============================================================
CREATE OR ALTER VIEW [dbo].[vw_DashboardKPIs]
AS
SELECT 
    (SELECT COUNT(*) FROM [dbo].[Users] WHERE [IsDeleted] = 0) AS [TotalUsers],
    (SELECT COUNT(*) FROM [dbo].[Customers]) AS [TotalCustomers],
    (SELECT COUNT(*) FROM [dbo].[Bookings]) AS [TotalBookings],
    (SELECT COUNT(*) FROM [dbo].[Bookings] WHERE CAST([CreatedAt] AS DATE) = CAST(GETUTCDATE() AS DATE)) AS [TodaysBookings],
    (SELECT ISNULL(SUM([Amount]), 0) FROM [dbo].[Payments] WHERE [Status] = 'Completed') AS [TotalRevenue],
    (SELECT ISNULL(SUM([DueAmount]), 0) FROM [dbo].[Invoices] WHERE [Status] IN ('Unpaid','PartiallyPaid','Overdue')) AS [PendingPayments],
    (SELECT COUNT(*) FROM [dbo].[Events] WHERE [StartDate] > GETUTCDATE() AND [IsDeleted] = 0 AND [IsPublished] = 1) AS [UpcomingEvents],
    (SELECT COUNT(*) FROM [dbo].[Vendors] WHERE [IsActive] = 1) AS [ActiveVendors];
GO
