CREATE TABLE [dbo].[CountryRegion] (
	Id INT IDENTITY(1,1) NOT NULL,
	CountryId INT NOT NULL,
	RegionName NVARCHAR(50) DEFAULT NULL,
	RegionCode NVARCHAR(10) DEFAULT NULL,
	RegionClass TINYINT NOT NULL DEFAULT 0, --Enum numeric value: the higher the value is, the lower of class the region stays
	ResidedInId INT DEFAULT NULL,
	CONSTRAINT [PK_CountryRegion_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_CountryRegion_Country] FOREIGN KEY ([CountryId]) REFERENCES Country ([Id]),
	CONSTRAINT [FK_CountryRegion_Self] FOREIGN KEY ([ResidedInId]) REFERENCES CountryRegion ([Id])

)
