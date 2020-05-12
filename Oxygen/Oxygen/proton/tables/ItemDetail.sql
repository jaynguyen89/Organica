CREATE TABLE [dbo].[ItemDetail] (
	Id INT IDENTITY(1,1) NOT NULL,
	ItemId INT NOT NULL,
	Condition NVARCHAR(30) DEFAULT NULL, --New, used, crap...
	Quality NVARCHAR(50) DEFAULT NULL, --Good, fair, bad...
	Brand NVARCHAR(50) DEFAULT NULL,
	Model NVARCHAR(50) DEFAULT NULL,
	Storage INT DEFAULT NULL, --in MB, in JSON. main/secondary/others
	Memory INT DEFAULT NULL, --in MB
	ProductionNote NVARCHAR(200) DEFAULT NULL, --Thong tin phien ban,
	Color NVARCHAR(50) DEFAULT NULL, --in JSON, front/rear/side
	CarrierName NVARCHAR(50) DEFAULT NULL,
	LockStatus NVARCHAR(30) DEFAULT NULL,
	MadeIn NVARCHAR(50) DEFAULT NULL,
	Warranty SMALLINT DEFAULT NULL,
	WarrantedBy NVARCHAR(50) DEFAULT NULL,
	Size NVARCHAR(30) DEFAULT NULL, --For wearables, S/M/L/XL...
	Dimensions NVARCHAR(50) DEFAULT NULL, --in JSON, l/w/h, unit: millimeter
	FormFactor NVARCHAR(30) DEFAULT NULL,
	Certification NVARCHAR(50) DEFAULT NULL,
	Materials NVARCHAR(250) DEFAULT NULL, --in JSON, main/secondary/auxiliary/main stones/side stones/other,
	Measurement SMALLINT DEFAULT NULL, --inJSON, volume/weight/quantity/length
	Packaging NVARCHAR(30) DEFAULT NULL, --Can, bottle, box, pack, carton...
	[Version] NVARCHAR(30) DEFAULT NULL,
	[Processing] NVARCHAR DEFAULT NULL, --Handmade, Machine, Casting, (stones) 6-side/5-side/verconia...
	SerialOrSku NVARCHAR DEFAULT NULL,
	ToBeUsedFor NVARCHAR DEFAULT NULL, --Dog/cat/bird..., men/lady/child..., garden/farm/..., ....
	CompatibleWith NVARCHAR DEFAULT NULL, --in JSON. Windows/MacOS..., a specific device...
	CONSTRAINT [PK_ItemProperty_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ItemProperty_Item] FOREIGN KEY ([ItemId]) REFERENCES Item ([Id])
)
