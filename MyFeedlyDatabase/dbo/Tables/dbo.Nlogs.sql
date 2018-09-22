CREATE TABLE [dbo].[Nlogs] (
    [id]        INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp] DATETIME2 (7)  NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [Level]     NVARCHAR (10)  NULL,
    [Logger]    NVARCHAR (128) NULL,
    CONSTRAINT [PK_Nlogs] PRIMARY KEY CLUSTERED ([id] ASC)
);

