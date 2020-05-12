CREATE TABLE [dbo].[HidroTrader] (
	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	BuyerRating FLOAT(24) NOT NULL DEFAULT 0,
	SellerRating FLOAT(24) NOT NULL DEFAULT 0,
	CONSTRAINT [PK_HidroBuyer_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_HidroBuyer_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES Hidrogenian ([Id])
)
