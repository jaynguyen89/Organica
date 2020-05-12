CREATE TABLE [dbo].[ShoppingCart] (
	Id INT IDENTITY(1, 1) NOT NULL,
	BuyerId INT NOT NULL,
	CartNote NVARCHAR(100) DEFAULT NULL,
	CONSTRAINT [PK_ShoppingCart_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ShoppingCart_HidroTrader] FOREIGN KEY ([BuyerId]) REFERENCES HidroTrader ([Id])
)
