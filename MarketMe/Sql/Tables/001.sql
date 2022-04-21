IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CustomersDetails] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Address] nvarchar(500) NULL,
    [BusinessName] nvarchar(max) NULL,
    [BusinessEmail] nvarchar(max) NULL,
    [BusinessAddress] nvarchar(500) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_CustomersDetails] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220421060231_init', N'5.0.0');
GO

COMMIT;
GO

