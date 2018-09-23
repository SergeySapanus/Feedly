CREATE TABLE [dbo].[News] (
    [Id]      INT   IDENTITY (1, 1) NOT NULL,
    [Content] NTEXT NOT NULL,
    [FeedId]  INT   NOT NULL,
    CONSTRAINT [PK_New] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_News_Feeds] FOREIGN KEY ([FeedId]) REFERENCES [dbo].[Feeds] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);







GO
CREATE NONCLUSTERED INDEX [IX_News_idFeed]
    ON [dbo].[News]([FeedId] ASC);



