CREATE TABLE [dbo].[Collections] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [UserId] INT           NOT NULL,
    CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Collections_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [IX_Collections_UserId_Name] UNIQUE NONCLUSTERED ([UserId] ASC, [Name] ASC)
);















GO

CREATE NONCLUSTERED INDEX [IX_Collections_idUser]
    ON [dbo].[Collections]([UserId] ASC);


