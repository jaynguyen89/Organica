CREATE TABLE [dbo].[ChatMessage] (
	Id INT IDENTITY(1,1) NOT NULL,
	ParticipantId INT NOT NULL,
	GroupId INT NOT NULL,
	Content NVARCHAR(500) DEFAULT NULL,
	SentOn DATETIME2(7) NOT NULL DEFAULT (GETDATE()),
	IsHiddenFor NVARCHAR(500) DEFAULT NULL, --string contains participantId separated by comma
	IsVisible BIT NOT NULL DEFAULT 0,
	Attachment NVARCHAR(150) DEFAULT NULL,
	SeenByIds NVARCHAR(500) DEFAULT NULL, --string contains participantId separated by comma
	CONSTRAINT [PK_ChatMessage_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ChatMessage_ChatGroup] FOREIGN KEY ([GroupId]) REFERENCES ChatGroup ([Id]),
	CONSTRAINT [FK_ChatMessage_ChatParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES ChatParticipant ([Id])
)
