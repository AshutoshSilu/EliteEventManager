-- ============================================================
-- Elite Event Management System - Table Creation
-- ============================================================

USE [EliteEventDB];
GO

-- ============================================================
-- LOOKUP / REFERENCE TABLES
-- ============================================================

-- Countries
CREATE TABLE [dbo].[Countries] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL,
    [Code]        NVARCHAR(5) NOT NULL,
    [PhoneCode]   NVARCHAR(10) NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- States
CREATE TABLE [dbo].[States] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL,
    [Code]        NVARCHAR(10) NULL,
    [CountryId]   INT NOT NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_States_Countries] FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries]([Id])
);
GO

-- Cities
CREATE TABLE [dbo].[Cities] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL,
    [StateId]     INT NOT NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Cities_States] FOREIGN KEY ([StateId])
        REFERENCES [dbo].[States]([Id])
);
GO

-- ============================================================
-- IDENTITY & ACCESS MANAGEMENT
-- ============================================================

-- Roles
CREATE TABLE [dbo].[Roles] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(50) NOT NULL UNIQUE,
    [Description] NVARCHAR(200) NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]   DATETIME2 NULL
);
GO

-- Permissions
CREATE TABLE [dbo].[Permissions] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL UNIQUE,
    [Module]      NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- RolePermissions (Many-to-Many)
CREATE TABLE [dbo].[RolePermissions] (
    [RoleId]       INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY ([RoleId])
        REFERENCES [dbo].[Roles]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId])
        REFERENCES [dbo].[Permissions]([Id]) ON DELETE CASCADE
);
GO

-- Users
CREATE TABLE [dbo].[Users] (
    [Id]                  UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [FirstName]           NVARCHAR(50) NOT NULL,
    [LastName]            NVARCHAR(50) NOT NULL,
    [Email]               NVARCHAR(256) NOT NULL UNIQUE,
    [PasswordHash]        NVARCHAR(500) NOT NULL,
    [PhoneNumber]         NVARCHAR(20) NULL,
    [ProfileImageUrl]     NVARCHAR(500) NULL,
    [RoleId]              INT NOT NULL,
    [IsActive]            BIT NOT NULL DEFAULT 1,
    [IsEmailVerified]     BIT NOT NULL DEFAULT 0,
    [EmailVerificationToken] NVARCHAR(500) NULL,
    [PasswordResetToken]  NVARCHAR(500) NULL,
    [PasswordResetExpiry] DATETIME2 NULL,
    [RefreshToken]        NVARCHAR(500) NULL,
    [RefreshTokenExpiry]  DATETIME2 NULL,
    [LastLoginAt]         DATETIME2 NULL,
    [CreatedAt]           DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]           DATETIME2 NULL,
    [IsDeleted]           BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId])
        REFERENCES [dbo].[Roles]([Id])
);
GO

-- Customers
CREATE TABLE [dbo].[Customers] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NOT NULL UNIQUE,
    [DateOfBirth]     DATE NULL,
    [Gender]          NVARCHAR(10) NULL,
    [Address]         NVARCHAR(500) NULL,
    [CityId]          INT NULL,
    [StateId]         INT NULL,
    [CountryId]       INT NULL,
    [ZipCode]         NVARCHAR(20) NULL,
    [CompanyName]     NVARCHAR(200) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Customers_Users] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Customers_Cities] FOREIGN KEY ([CityId])
        REFERENCES [dbo].[Cities]([Id]),
    CONSTRAINT [FK_Customers_States] FOREIGN KEY ([StateId])
        REFERENCES [dbo].[States]([Id]),
    CONSTRAINT [FK_Customers_Countries] FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries]([Id])
);
GO

-- Employees
CREATE TABLE [dbo].[Employees] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NOT NULL UNIQUE,
    [EmployeeCode]    NVARCHAR(20) NOT NULL UNIQUE,
    [Department]      NVARCHAR(100) NULL,
    [Designation]     NVARCHAR(100) NULL,
    [DateOfJoining]   DATE NOT NULL,
    [EmploymentStatus] NVARCHAR(30) NOT NULL DEFAULT 'Pending Onboarding',
    [Salary]          DECIMAL(18,2) NULL,
    [Address]         NVARCHAR(500) NULL,
    [CityId]          INT NULL,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Employees_Users] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Cities] FOREIGN KEY ([CityId])
        REFERENCES [dbo].[Cities]([Id]),
    CONSTRAINT [CK_Employees_EmploymentStatus]
        CHECK ([EmploymentStatus] IN ('Pending Onboarding', 'Onboarded', 'Resigned', 'Terminated'))
);
GO

-- ============================================================
-- VENDOR MANAGEMENT
-- ============================================================

-- Vendor Categories
CREATE TABLE [dbo].[VendorCategories] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IconUrl]     NVARCHAR(500) NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Vendors
CREATE TABLE [dbo].[Vendors] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NOT NULL UNIQUE,
    [BusinessName]    NVARCHAR(200) NOT NULL,
    [CategoryId]      INT NOT NULL,
    [Description]     NVARCHAR(2000) NULL,
    [ContactPerson]   NVARCHAR(100) NULL,
    [Phone]           NVARCHAR(20) NULL,
    [Email]           NVARCHAR(256) NULL,
    [Website]         NVARCHAR(500) NULL,
    [Address]         NVARCHAR(500) NULL,
    [CityId]          INT NULL,
    [Rating]          DECIMAL(3,2) NULL DEFAULT 0,
    [TotalReviews]    INT NOT NULL DEFAULT 0,
    [PricePerHour]    DECIMAL(18,2) NULL,
    [PricePerEvent]   DECIMAL(18,2) NULL,
    [LogoUrl]         NVARCHAR(500) NULL,
    [IsVerified]      BIT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Vendors_Users] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [FK_Vendors_Categories] FOREIGN KEY ([CategoryId])
        REFERENCES [dbo].[VendorCategories]([Id]),
    CONSTRAINT [FK_Vendors_Cities] FOREIGN KEY ([CityId])
        REFERENCES [dbo].[Cities]([Id])
);
GO

-- ============================================================
-- VENUE MANAGEMENT
-- ============================================================

-- Venues
CREATE TABLE [dbo].[Venues] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Name]            NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(2000) NULL,
    [Address]         NVARCHAR(500) NOT NULL,
    [CityId]          INT NULL,
    [Latitude]        DECIMAL(10,8) NULL,
    [Longitude]       DECIMAL(11,8) NULL,
    [Capacity]        INT NOT NULL,
    [PricePerHour]    DECIMAL(18,2) NULL,
    [PricePerDay]     DECIMAL(18,2) NULL,
    [ContactPerson]   NVARCHAR(100) NULL,
    [ContactPhone]    NVARCHAR(20) NULL,
    [ContactEmail]    NVARCHAR(256) NULL,
    [Facilities]      NVARCHAR(2000) NULL,
    [Rules]           NVARCHAR(2000) NULL,
    [CoverImageUrl]   NVARCHAR(500) NULL,
    [Rating]          DECIMAL(3,2) NULL DEFAULT 0,
    [TotalReviews]    INT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [IsFeatured]      BIT NOT NULL DEFAULT 0,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    [IsDeleted]       BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Venues_Cities] FOREIGN KEY ([CityId])
        REFERENCES [dbo].[Cities]([Id])
);
GO

-- Venue Images
CREATE TABLE [dbo].[VenueImages] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [VenueId]     INT NOT NULL,
    [ImageUrl]    NVARCHAR(500) NOT NULL,
    [Caption]     NVARCHAR(200) NULL,
    [SortOrder]   INT NOT NULL DEFAULT 0,
    [IsPrimary]   BIT NOT NULL DEFAULT 0,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_VenueImages_Venues] FOREIGN KEY ([VenueId])
        REFERENCES [dbo].[Venues]([Id]) ON DELETE CASCADE
);
GO

-- Venue Availability
CREATE TABLE [dbo].[VenueAvailability] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [VenueId]     INT NOT NULL,
    [Date]        DATE NOT NULL,
    [StartTime]   TIME NULL,
    [EndTime]     TIME NULL,
    [IsAvailable] BIT NOT NULL DEFAULT 1,
    [Notes]       NVARCHAR(500) NULL,
    CONSTRAINT [FK_VenueAvailability_Venues] FOREIGN KEY ([VenueId])
        REFERENCES [dbo].[Venues]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_VenueAvailability] UNIQUE ([VenueId], [Date], [StartTime])
);
GO

-- ============================================================
-- EVENT MANAGEMENT
-- ============================================================

-- Event Categories
CREATE TABLE [dbo].[EventCategories] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IconUrl]     NVARCHAR(500) NULL,
    [ImageUrl]    NVARCHAR(500) NULL,
    [IsActive]    BIT NOT NULL DEFAULT 1,
    [SortOrder]   INT NOT NULL DEFAULT 0,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Events
CREATE TABLE [dbo].[Events] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Title]           NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(4000) NULL,
    [CategoryId]      INT NOT NULL,
    [VenueId]         INT NULL,
    [StartDate]       DATETIME2 NOT NULL,
    [EndDate]         DATETIME2 NOT NULL,
    [StartTime]       TIME NULL,
    [EndTime]         TIME NULL,
    [MaxAttendees]    INT NULL,
    [CurrentAttendees] INT NOT NULL DEFAULT 0,
    [Price]           DECIMAL(18,2) NOT NULL DEFAULT 0,
    [DiscountPrice]   DECIMAL(18,2) NULL,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [CoverImageUrl]   NVARCHAR(500) NULL,
    [Tags]            NVARCHAR(500) NULL,
    [IsFeatured]      BIT NOT NULL DEFAULT 0,
    [IsPublished]     BIT NOT NULL DEFAULT 0,
    [OrganizerId]     UNIQUEIDENTIFIER NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    [IsDeleted]       BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Events_Categories] FOREIGN KEY ([CategoryId])
        REFERENCES [dbo].[EventCategories]([Id]),
    CONSTRAINT [FK_Events_Venues] FOREIGN KEY ([VenueId])
        REFERENCES [dbo].[Venues]([Id]),
    CONSTRAINT [FK_Events_Organizer] FOREIGN KEY ([OrganizerId])
        REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [CK_Events_Status] CHECK ([Status] IN ('Draft','Published','Ongoing','Completed','Cancelled'))
);
GO

-- Event Images
CREATE TABLE [dbo].[EventImages] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [EventId]     INT NOT NULL,
    [ImageUrl]    NVARCHAR(500) NOT NULL,
    [Caption]     NVARCHAR(200) NULL,
    [SortOrder]   INT NOT NULL DEFAULT 0,
    [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_EventImages_Events] FOREIGN KEY ([EventId])
        REFERENCES [dbo].[Events]([Id]) ON DELETE CASCADE
);
GO

-- ============================================================
-- PACKAGES
-- ============================================================

-- Packages
CREATE TABLE [dbo].[Packages] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Name]            NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(2000) NULL,
    [CategoryId]      INT NULL,
    [BasePrice]       DECIMAL(18,2) NOT NULL,
    [DiscountPrice]   DECIMAL(18,2) NULL,
    [Duration]        NVARCHAR(50) NULL,
    [MaxGuests]       INT NULL,
    [ImageUrl]        NVARCHAR(500) NULL,
    [IsPopular]       BIT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [SortOrder]       INT NOT NULL DEFAULT 0,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Packages_Categories] FOREIGN KEY ([CategoryId])
        REFERENCES [dbo].[EventCategories]([Id])
);
GO

-- Package Services
CREATE TABLE [dbo].[PackageServices] (
    [Id]          INT IDENTITY(1,1) PRIMARY KEY,
    [PackageId]   INT NOT NULL,
    [ServiceName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IsIncluded]  BIT NOT NULL DEFAULT 1,
    [SortOrder]   INT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_PackageServices_Packages] FOREIGN KEY ([PackageId])
        REFERENCES [dbo].[Packages]([Id]) ON DELETE CASCADE
);
GO

-- ============================================================
-- BOOKING MANAGEMENT
-- ============================================================

-- Bookings
CREATE TABLE [dbo].[Bookings] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [BookingNumber]   NVARCHAR(20) NOT NULL UNIQUE,
    [CustomerId]      INT NOT NULL,
    [EventId]         INT NULL,
    [VenueId]         INT NULL,
    [PackageId]       INT NULL,
    [EventDate]       DATE NOT NULL,
    [StartTime]       TIME NULL,
    [EndTime]         TIME NULL,
    [GuestCount]      INT NOT NULL DEFAULT 1,
    [SpecialRequests] NVARCHAR(2000) NULL,
    [SubTotal]        DECIMAL(18,2) NOT NULL DEFAULT 0,
    [DiscountAmount]  DECIMAL(18,2) NOT NULL DEFAULT 0,
    [TaxAmount]       DECIMAL(18,2) NOT NULL DEFAULT 0,
    [TotalAmount]     DECIMAL(18,2) NOT NULL DEFAULT 0,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    [CouponId]        INT NULL,
    [Notes]           NVARCHAR(1000) NULL,
    [ApprovedBy]      UNIQUEIDENTIFIER NULL,
    [ApprovedAt]      DATETIME2 NULL,
    [CancelledAt]     DATETIME2 NULL,
    [CancelReason]    NVARCHAR(500) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Bookings_Customers] FOREIGN KEY ([CustomerId])
        REFERENCES [dbo].[Customers]([Id]),
    CONSTRAINT [FK_Bookings_Events] FOREIGN KEY ([EventId])
        REFERENCES [dbo].[Events]([Id]),
    CONSTRAINT [FK_Bookings_Venues] FOREIGN KEY ([VenueId])
        REFERENCES [dbo].[Venues]([Id]),
    CONSTRAINT [FK_Bookings_Packages] FOREIGN KEY ([PackageId])
        REFERENCES [dbo].[Packages]([Id]),
    CONSTRAINT [FK_Bookings_ApprovedBy] FOREIGN KEY ([ApprovedBy])
        REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [CK_Bookings_Status] CHECK ([Status] IN ('Pending','Confirmed','InProgress','Completed','Cancelled','Refunded'))
);
GO

-- Booking Details (line items / vendor assignments)
CREATE TABLE [dbo].[BookingDetails] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [BookingId]       INT NOT NULL,
    [VendorId]        INT NULL,
    [ServiceName]     NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(500) NULL,
    [Quantity]        INT NOT NULL DEFAULT 1,
    [UnitPrice]       DECIMAL(18,2) NOT NULL,
    [TotalPrice]      DECIMAL(18,2) NOT NULL,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    [Notes]           NVARCHAR(500) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_BookingDetails_Bookings] FOREIGN KEY ([BookingId])
        REFERENCES [dbo].[Bookings]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BookingDetails_Vendors] FOREIGN KEY ([VendorId])
        REFERENCES [dbo].[Vendors]([Id])
);
GO

-- ============================================================
-- PAYMENT & INVOICING
-- ============================================================

-- Payments
CREATE TABLE [dbo].[Payments] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [PaymentNumber]   NVARCHAR(20) NOT NULL UNIQUE,
    [BookingId]       INT NOT NULL,
    [CustomerId]      INT NOT NULL,
    [Amount]          DECIMAL(18,2) NOT NULL,
    [PaymentMethod]   NVARCHAR(30) NOT NULL,
    [TransactionId]   NVARCHAR(100) NULL,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    [PaymentDate]     DATETIME2 NULL,
    [GatewayResponse] NVARCHAR(2000) NULL,
    [RefundAmount]    DECIMAL(18,2) NULL,
    [RefundDate]      DATETIME2 NULL,
    [RefundReason]    NVARCHAR(500) NULL,
    [Notes]           NVARCHAR(500) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Payments_Bookings] FOREIGN KEY ([BookingId])
        REFERENCES [dbo].[Bookings]([Id]),
    CONSTRAINT [FK_Payments_Customers] FOREIGN KEY ([CustomerId])
        REFERENCES [dbo].[Customers]([Id]),
    CONSTRAINT [CK_Payments_Method] CHECK ([PaymentMethod] IN ('UPI','CreditCard','DebitCard','NetBanking','Cash','BankTransfer')),
    CONSTRAINT [CK_Payments_Status] CHECK ([Status] IN ('Pending','Processing','Completed','Failed','Refunded','PartialRefund'))
);
GO

-- Invoices
CREATE TABLE [dbo].[Invoices] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [InvoiceNumber]   NVARCHAR(20) NOT NULL UNIQUE,
    [BookingId]       INT NOT NULL,
    [CustomerId]      INT NOT NULL,
    [SubTotal]        DECIMAL(18,2) NOT NULL,
    [TaxAmount]       DECIMAL(18,2) NOT NULL DEFAULT 0,
    [DiscountAmount]  DECIMAL(18,2) NOT NULL DEFAULT 0,
    [TotalAmount]     DECIMAL(18,2) NOT NULL,
    [PaidAmount]      DECIMAL(18,2) NOT NULL DEFAULT 0,
    [DueAmount]       DECIMAL(18,2) NOT NULL,
    [DueDate]         DATE NULL,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'Unpaid',
    [Notes]           NVARCHAR(500) NULL,
    [IssuedAt]        DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [PaidAt]          DATETIME2 NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Invoices_Bookings] FOREIGN KEY ([BookingId])
        REFERENCES [dbo].[Bookings]([Id]),
    CONSTRAINT [FK_Invoices_Customers] FOREIGN KEY ([CustomerId])
        REFERENCES [dbo].[Customers]([Id]),
    CONSTRAINT [CK_Invoices_Status] CHECK ([Status] IN ('Unpaid','PartiallyPaid','Paid','Overdue','Cancelled'))
);
GO

-- ============================================================
-- REVIEWS & RATINGS
-- ============================================================

-- Reviews
CREATE TABLE [dbo].[Reviews] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerId]      INT NOT NULL,
    [EntityType]      NVARCHAR(30) NOT NULL, -- Event, Venue, Vendor, Package
    [EntityId]        INT NOT NULL,
    [Rating]          INT NOT NULL,
    [Title]           NVARCHAR(200) NULL,
    [Comment]         NVARCHAR(2000) NULL,
    [ImageUrl]        NVARCHAR(500) NULL,
    [Reply]           NVARCHAR(1000) NULL,
    [RepliedBy]       UNIQUEIDENTIFIER NULL,
    [RepliedAt]       DATETIME2 NULL,
    [IsApproved]      BIT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt]       DATETIME2 NULL,
    CONSTRAINT [FK_Reviews_Customers] FOREIGN KEY ([CustomerId])
        REFERENCES [dbo].[Customers]([Id]),
    CONSTRAINT [FK_Reviews_RepliedBy] FOREIGN KEY ([RepliedBy])
        REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [CK_Reviews_Rating] CHECK ([Rating] BETWEEN 1 AND 5),
    CONSTRAINT [CK_Reviews_EntityType] CHECK ([EntityType] IN ('Event','Venue','Vendor','Package'))
);
GO

-- ============================================================
-- GALLERY
-- ============================================================

-- Gallery
CREATE TABLE [dbo].[Gallery] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Title]           NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(500) NULL,
    [MediaType]       NVARCHAR(10) NOT NULL DEFAULT 'Image',
    [MediaUrl]        NVARCHAR(500) NOT NULL,
    [ThumbnailUrl]    NVARCHAR(500) NULL,
    [AlbumName]       NVARCHAR(100) NULL,
    [EventId]         INT NULL,
    [SortOrder]       INT NOT NULL DEFAULT 0,
    [IsFeatured]      BIT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Gallery_Events] FOREIGN KEY ([EventId])
        REFERENCES [dbo].[Events]([Id]),
    CONSTRAINT [CK_Gallery_MediaType] CHECK ([MediaType] IN ('Image','Video'))
);
GO

-- ============================================================
-- NOTIFICATIONS
-- ============================================================

-- Notifications
CREATE TABLE [dbo].[Notifications] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [Title]           NVARCHAR(200) NOT NULL,
    [Message]         NVARCHAR(1000) NOT NULL,
    [Type]            NVARCHAR(30) NOT NULL DEFAULT 'Info',
    [Channel]         NVARCHAR(20) NOT NULL DEFAULT 'InApp',
    [ReferenceType]   NVARCHAR(30) NULL,
    [ReferenceId]     INT NULL,
    [IsRead]          BIT NOT NULL DEFAULT 0,
    [ReadAt]          DATETIME2 NULL,
    [SentAt]          DATETIME2 NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Notifications_Users] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_Notifications_Type] CHECK ([Type] IN ('Info','Success','Warning','Error','Reminder')),
    CONSTRAINT [CK_Notifications_Channel] CHECK ([Channel] IN ('InApp','Email','SMS','WhatsApp'))
);
GO

-- ============================================================
-- TESTIMONIALS
-- ============================================================

-- Testimonials
CREATE TABLE [dbo].[Testimonials] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerName]    NVARCHAR(100) NOT NULL,
    [Designation]     NVARCHAR(100) NULL,
    [Company]         NVARCHAR(100) NULL,
    [Content]         NVARCHAR(1000) NOT NULL,
    [Rating]          INT NOT NULL DEFAULT 5,
    [PhotoUrl]        NVARCHAR(500) NULL,
    [IsApproved]      BIT NOT NULL DEFAULT 0,
    [IsFeatured]      BIT NOT NULL DEFAULT 0,
    [SortOrder]       INT NOT NULL DEFAULT 0,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- ============================================================
-- COUPONS & OFFERS
-- ============================================================

-- Coupons
CREATE TABLE [dbo].[Coupons] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Code]            NVARCHAR(50) NOT NULL UNIQUE,
    [Description]     NVARCHAR(500) NULL,
    [DiscountType]    NVARCHAR(20) NOT NULL, -- Percentage, FixedAmount
    [DiscountValue]   DECIMAL(18,2) NOT NULL,
    [MinOrderAmount]  DECIMAL(18,2) NULL,
    [MaxDiscountAmount] DECIMAL(18,2) NULL,
    [UsageLimit]      INT NULL,
    [UsedCount]       INT NOT NULL DEFAULT 0,
    [StartDate]       DATE NOT NULL,
    [EndDate]         DATE NOT NULL,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [CK_Coupons_DiscountType] CHECK ([DiscountType] IN ('Percentage','FixedAmount'))
);
GO

-- Offers
CREATE TABLE [dbo].[Offers] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Title]           NVARCHAR(200) NOT NULL,
    [Description]     NVARCHAR(1000) NULL,
    [DiscountPercentage] DECIMAL(5,2) NOT NULL,
    [EntityType]      NVARCHAR(30) NULL, -- Event, Venue, Package
    [EntityId]        INT NULL,
    [ImageUrl]        NVARCHAR(500) NULL,
    [StartDate]       DATE NOT NULL,
    [EndDate]         DATE NOT NULL,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- ============================================================
-- SYSTEM TABLES
-- ============================================================

-- Audit Logs
CREATE TABLE [dbo].[AuditLogs] (
    [Id]              BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NULL,
    [Action]          NVARCHAR(50) NOT NULL,
    [EntityType]      NVARCHAR(50) NOT NULL,
    [EntityId]        NVARCHAR(50) NULL,
    [OldValues]       NVARCHAR(MAX) NULL,
    [NewValues]       NVARCHAR(MAX) NULL,
    [IpAddress]       NVARCHAR(50) NULL,
    [UserAgent]       NVARCHAR(500) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Settings
CREATE TABLE [dbo].[Settings] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Key]             NVARCHAR(100) NOT NULL UNIQUE,
    [Value]           NVARCHAR(2000) NOT NULL,
    [Description]     NVARCHAR(500) NULL,
    [Group]           NVARCHAR(50) NULL,
    [DataType]        NVARCHAR(20) NOT NULL DEFAULT 'String',
    [UpdatedAt]       DATETIME2 NULL,
    [UpdatedBy]       UNIQUEIDENTIFIER NULL
);
GO

-- FAQs
CREATE TABLE [dbo].[FAQs] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Question]        NVARCHAR(500) NOT NULL,
    [Answer]          NVARCHAR(2000) NOT NULL,
    [Category]        NVARCHAR(50) NULL,
    [SortOrder]       INT NOT NULL DEFAULT 0,
    [IsActive]        BIT NOT NULL DEFAULT 1,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Contact Messages
CREATE TABLE [dbo].[ContactMessages] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [Name]            NVARCHAR(100) NOT NULL,
    [Email]           NVARCHAR(256) NOT NULL,
    [Phone]           NVARCHAR(20) NULL,
    [Subject]         NVARCHAR(200) NOT NULL,
    [Message]         NVARCHAR(2000) NOT NULL,
    [Status]          NVARCHAR(20) NOT NULL DEFAULT 'New',
    [RepliedBy]       UNIQUEIDENTIFIER NULL,
    [RepliedAt]       DATETIME2 NULL,
    [Reply]           NVARCHAR(2000) NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [CK_ContactMessages_Status] CHECK ([Status] IN ('New','Read','Replied','Closed'))
);
GO

-- Wishlist
CREATE TABLE [dbo].[Wishlists] (
    [Id]              INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerId]      INT NOT NULL,
    [EntityType]      NVARCHAR(30) NOT NULL,
    [EntityId]        INT NOT NULL,
    [CreatedAt]       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Wishlists_Customers] FOREIGN KEY ([CustomerId])
        REFERENCES [dbo].[Customers]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_Wishlists] UNIQUE ([CustomerId], [EntityType], [EntityId])
);
GO
