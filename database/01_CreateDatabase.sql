-- ============================================================
-- Elite Event Management System - Database Creation
-- SQL Server 2022+
-- ============================================================

USE master;
GO

-- Drop database if exists (development only)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'EliteEventDB')
BEGIN
    ALTER DATABASE [EliteEventDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [EliteEventDB];
END
GO

-- Create Database
CREATE DATABASE [EliteEventDB]
ON PRIMARY
(
    NAME = N'EliteEventDB',
    SIZE = 100MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 64MB
)
LOG ON
(
    NAME = N'EliteEventDB_Log',
    SIZE = 50MB,
    MAXSIZE = 2048GB,
    FILEGROWTH = 64MB
);
GO

USE [EliteEventDB];
GO

-- Enable snapshot isolation for better concurrency
ALTER DATABASE [EliteEventDB] SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE [EliteEventDB] SET READ_COMMITTED_SNAPSHOT ON;
GO
