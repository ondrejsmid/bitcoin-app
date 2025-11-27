------------------------------------------------------------
-- Create database
------------------------------------------------------------
IF DB_ID('ondrej-bitcoinapp') IS NULL
    CREATE DATABASE [ondrej-bitcoinapp];
GO

USE [ondrej-bitcoinapp];
GO

------------------------------------------------------------
-- Create table: snapshot
------------------------------------------------------------
IF OBJECT_ID('dbo.snapshot', 'U') IS NOT NULL
    DROP TABLE dbo.snapshot;
GO

CREATE TABLE dbo.snapshot
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    note        NVARCHAR(255) NULL
);
GO

------------------------------------------------------------
-- Create table: snapshot_row
------------------------------------------------------------
IF OBJECT_ID('dbo.snapshot_row', 'U') IS NOT NULL
    DROP TABLE dbo.snapshot_row;
GO

CREATE TABLE dbo.snapshot_row
(
    id              INT IDENTITY(1,1) PRIMARY KEY,
    snapshot_id     INT NOT NULL,
    btc_fieldname   NVARCHAR(255) NOT NULL,
    btc_fieldvalue  NVARCHAR(255) NOT NULL,

    CONSTRAINT FK_snapshotrow_snapshot
        FOREIGN KEY (snapshot_id)
        REFERENCES dbo.snapshot(id)
        ON DELETE CASCADE
);
GO
