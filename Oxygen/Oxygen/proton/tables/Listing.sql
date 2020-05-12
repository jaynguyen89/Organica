CREATE TABLE [dbo].[Listing] (
	Id INT IDENTITY(1,1) NOT NULL,
	SellerId INT NOT NULL,
	LocationId INT DEFAULT NULL,
	IsCombo BIT NOT NULL DEFAULT 0, --combo prices are saved in MongoDb
	Title NVARCHAR(80) DEFAULT NULL,
	Caption NVARCHAR(50) DEFAULT NULL,
	Headline NVARCHAR(200) DEFAULT NULL,
	SellerNote NVARCHAR(70) DEFAULT NULL,
	[Description] NVARCHAR(MAX) DEFAULT NULL,
	SellingFormat TINYINT DEFAULT 0, --0 for fixed price, 1 for auction, 2 for invert auction, 3 for swap, 4 for buying, 5 for giving
	IsActive BIT NOT NULL DEFAULT 0,
	DropOnStockless BIT NOT NULL DEFAULT 0, --on out-of-stock, keep the listing and display "out of stock" label, or set it inactivate
	CreatedOn DATETIME2(7) DEFAULT (GETDATE()),
	CONSTRAINT [PK_Listing_Id] PRIMARY KEY ([Id] ASC)
)
