CREATE TABLE [dbo].[ChatMessage] (
    [Id]          NVARCHAR (450) NOT NULL,
    [IdFromUser]  NVARCHAR (450) NULL,
    [IdToUser]    NVARCHAR (450) NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [SendDate] DATETIME2 (7)  NOT NULL,
    [ReadDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChatMessage_AspNetUsers_IdFromUser] FOREIGN KEY ([IdFromUser]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ChatMessage_AspNetUsers_IdToUser] FOREIGN KEY ([IdToUser]) REFERENCES [dbo].[AspNetUsers] ([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_ChatMessage_IdFromUser]
    ON [dbo].[ChatMessage]([IdFromUser] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_ChatMessage_IdToUser]
    ON [dbo].[ChatMessage]([IdToUser] ASC);
GO

