CREATE TABLE [dbo].[CollectionsFeeds] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [CollectionId] INT NOT NULL,
    [FeedId]       INT NOT NULL,
    CONSTRAINT [PK_CollectionsFeeds] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CollectionsFeeds_Collections] FOREIGN KEY ([CollectionId]) REFERENCES [dbo].[Collections] ([Id]),
    CONSTRAINT [FK_CollectionsFeeds_Feeds] FOREIGN KEY ([FeedId]) REFERENCES [dbo].[Feeds] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);







GO
CREATE NONCLUSTERED INDEX [IX_CollectionsFeeds_idFeed]
    ON [dbo].[CollectionsFeeds]([FeedId] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_CollectionsFeeds_idCollection]
    ON [dbo].[CollectionsFeeds]([CollectionId] ASC);



