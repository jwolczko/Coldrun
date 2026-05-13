USE [master];
GO

IF DB_ID(N'Coldrun') IS NULL
BEGIN
    CREATE DATABASE [Coldrun];
END
GO

USE [Coldrun];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

IF SCHEMA_ID(N'TruckManagement') IS NULL
BEGIN
    EXEC(N'CREATE SCHEMA [TruckManagement]');
END
GO

IF OBJECT_ID(N'[TruckManagement].[TruckStatuses]', N'U') IS NULL
BEGIN
    CREATE TABLE [TruckManagement].[TruckStatuses]
    (
        [Id] [smallint] NOT NULL,
        [Code] [varchar](32) NOT NULL,
        [Name] [nvarchar](64) NOT NULL,
        [SortOrder] [smallint] NOT NULL,
        CONSTRAINT [PK_TruckStatuses] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_TruckStatuses_Code] UNIQUE NONCLUSTERED ([Code] ASC),
        CONSTRAINT [UQ_TruckStatuses_Name] UNIQUE NONCLUSTERED ([Name] ASC)
    );
END
GO

IF OBJECT_ID(N'[TruckManagement].[Trucks]', N'U') IS NULL
BEGIN
    CREATE TABLE [TruckManagement].[Trucks]
    (
        [Code] [varchar](64) NOT NULL,
        [Name] [nvarchar](256) NOT NULL,
        [StatusId] [smallint] NOT NULL,
        [Description] [nvarchar](2000) NULL,
        CONSTRAINT [PK_Trucks] PRIMARY KEY CLUSTERED ([Code] ASC)
    );
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE [name] = N'FK_Trucks_TruckStatuses_StatusId'
        AND [parent_object_id] = OBJECT_ID(N'[TruckManagement].[Trucks]')
)
BEGIN
    ALTER TABLE [TruckManagement].[Trucks] WITH CHECK
    ADD CONSTRAINT [FK_Trucks_TruckStatuses_StatusId]
        FOREIGN KEY ([StatusId])
        REFERENCES [TruckManagement].[TruckStatuses] ([Id]);
END
GO

IF EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE [name] = N'FK_Trucks_TruckStatuses_StatusId'
        AND [parent_object_id] = OBJECT_ID(N'[TruckManagement].[Trucks]')
)
BEGIN
    ALTER TABLE [TruckManagement].[Trucks]
    CHECK CONSTRAINT [FK_Trucks_TruckStatuses_StatusId];
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [name] = N'IX_Trucks_StatusId'
        AND [object_id] = OBJECT_ID(N'[TruckManagement].[Trucks]')
)
BEGIN
    CREATE INDEX [IX_Trucks_StatusId]
    ON [TruckManagement].[Trucks] ([StatusId]);
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [name] = N'IX_Trucks_Name'
        AND [object_id] = OBJECT_ID(N'[TruckManagement].[Trucks]')
)
BEGIN
    CREATE INDEX [IX_Trucks_Name]
    ON [TruckManagement].[Trucks] ([Name]);
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [name] = N'IX_Trucks_Description'
        AND [object_id] = OBJECT_ID(N'[TruckManagement].[Trucks]')
)
BEGIN
    CREATE INDEX [IX_Trucks_Description]
    ON [TruckManagement].[Trucks] ([Description]);
END
GO

MERGE [TruckManagement].[TruckStatuses] AS [Target]
USING
(
    VALUES
        (CONVERT(smallint, 1), CONVERT(varchar(32), 'out_of_service'), CONVERT(nvarchar(64), N'Out Of Service'), CONVERT(smallint, 1)),
        (CONVERT(smallint, 2), CONVERT(varchar(32), 'loading'), CONVERT(nvarchar(64), N'Loading'), CONVERT(smallint, 2)),
        (CONVERT(smallint, 3), CONVERT(varchar(32), 'to_job'), CONVERT(nvarchar(64), N'To Job'), CONVERT(smallint, 3)),
        (CONVERT(smallint, 4), CONVERT(varchar(32), 'at_job'), CONVERT(nvarchar(64), N'At Job'), CONVERT(smallint, 4)),
        (CONVERT(smallint, 5), CONVERT(varchar(32), 'returning'), CONVERT(nvarchar(64), N'Returning'), CONVERT(smallint, 5))
) AS [Source] ([Id], [Code], [Name], [SortOrder])
ON [Target].[Id] = [Source].[Id]
WHEN MATCHED THEN
    UPDATE SET
        [Code] = [Source].[Code],
        [Name] = [Source].[Name],
        [SortOrder] = [Source].[SortOrder]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Code], [Name], [SortOrder])
    VALUES ([Source].[Id], [Source].[Code], [Source].[Name], [Source].[SortOrder]);
GO
