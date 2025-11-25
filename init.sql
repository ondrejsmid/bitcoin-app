-- Create database
CREATE DATABASE BitcoinAppDb;
GO

-- Use the database
USE BitcoinAppDb;
GO

-- Create table to store Bitcoin course data
CREATE TABLE BitcoinData (
    Id INT PRIMARY KEY IDENTITY(1,1),
    [Key] NVARCHAR(100) NOT NULL,
    [Value] NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
GO

-- Create index on Key for faster lookups
CREATE INDEX IX_BitcoinData_Key ON BitcoinData([Key]);
GO

-- Seed with two dummy rows
INSERT INTO BitcoinData ([Key], [Value]) VALUES (N'a', N'b');
INSERT INTO BitcoinData ([Key], [Value]) VALUES (N'c', N'd');
GO
