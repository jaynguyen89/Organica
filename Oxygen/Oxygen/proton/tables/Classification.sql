CREATE TABLE [dbo].[Classification] (
	Id INT IDENTITY(1,1) NOT NULL,
	ListingId INT NOT NULL,
	CategoryId INT DEFAULT NULL,
	CONSTRAINT [PK_Classification_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_Classification_Category] FOREIGN KEY ([CategoryId]) REFERENCES Category([Id]),
	CONSTRAINT [FK_Classification_Listing] FOREIGN KEY ([ListingId]) REFERENCES Listing([Id])
)
