CREATE TABLE [dbo].[Trading] (
	Id INT IDENTITY(1, 1) NOT NULL,
	BuyerId INT NOT NULL,
	SellerId INT NOT NULL,
	OrderId INT NOT NULL,
	HasItemInWarranty BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_Trading_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_Trading_HidroTrader_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES HidroTrader ([Id]),
	CONSTRAINT [FK_Trading_HidroTrader_Seller] FOREIGN KEY ([SellerId]) REFERENCES HidroTrader ([Id]),
	CONSTRAINT [FK_Trading_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]),
)
