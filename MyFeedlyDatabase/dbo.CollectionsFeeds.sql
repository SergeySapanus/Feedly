CREATE TABLE [dbo].[CollectionsFeeds] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [idCollection] INT NOT NULL,
    [idFeed]       INT NOT NULL,
    CONSTRAINT [PK_CollectionsFeeds] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CollectionsFeeds_Collections] FOREIGN KEY ([idCollection]) REFERENCES [dbo].[Collections] ([Id]),
    CONSTRAINT [FK_CollectionsFeeds_Feeds] FOREIGN KEY ([idFeed]) REFERENCES [dbo].[Feeds] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);



GO
CREATE NONCLUSTERED INDEX [IX_CollectionsFeeds_idFeed]
    ON [dbo].[CollectionsFeeds]([idFeed] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CollectionsFeeds_idCollection]
    ON [dbo].[CollectionsFeeds]([idCollection] ASC);

