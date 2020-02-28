CREATE TABLE [dbo].[RoleClaimer] (

	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	RoleId INT NOT NULL,
	CONSTRAINT [PK_RoleClaimer_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_RoleClaimer_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES [dbo].[Hidrogenian] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_RoleClaimer_HidroRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[HidroRole] ([Id])
)
