CREATE TABLE [dbo].[SellerRating] (
	Id INT IDENTITY(1,1) NOT NULL,
	SellerId INT NOT NULL,
	TradingId INT NOT NULL,
	RatedById INT NOT NULL,
	RatingSign BIT DEFAULT NULL, --null for neutral, 0 for negative, 1 for positive
	Comment NVARCHAR(200) DEFAULT NULL,
	AllowEdit BIT NOT NULL DEFAULT 0,
	RatedOn DATETIME2 NOT NULL DEFAULT (GETDATE()),
	RevisedOn DATETIME2 DEFAULT NULL,
	CONSTRAINT [PK_SellerRating_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_SellerRating_HidroSeller] FOREIGN KEY ([SellerId]) REFERENCES HidroTrader ([Id]),
	CONSTRAINT [FK_SellerRating_Listing] FOREIGN KEY ([TradingId]) REFERENCES Trading ([Id]),
	CONSTRAINT [FK_SellerRating_HidroBuyer] FOREIGN KEY ([RatedById]) REFERENCES HidroTrader ([Id])
)
