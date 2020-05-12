CREATE TABLE [dbo].[SellerReliability] (
	Id INT IDENTITY(1,1) NOT NULL,
	RatingId INT NOT NULL,
	FairPrice TINYINT DEFAULT NULL, --5-star scale
	FastResponse TINYINT DEFAULT NULL, --5-star scale
	Politeness TINYINT DEFAULT NULL, --5-star scale
	Liablility TINYINT DEFAULT NULL, --5-star scale
	CONSTRAINT [PK_SellerReliability_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_SellerReliability_SellerRating] FOREIGN KEY ([RatingId]) REFERENCES SellerRating ([Id])
)
