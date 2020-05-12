CREATE TABLE [dbo].[ItemStock] (
	Id INT IDENTITY(1, 1) NOT NULL,
	ItemId INT NOT NULL,
	VariationId INT DEFAULT NULL,
	BundleId INT DEFAULT NULL,
	StockQuantity DECIMAL(10,3) DEFAULT 0,
	CONSTRAINT [PK_ItemStock_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ItemStock_Item] FOREIGN KEY ([ItemId]) REFERENCES Item ([Id]),
	CONSTRAINT [FK_ItemStock_ItemVariation] FOREIGN KEY ([VariationId]) REFERENCES ItemVariation ([Id]),
	CONSTRAINT [FK_ItemStock_ItemBundle] FOREIGN KEY ([BundleId]) REFERENCES ItemBundle ([Id])
)
