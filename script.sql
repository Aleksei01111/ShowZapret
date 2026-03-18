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
CREATE TABLE [User] (
    [Id] int NOT NULL IDENTITY,
    [Login] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);

CREATE TABLE [Note] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_Note] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Note_User] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Rule] (
    [Id] int NOT NULL IDENTITY,
    [NameOfRule] nvarchar(max) NOT NULL,
    [TriggerWord] nvarchar(max) NOT NULL,
    [TriggerWordMatchThreshold] real NOT NULL,
    [UserCreatorId] int NOT NULL,
    [FreedomPunishInMonth] real NOT NULL,
    [MoneyPunishmentInRubles] real NOT NULL,
    [RightWordNext] nvarchar(max) NULL,
    [ThresholdForRightWordNext] real NULL,
    [RightWordPrevious] nvarchar(max) NULL,
    [ThresholdForRightWordPrevious] real NULL,
    CONSTRAINT [PK_Rule] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Rule_User_UserCreatorId] FOREIGN KEY ([UserCreatorId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Note_UserId] ON [Note] ([UserId]);

CREATE INDEX [IX_Rule_UserCreatorId] ON [Rule] ([UserCreatorId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260318144725_Initial', N'9.0.14');

COMMIT;
GO

