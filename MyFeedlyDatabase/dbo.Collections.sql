CREATE TABLE [dbo].[Collections] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [idUser] INT           NOT NULL,
    CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Collections_Users] FOREIGN KEY ([idUser]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);





GO

CREATE INDEX [IX_Collections_idUser] ON [dbo].[Collections] ([idUser])
