CREATE TABLE [dbo].[BuyerRating] (
	Id INT IDENTITY(1,1) NOT NULL,
	BuyerId INT NOT NULL,
	TradingId INT NOT NULL,
	RatedById INT NOT NULL,
	RatingSign BIT DEFAULT NULL, --null for neutral, 0 for negative, 1 for positive
	Comment NVARCHAR(200) DEFAULT NULL,
	Reliability NVARCHAR(200) DEFAULT NULL, --in JSON, summarize BuyerReliability table
	AllowEdit BIT NOT NULL DEFAULT 0,
	RatedOn DATETIME2 NOT NULL DEFAULT (GETDATE()),
	RevisedOn DATETIME2 DEFAULT NULL,
	CONSTRAINT [PK_BuyerRating_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_BuyerRating_HidroTrader_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES HidroTrader ([Id]),
	CONSTRAINT [FK_BuyerRating_Trading] FOREIGN KEY ([TradingId]) REFERENCES Trading ([Id]),
	CONSTRAINT [FK_BuyerRating_HidroTrader_Seller] FOREIGN KEY ([RatedById]) REFERENCES HidroTrader ([Id])
)
