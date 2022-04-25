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

CREATE TABLE [MarketmeRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_MarketmeRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [MarketmeUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_MarketmeUsers] PRIMARY KEY ([Id])
);
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

CREATE TABLE [MarketmeRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_MarketmeRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MarketmeRoleClaims_MarketmeRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [MarketmeRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MarketmeUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_MarketmeUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MarketmeUserClaims_MarketmeUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [MarketmeUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MarketmeUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_MarketmeUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_MarketmeUserLogins_MarketmeUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [MarketmeUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MarketmeUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_MarketmeUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_MarketmeUserRoles_MarketmeRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [MarketmeRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MarketmeUserRoles_MarketmeUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [MarketmeUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [MarketmeUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_MarketmeUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_MarketmeUserTokens_MarketmeUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [MarketmeUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_MarketmeRoleClaims_RoleId] ON [MarketmeRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [MarketmeRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_MarketmeUserClaims_UserId] ON [MarketmeUserClaims] ([UserId]);
GO

CREATE INDEX [IX_MarketmeUserLogins_UserId] ON [MarketmeUserLogins] ([UserId]);
GO

CREATE INDEX [IX_MarketmeUserRoles_RoleId] ON [MarketmeUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [MarketmeUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [MarketmeUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220423144709_init', N'5.0.0');
GO

COMMIT;
GO

