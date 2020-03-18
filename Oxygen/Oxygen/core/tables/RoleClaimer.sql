CREATE TABLE [dbo].[RoleClaimer] (

	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	RoleId INT NOT NULL,
	AllowCreate BIT NOT NULL DEFAULT 1,
	AllowView BIT NOT NULL DEFAULT 1,
	AllowEditOwn BIT NOT NULL DEFAULT 1,
	AllowEditOthers BIT NOT NULL DEFAULT 0,
	AllowDeleteOwn BIT NOT NULL DEFAULT 1,
	AllowDeleteOthers BIT NOT NULL DEFAULT 0,
	AllowReviveOwn BIT NOT NULL DEFAULT 1,
	AllowReviveOthers BIT NOT NULL DEFAULT 0,
	AllowTemporarily NVARCHAR(255) DEFAULT NULL,
	CONSTRAINT [PK_RoleClaimer_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_RoleClaimer_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES [dbo].[Hidrogenian] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_RoleClaimer_HidroRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[HidroRole] ([Id])
)
