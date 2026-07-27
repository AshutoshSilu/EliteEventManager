-- ============================================================
-- Elite Event Management System - Employee Employment Status
-- ============================================================

USE [EliteEventDB];
GO

IF COL_LENGTH('dbo.Employees', 'EmploymentStatus') IS NULL
BEGIN
    ALTER TABLE [dbo].[Employees]
    ADD [EmploymentStatus] NVARCHAR(30) NOT NULL
        CONSTRAINT [DF_Employees_EmploymentStatus] DEFAULT 'Pending Onboarding';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = 'CK_Employees_EmploymentStatus'
)
BEGIN
    ALTER TABLE [dbo].[Employees]
    ADD CONSTRAINT [CK_Employees_EmploymentStatus]
        CHECK ([EmploymentStatus] IN ('Pending Onboarding', 'Onboarded', 'Resigned', 'Terminated'));
END
GO

UPDATE [dbo].[Employees]
SET [EmploymentStatus] = CASE
    WHEN [IsActive] = 1 THEN 'Onboarded'
    ELSE 'Terminated'
END
WHERE [EmploymentStatus] IS NULL OR LTRIM(RTRIM([EmploymentStatus])) = '';
GO
