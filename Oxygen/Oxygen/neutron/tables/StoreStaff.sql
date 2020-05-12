CREATE TABLE [dbo].[StoreStaff] (
	Id INT IDENTITY(1,1) NOT NULL,
	StoreId INT NOT NULL,
	StaffId INT NOT NULL,
	Position TINYINT NOT NULL DEFAULT 0, --Enum numeric value
	[Description] NVARCHAR(1500) DEFAULT NULL,
	AddedOn DATETIME2(7) NOT NULL DEFAULT (GETDATE()),
	CONSTRAINT [PK_StoreStaff_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_StoreStaff_HidroStore] FOREIGN KEY ([StoreId]) REFERENCES HidroStore ([Id]),
	CONSTRAINT [FK_StoreStaff_Hidrogenian] FOREIGN KEY ([StaffId]) REFERENCES Hidrogenian ([Id]),
)
