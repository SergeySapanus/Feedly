CREATE TABLE [dbo].[Feeds] (
    [Id]   INT            NOT NULL IDENTITY,
    [Name] NVARCHAR (MAX) NOT NULL,
    [Hash] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Feeds] PRIMARY KEY CLUSTERED ([Id] ASC)
);


