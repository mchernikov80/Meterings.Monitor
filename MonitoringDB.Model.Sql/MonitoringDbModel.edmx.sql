
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/22/2020 22:22:49
-- Generated from EDMX file: C:\Projects\Prosoft\Metering\MonitoringDB.Model.Sql\MonitoringDbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MonitoringDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MeteringDataMeteringPoint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeteringDataHistory] DROP CONSTRAINT [FK_MeteringDataMeteringPoint];
GO
IF OBJECT_ID(N'[dbo].[FK_MeteringDeviceInfoMeteringPoint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeteringDeviceHistory] DROP CONSTRAINT [FK_MeteringDeviceInfoMeteringPoint];
GO
IF OBJECT_ID(N'[dbo].[FK_MeteringDeviceInfoMeteringDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeteringDeviceHistory] DROP CONSTRAINT [FK_MeteringDeviceInfoMeteringDevice];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MeteringDevices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeteringDevices];
GO
IF OBJECT_ID(N'[dbo].[MeteringPoints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeteringPoints];
GO
IF OBJECT_ID(N'[dbo].[MeteringDataHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeteringDataHistory];
GO
IF OBJECT_ID(N'[dbo].[MeteringDeviceHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeteringDeviceHistory];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MeteringDevices'
CREATE TABLE [dbo].[MeteringDevices] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [DeviceType] nvarchar(max)  NOT NULL,
    [SerialNo] nvarchar(max)  NOT NULL,
    [CheckDate] datetime  NULL
);
GO

-- Creating table 'MeteringPoints'
CREATE TABLE [dbo].[MeteringPoints] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [House] nvarchar(max)  NOT NULL,
    [Flat] nvarchar(max)  NULL,
    [Owner] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MeteringDataHistory'
CREATE TABLE [dbo].[MeteringDataHistory] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [PointId] bigint  NOT NULL,
    [CheckedAt] datetime  NOT NULL,
    [Value] real  NOT NULL
);
GO

-- Creating table 'MeteringDeviceHistory'
CREATE TABLE [dbo].[MeteringDeviceHistory] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [MountedAt] datetime  NOT NULL,
    [DemountedAt] datetime  NULL,
    [DeviceId] bigint  NOT NULL,
    [PointId] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'MeteringDevices'
ALTER TABLE [dbo].[MeteringDevices]
ADD CONSTRAINT [PK_MeteringDevices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeteringPoints'
ALTER TABLE [dbo].[MeteringPoints]
ADD CONSTRAINT [PK_MeteringPoints]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeteringDataHistory'
ALTER TABLE [dbo].[MeteringDataHistory]
ADD CONSTRAINT [PK_MeteringDataHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeteringDeviceHistory'
ALTER TABLE [dbo].[MeteringDeviceHistory]
ADD CONSTRAINT [PK_MeteringDeviceHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PointId] in table 'MeteringDataHistory'
ALTER TABLE [dbo].[MeteringDataHistory]
ADD CONSTRAINT [FK_MeteringDataMeteringPoint]
    FOREIGN KEY ([PointId])
    REFERENCES [dbo].[MeteringPoints]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeteringDataMeteringPoint'
CREATE INDEX [IX_FK_MeteringDataMeteringPoint]
ON [dbo].[MeteringDataHistory]
    ([PointId]);
GO

-- Creating foreign key on [DeviceId] in table 'MeteringDeviceHistory'
ALTER TABLE [dbo].[MeteringDeviceHistory]
ADD CONSTRAINT [FK_MeteringDeviceInfoMeteringDevice]
    FOREIGN KEY ([DeviceId])
    REFERENCES [dbo].[MeteringDevices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeteringDeviceInfoMeteringDevice'
CREATE INDEX [IX_FK_MeteringDeviceInfoMeteringDevice]
ON [dbo].[MeteringDeviceHistory]
    ([DeviceId]);
GO

-- Creating foreign key on [PointId] in table 'MeteringDeviceHistory'
ALTER TABLE [dbo].[MeteringDeviceHistory]
ADD CONSTRAINT [FK_MeteringDeviceInfoMeteringPoint]
    FOREIGN KEY ([PointId])
    REFERENCES [dbo].[MeteringPoints]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeteringDeviceInfoMeteringPoint'
CREATE INDEX [IX_FK_MeteringDeviceInfoMeteringPoint]
ON [dbo].[MeteringDeviceHistory]
    ([PointId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------