CREATE TABLE [dbo].[HidroSetting] (

	Id INT IDENTITY(1,1) NOT NULL,
	HidrogenianId INT NOT NULL,
	HidroThemeId TINYINT NOT NULL DEFAULT 1,
	BirthDisplayRule TINYINT NOT NULL DEFAULT 0,
	TwoFaMethod TINYINT NOT NULL DEFAULT 0, --1 for email code, 2 for sms code, 3 for app code
	CONSTRAINT [PK_HidroSetting_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_HidroSetting_Hidrogenian] FOREIGN KEY ([HidrogenianId]) REFERENCES [dbo].[Hidrogenian] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_HidroSetting_HidroTheme] FOREIGN KEY ([HidroThemeId]) REFERENCES [dbo].[HidroTheme] ([Id]) ON DELETE SET DEFAULT
)