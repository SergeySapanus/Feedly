CREATE TABLE [dbo].[Feeds] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    [Hash] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Feeds] PRIMARY KEY CLUSTERED ([Id] ASC)
);






