CREATE TABLE [dbo].[ChatParticipant] (
	Id INT IDENTITY(1,1) NOT NULL,
	ParticipantId INT NOT NULL,
	GroupId INT NOT NULL,
	AddedById INT DEFAULT NULL,
	TimeStamps NVARCHAR(1000) DEFAULT NULL, --in JSON: [{addedOn: DATETIME2, leftOn: DATETIME2}]
	CONSTRAINT [PK_ChatParticipant_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ChatParticipant_ChatGroup] FOREIGN KEY ([GroupId]) REFERENCES ChatGroup ([Id]),
	CONSTRAINT [FK_ChatParticipant_Hidrogenian] FOREIGN KEY ([ParticipantId]) REFERENCES Hidrogenian ([Id])
)
