CREATE TABLE [dbo].[TradingRating] (
	Id INT IDENTITY(1,1) NOT NULL,
	TradingId INT NOT NULL,
	RatingSign BIT NOT NULL DEFAULT 0,
	Comment NVARCHAR(200) DEFAULT NULL,
	MatchingDescription TINYINT DEFAULT NULL, --5-star scale
	FastDispatch TINYINT DEFAULT NULL, --5-star scale
	PostageSpeed TINYINT DEFAULT NULL, --5-star scale
	PackageProtection TINYINT DEFAULT NULL, --5-star scale
	CONSTRAINT [PK_TradingRating_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_TradingRating_Trading] FOREIGN KEY ([TradingId]) REFERENCES Trading ([Id])
)
