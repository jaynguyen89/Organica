CREATE TABLE [dbo].[StaffRole] (
	Id INT IDENTITY(1,1) NOT NULL,
	StaffId INT NOT NULL,
	RoleId INT NOT NULL,
	WorkDescription NVARCHAR(500) DEFAULT NULL,
	CONSTRAINT [PK_StaffRole_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_StaffRole_StoreRole] FOREIGN KEY ([RoleId]) REFERENCES StoreRole ([Id]),
	CONSTRAINT [FK_StaffRole_StoreStaff] FOREIGN KEY ([StaffId]) REFERENCES StoreStaff ([Id])
)
