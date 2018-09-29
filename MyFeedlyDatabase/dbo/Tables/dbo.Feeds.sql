CREATE TABLE [dbo].[Feeds] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Uri]  NVARCHAR (MAX) NOT NULL,
    [Hash] INT            NOT NULL,
    CONSTRAINT [PK_Feeds] PRIMARY KEY CLUSTERED ([Id] ASC)
);












