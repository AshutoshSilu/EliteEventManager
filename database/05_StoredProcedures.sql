-- ============================================================
-- Elite Event Management System - Stored Procedures
-- ============================================================

USE [EliteEventDB];
GO

-- ============================================================
-- SP: Generate Booking Number
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateBookingNumber]
    @BookingNumber NVARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Sequence INT;
    SELECT @Sequence = ISNULL(MAX(Id), 0) + 1 FROM [dbo].[Bookings];
    SET @BookingNumber = 'BK' + FORMAT(GETUTCDATE(), 'yyyyMMdd') + RIGHT('0000' + CAST(@Sequence AS NVARCHAR), 4);
END;
GO

-- ============================================================
-- SP: Generate Payment Number
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GeneratePaymentNumber]
    @PaymentNumber NVARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Sequence INT;
    SELECT @Sequence = ISNULL(MAX(Id), 0) + 1 FROM [dbo].[Payments];
    SET @PaymentNumber = 'PAY' + FORMAT(GETUTCDATE(), 'yyyyMMdd') + RIGHT('0000' + CAST(@Sequence AS NVARCHAR), 4);
END;
GO

-- ============================================================
-- SP: Generate Invoice Number
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateInvoiceNumber]
    @InvoiceNumber NVARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Sequence INT;
    SELECT @Sequence = ISNULL(MAX(Id), 0) + 1 FROM [dbo].[Invoices];
    SET @InvoiceNumber = 'INV' + FORMAT(GETUTCDATE(), 'yyyyMMdd') + RIGHT('0000' + CAST(@Sequence AS NVARCHAR), 4);
END;
GO

-- ============================================================
-- SP: Get Revenue Report
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRevenueReport]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(pay.[PaymentDate] AS DATE) AS [Date],
        COUNT(*) AS [TransactionCount],
        SUM(pay.[Amount]) AS [TotalRevenue],
        SUM(ISNULL(pay.[RefundAmount], 0)) AS [TotalRefunds],
        SUM(pay.[Amount]) - SUM(ISNULL(pay.[RefundAmount], 0)) AS [NetRevenue],
        pay.[PaymentMethod]
    FROM [dbo].[Payments] pay
    WHERE pay.[Status] = 'Completed'
        AND CAST(pay.[PaymentDate] AS DATE) BETWEEN @StartDate AND @EndDate
    GROUP BY CAST(pay.[PaymentDate] AS DATE), pay.[PaymentMethod]
    ORDER BY [Date];
END;
GO

-- ============================================================
-- SP: Get Booking Report
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetBookingReport]
    @StartDate DATE,
    @EndDate DATE,
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.[BookingNumber],
        b.[EventDate],
        b.[GuestCount],
        b.[TotalAmount],
        b.[Status],
        b.[CreatedAt],
        u.[FirstName] + ' ' + u.[LastName] AS [CustomerName],
        e.[Title] AS [EventTitle],
        v.[Name] AS [VenueName],
        p.[Name] AS [PackageName]
    FROM [dbo].[Bookings] b
    INNER JOIN [dbo].[Customers] c ON b.[CustomerId] = c.[Id]
    INNER JOIN [dbo].[Users] u ON c.[UserId] = u.[Id]
    LEFT JOIN [dbo].[Events] e ON b.[EventId] = e.[Id]
    LEFT JOIN [dbo].[Venues] v ON b.[VenueId] = v.[Id]
    LEFT JOIN [dbo].[Packages] p ON b.[PackageId] = p.[Id]
    WHERE b.[CreatedAt] BETWEEN @StartDate AND DATEADD(DAY, 1, @EndDate)
        AND (@Status IS NULL OR b.[Status] = @Status)
    ORDER BY b.[CreatedAt] DESC;
END;
GO

-- ============================================================
-- SP: Get Monthly Sales Chart Data
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetMonthlySalesData]
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Months AS (
        SELECT 1 AS MonthNum UNION ALL SELECT 2 UNION ALL SELECT 3
        UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6
        UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
        UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
    )
    SELECT 
        m.MonthNum,
        DATENAME(MONTH, DATEFROMPARTS(@Year, m.MonthNum, 1)) AS [MonthName],
        ISNULL(COUNT(b.[Id]), 0) AS [BookingCount],
        ISNULL(SUM(b.[TotalAmount]), 0) AS [Revenue]
    FROM Months m
    LEFT JOIN [dbo].[Bookings] b 
        ON MONTH(b.[CreatedAt]) = m.MonthNum 
        AND YEAR(b.[CreatedAt]) = @Year
        AND b.[Status] NOT IN ('Cancelled', 'Refunded')
    GROUP BY m.MonthNum
    ORDER BY m.MonthNum;
END;
GO

-- ============================================================
-- SP: Get Popular Packages
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetPopularPackages]
    @TopN INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopN)
        p.[Id],
        p.[Name],
        p.[BasePrice],
        p.[ImageUrl],
        COUNT(b.[Id]) AS [BookingCount],
        ISNULL(SUM(b.[TotalAmount]), 0) AS [TotalRevenue]
    FROM [dbo].[Packages] p
    LEFT JOIN [dbo].[Bookings] b ON b.[PackageId] = p.[Id] 
        AND b.[Status] NOT IN ('Cancelled', 'Refunded')
    WHERE p.[IsActive] = 1
    GROUP BY p.[Id], p.[Name], p.[BasePrice], p.[ImageUrl]
    ORDER BY [BookingCount] DESC;
END;
GO
