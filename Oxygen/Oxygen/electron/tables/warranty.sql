CREATE TABLE [dbo].[Warranty] (
	Id INT IDENTITY(1,1) NOT NULL,
	ItemId INT NOT NULL,
	Duration TINYINT NOT NULL DEFAULT 12, --months
	StartedOn DATETIME2(7) NOT NULL DEFAULT (GETDATE()),
	IsEditted BIT NOT NULL DEFAULT 0, --for integrity
	IsActive BIT NOT NULL DEFAULT 0, --if warranty voids before being expired
	CONSTRAINT [PK_Warranty_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_Warranty_Item] FOREIGN KEY ([ItemId]) REFERENCES Item ([Id])
)
