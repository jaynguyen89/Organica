CREATE TABLE [dbo].[BuyerReliability] (
	Id INT IDENTITY(1,1) NOT NULL,
	RatingId INT NOT NULL,
	FairNegotiation TINYINT DEFAULT NULL, --5-star scale
	FastResponse TINYINT DEFAULT NULL, --5-star scale
	QuickPayment TINYINT DEFAULT NULL, --5-star scale
	InTimePickup TINYINT DEFAULT NULL, --5-star scale
	Politeness TINYINT DEFAULT NULL, --5-star scale
	VotedOn DATETIME2(7) NOT NULL DEFAULT (GETDATE()),
	CONSTRAINT [PK_BuyerReliability_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_BuyerReliability_BuyerRating] FOREIGN KEY ([RatingId]) REFERENCES BuyerRating ([Id])
)
