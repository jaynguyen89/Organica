CREATE TABLE [dbo].[HidroAddress] (

	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	LocationId INT NOT NULL,
	IsPrimaryAddress BIT NOT NULL DEFAULT 0,
	IsDeliveryAddress BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_HidroAddress_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_HidroAddress_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES [dbo].[Hidrogenian] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_HidroAddress_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
)
