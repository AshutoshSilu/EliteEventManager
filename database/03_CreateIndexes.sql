-- ============================================================
-- Elite Event Management System - Indexes
-- ============================================================

USE [EliteEventDB];
GO

-- Users indexes
CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]([Email]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]([RoleId]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Users_IsActive] ON [dbo].[Users]([IsActive]) WHERE [IsDeleted] = 0;
GO

-- Events indexes
CREATE NONCLUSTERED INDEX [IX_Events_CategoryId] ON [dbo].[Events]([CategoryId]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Events_VenueId] ON [dbo].[Events]([VenueId]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Events_Status] ON [dbo].[Events]([Status]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Events_StartDate] ON [dbo].[Events]([StartDate]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Events_IsFeatured] ON [dbo].[Events]([IsFeatured]) WHERE [IsDeleted] = 0 AND [IsPublished] = 1;
GO

-- Bookings indexes
CREATE NONCLUSTERED INDEX [IX_Bookings_CustomerId] ON [dbo].[Bookings]([CustomerId]);
CREATE NONCLUSTERED INDEX [IX_Bookings_EventId] ON [dbo].[Bookings]([EventId]);
CREATE NONCLUSTERED INDEX [IX_Bookings_Status] ON [dbo].[Bookings]([Status]);
CREATE NONCLUSTERED INDEX [IX_Bookings_EventDate] ON [dbo].[Bookings]([EventDate]);
CREATE NONCLUSTERED INDEX [IX_Bookings_CreatedAt] ON [dbo].[Bookings]([CreatedAt] DESC);
GO

-- Payments indexes
CREATE NONCLUSTERED INDEX [IX_Payments_BookingId] ON [dbo].[Payments]([BookingId]);
CREATE NONCLUSTERED INDEX [IX_Payments_CustomerId] ON [dbo].[Payments]([CustomerId]);
CREATE NONCLUSTERED INDEX [IX_Payments_Status] ON [dbo].[Payments]([Status]);
CREATE NONCLUSTERED INDEX [IX_Payments_PaymentDate] ON [dbo].[Payments]([PaymentDate]);
GO

-- Venues indexes
CREATE NONCLUSTERED INDEX [IX_Venues_CityId] ON [dbo].[Venues]([CityId]) WHERE [IsDeleted] = 0;
CREATE NONCLUSTERED INDEX [IX_Venues_IsFeatured] ON [dbo].[Venues]([IsFeatured]) WHERE [IsDeleted] = 0 AND [IsActive] = 1;
CREATE NONCLUSTERED INDEX [IX_Venues_Capacity] ON [dbo].[Venues]([Capacity]) WHERE [IsDeleted] = 0;
GO

-- Vendors indexes
CREATE NONCLUSTERED INDEX [IX_Vendors_CategoryId] ON [dbo].[Vendors]([CategoryId]) WHERE [IsActive] = 1;
CREATE NONCLUSTERED INDEX [IX_Vendors_Rating] ON [dbo].[Vendors]([Rating] DESC) WHERE [IsActive] = 1;
GO

-- Reviews indexes
CREATE NONCLUSTERED INDEX [IX_Reviews_Entity] ON [dbo].[Reviews]([EntityType], [EntityId]) WHERE [IsActive] = 1;
CREATE NONCLUSTERED INDEX [IX_Reviews_CustomerId] ON [dbo].[Reviews]([CustomerId]);
GO

-- Notifications indexes
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId] ON [dbo].[Notifications]([UserId], [IsRead]);
CREATE NONCLUSTERED INDEX [IX_Notifications_CreatedAt] ON [dbo].[Notifications]([CreatedAt] DESC);
GO

-- Gallery indexes
CREATE NONCLUSTERED INDEX [IX_Gallery_Album] ON [dbo].[Gallery]([AlbumName]) WHERE [IsActive] = 1;
CREATE NONCLUSTERED INDEX [IX_Gallery_Featured] ON [dbo].[Gallery]([IsFeatured]) WHERE [IsActive] = 1;
GO

-- Audit Logs indexes
CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserId] ON [dbo].[AuditLogs]([UserId]);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_EntityType] ON [dbo].[AuditLogs]([EntityType], [EntityId]);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_CreatedAt] ON [dbo].[AuditLogs]([CreatedAt] DESC);
GO

-- Coupons indexes
CREATE NONCLUSTERED INDEX [IX_Coupons_Code] ON [dbo].[Coupons]([Code]) WHERE [IsActive] = 1;
CREATE NONCLUSTERED INDEX [IX_Coupons_Dates] ON [dbo].[Coupons]([StartDate], [EndDate]) WHERE [IsActive] = 1;
GO
