CREATE TABLE [dbo].[StoreOwner] (
	Id INT IDENTITY(1,1) NOT NULL,
	StoreId INT NOT NULL,
	OwnerId INT NOT NULL,
	SharedProfit FLOAT(24) NOT NULL DEFAULT 100, --percentage of profits shared among owners
	OwnerNote NVARCHAR(1000) DEFAULT NULL,
	JointOn DATETIME2(7) DEFAULT (GETDATE()),
	CONSTRAINT [PK_StoreOwner_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_StoreOwner_HidroStore] FOREIGN KEY ([StoreId]) REFERENCES HidroStore ([Id]),
	CONSTRAINT [FK_StoreOwner_Hidrogenian] FOREIGN KEY ([OwnerId]) REFERENCES Hidrogenian ([Id]),
)
