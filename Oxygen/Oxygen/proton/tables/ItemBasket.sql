CREATE TABLE [dbo].[ItemBasket] (
	Id INT IDENTITY(1,1) NOT NULL,
	CartId INT NOT NULL,
	ListingId INT NOT NULL,
	DeliverToId INT NOT NULL,
	VariationId INT DEFAULT NULL,
	BundleId INT DEFAULT NULL,
	Amount DECIMAL(6,3) NOT NULL DEFAULT 0,
	BasketNote NVARCHAR(100) DEFAULT NULL,
	CONSTRAINT [PK_ItemBasket_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ItemBasket_Listing] FOREIGN KEY ([ListingId]) REFERENCES Listing ([Id]),
	CONSTRAINT [FK_ItemBasket_ShoppingCart] FOREIGN KEY ([CartId]) REFERENCES ShoppingCart ([Id]),
	CONSTRAINT [FK_ItemBasket_HidroAddress] FOREIGN KEY ([DeliverToId]) REFERENCES HidroAddress ([Id]),
	CONSTRAINT [FK_ItemBasket_ItemVariation] FOREIGN KEY ([VariationId]) REFERENCES ItemVariation ([Id]),
	CONSTRAINT [FK_ItemBasket_ItemBundle] FOREIGN KEY ([BundleId]) REFERENCES ItemBundle ([Id])
)
