-- ============================================================
-- Elite Event Management System - Seed Data
-- ============================================================

USE [EliteEventDB];
GO

-- ============================================================
-- Roles
-- ============================================================
SET IDENTITY_INSERT [dbo].[Roles] ON;
INSERT INTO [dbo].[Roles] ([Id], [Name], [Description]) VALUES
(1, 'Administrator', 'Full system access'),
(2, 'EventManager', 'Manages events, bookings, vendors'),
(3, 'Customer', 'Can browse events, make bookings'),
(4, 'Vendor', 'Service provider role'),
(5, 'Visitor', 'Read-only public access');
SET IDENTITY_INSERT [dbo].[Roles] OFF;
GO

-- ============================================================
-- Permissions
-- ============================================================
INSERT INTO [dbo].[Permissions] ([Name], [Module], [Description]) VALUES
-- User Management
('Users.View', 'Users', 'View users'),
('Users.Create', 'Users', 'Create users'),
('Users.Edit', 'Users', 'Edit users'),
('Users.Delete', 'Users', 'Delete users'),
-- Events
('Events.View', 'Events', 'View events'),
('Events.Create', 'Events', 'Create events'),
('Events.Edit', 'Events', 'Edit events'),
('Events.Delete', 'Events', 'Delete events'),
-- Bookings
('Bookings.View', 'Bookings', 'View bookings'),
('Bookings.Create', 'Bookings', 'Create bookings'),
('Bookings.Edit', 'Bookings', 'Edit bookings'),
('Bookings.Approve', 'Bookings', 'Approve bookings'),
('Bookings.Cancel', 'Bookings', 'Cancel bookings'),
-- Venues
('Venues.View', 'Venues', 'View venues'),
('Venues.Create', 'Venues', 'Create venues'),
('Venues.Edit', 'Venues', 'Edit venues'),
('Venues.Delete', 'Venues', 'Delete venues'),
-- Vendors
('Vendors.View', 'Vendors', 'View vendors'),
('Vendors.Create', 'Vendors', 'Create vendors'),
('Vendors.Edit', 'Vendors', 'Edit vendors'),
('Vendors.Delete', 'Vendors', 'Delete vendors'),
-- Payments
('Payments.View', 'Payments', 'View payments'),
('Payments.Process', 'Payments', 'Process payments'),
('Payments.Refund', 'Payments', 'Process refunds'),
-- Reports
('Reports.View', 'Reports', 'View reports'),
('Reports.Export', 'Reports', 'Export reports'),
-- Settings
('Settings.View', 'Settings', 'View settings'),
('Settings.Edit', 'Settings', 'Edit settings');
GO

-- Assign all permissions to Administrator
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT 1, [Id] FROM [dbo].[Permissions];
GO

-- Assign EventManager permissions
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT 2, [Id] FROM [dbo].[Permissions] 
WHERE [Module] IN ('Events', 'Bookings', 'Venues', 'Vendors', 'Payments', 'Reports');
GO

-- Assign Customer permissions
INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT 3, [Id] FROM [dbo].[Permissions] 
WHERE [Name] IN ('Events.View', 'Bookings.View', 'Bookings.Create', 'Venues.View', 'Vendors.View', 'Payments.View');
GO

-- ============================================================
-- Countries, States, Cities
-- ============================================================
SET IDENTITY_INSERT [dbo].[Countries] ON;
INSERT INTO [dbo].[Countries] ([Id], [Name], [Code], [PhoneCode]) VALUES
(1, 'India', 'IN', '+91'),
(2, 'United States', 'US', '+1'),
(3, 'United Kingdom', 'GB', '+44');
SET IDENTITY_INSERT [dbo].[Countries] OFF;
GO

SET IDENTITY_INSERT [dbo].[States] ON;
INSERT INTO [dbo].[States] ([Id], [Name], [Code], [CountryId]) VALUES
(1, 'Maharashtra', 'MH', 1),
(2, 'Karnataka', 'KA', 1),
(3, 'Delhi', 'DL', 1),
(4, 'Tamil Nadu', 'TN', 1),
(5, 'California', 'CA', 2),
(6, 'New York', 'NY', 2),
(7, 'England', 'ENG', 3);
SET IDENTITY_INSERT [dbo].[States] OFF;
GO

SET IDENTITY_INSERT [dbo].[Cities] ON;
INSERT INTO [dbo].[Cities] ([Id], [Name], [StateId]) VALUES
(1, 'Mumbai', 1),
(2, 'Pune', 1),
(3, 'Bangalore', 2),
(4, 'New Delhi', 3),
(5, 'Chennai', 4),
(6, 'San Francisco', 5),
(7, 'Los Angeles', 5),
(8, 'New York City', 6),
(9, 'London', 7);
SET IDENTITY_INSERT [dbo].[Cities] OFF;
GO

-- ============================================================
-- Admin User (Password: Admin@123)
-- ============================================================
INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [PasswordHash], [PhoneNumber], [RoleId], [IsActive], [IsEmailVerified])
VALUES ('System', 'Administrator', 'admin@eliteevents.com', 
    '$2a$11$KxZmCklhN8dFwAmtGMaGkONLQNBJ0JXZLGZHlZ0hGYlKdVxGBqK3m',
    '+91-9876543210', 1, 1, 1);
GO

-- ============================================================
-- Vendor Categories
-- ============================================================
INSERT INTO [dbo].[VendorCategories] ([Name], [Description]) VALUES
('Photography', 'Professional photography services'),
('Decoration', 'Event decoration and floral arrangements'),
('DJ & Music', 'DJ and music entertainment services'),
('Catering', 'Food and beverage catering services'),
('Lighting', 'Professional lighting and effects'),
('Security', 'Event security services'),
('Entertainment', 'Live entertainment and performers'),
('Transportation', 'Guest transportation services'),
('Makeup & Styling', 'Professional makeup and styling'),
('Video & Film', 'Videography and film production');
GO

-- ============================================================
-- Event Categories
-- ============================================================
INSERT INTO [dbo].[EventCategories] ([Name], [Description], [SortOrder]) VALUES
('Wedding', 'Wedding ceremonies and receptions', 1),
('Corporate', 'Corporate events and conferences', 2),
('Birthday', 'Birthday celebrations and parties', 3),
('Anniversary', 'Anniversary celebrations', 4),
('Concert', 'Music concerts and shows', 5),
('Exhibition', 'Trade shows and exhibitions', 6),
('Conference', 'Seminars and conferences', 7),
('Social Gathering', 'Social events and get-togethers', 8),
('Festival', 'Cultural and religious festivals', 9),
('Sports', 'Sports events and tournaments', 10);
GO

-- ============================================================
-- FAQs
-- ============================================================
INSERT INTO [dbo].[FAQs] ([Question], [Answer], [Category], [SortOrder]) VALUES
('How do I book an event?', 'You can book an event by registering on our platform, browsing available events or packages, and clicking the Book Now button. Follow the steps to complete your booking.', 'Booking', 1),
('What payment methods do you accept?', 'We accept UPI, Credit Cards, Debit Cards, and Net Banking. All payments are processed securely.', 'Payment', 2),
('Can I cancel my booking?', 'Yes, you can cancel your booking from your dashboard. Cancellation policies vary depending on how close the event date is.', 'Booking', 3),
('How do I become a vendor?', 'Register on our platform as a vendor, complete your profile, and submit for verification. Once approved, you can start receiving bookings.', 'Vendor', 4),
('Do you offer custom packages?', 'Yes! Contact our team to create a custom package tailored to your specific requirements and budget.', 'Packages', 5);
GO

-- ============================================================
-- System Settings
-- ============================================================
INSERT INTO [dbo].[Settings] ([Key], [Value], [Description], [Group], [DataType]) VALUES
('Site.Name', 'Elite Event Management', 'Site display name', 'General', 'String'),
('Site.Logo', '/assets/images/logo.png', 'Site logo URL', 'General', 'String'),
('Site.Email', 'info@eliteevents.com', 'Contact email', 'General', 'String'),
('Site.Phone', '+91-9876543210', 'Contact phone', 'General', 'String'),
('Site.Address', '123 Event Street, Mumbai, India', 'Office address', 'General', 'String'),
('Booking.TaxPercentage', '18', 'Tax percentage for bookings', 'Booking', 'Decimal'),
('Booking.AutoApprove', 'false', 'Auto approve bookings', 'Booking', 'Boolean'),
('Payment.Currency', 'INR', 'Default currency', 'Payment', 'String'),
('Payment.GatewayKey', '', 'Payment gateway API key', 'Payment', 'String'),
('Email.SmtpHost', 'smtp.gmail.com', 'SMTP host', 'Email', 'String'),
('Email.SmtpPort', '587', 'SMTP port', 'Email', 'Integer'),
('Email.FromAddress', 'noreply@eliteevents.com', 'From email address', 'Email', 'String'),
('Notification.EnableEmail', 'true', 'Enable email notifications', 'Notification', 'Boolean'),
('Notification.EnableSMS', 'false', 'Enable SMS notifications', 'Notification', 'Boolean');
GO

-- ============================================================
-- Sample Testimonials
-- ============================================================
INSERT INTO [dbo].[Testimonials] ([CustomerName], [Designation], [Company], [Content], [Rating], [IsApproved], [IsFeatured], [SortOrder]) VALUES
('Rahul Sharma', 'CEO', 'TechVision Ltd', 'Elite Events made our corporate gala absolutely spectacular. The attention to detail was remarkable and our guests were thoroughly impressed.', 5, 1, 1, 1),
('Priya Patel', 'Bride', NULL, 'Our wedding was a dream come true thanks to Elite Events. Every moment was perfectly orchestrated and beautifully captured.', 5, 1, 1, 2),
('Amit Verma', 'Director', 'InnovateCorp', 'Professional, creative, and reliable. Elite Events handled our product launch flawlessly. Highly recommended!', 5, 1, 1, 3),
('Sneha Kapoor', 'Birthday Host', NULL, 'The birthday party they organized for my parents'' 50th anniversary was beyond our expectations. Truly elite service!', 4, 1, 0, 4);
GO
