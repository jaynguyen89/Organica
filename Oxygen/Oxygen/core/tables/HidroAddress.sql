CREATE TABLE [dbo].[HidroAddress] (

	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	LocationId INT DEFAULT NULL,
	Title NVARCHAR(30) DEFAULT NULL,
	IsRefined BIT NOT NULL DEFAULT 0,
	IsPrimaryAddress BIT NOT NULL DEFAULT 0,
	IsDeliveryAddress BIT NOT NULL DEFAULT 0,
	LastUpdated DATETIME2(7) NOT NULL DEFAULT (GETDATE()),
	CONSTRAINT [PK_HidroAddress_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_HidroAddress_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES [dbo].[Hidrogenian] ([Id]) ON DELETE CASCADE
)
