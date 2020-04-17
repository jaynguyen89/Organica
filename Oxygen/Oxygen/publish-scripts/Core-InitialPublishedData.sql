SET IDENTITY_INSERT [dbo].[HidroTheme] ON;

--The default theme
INSERT INTO [dbo].[HidroTheme] (
	Id, ThemeName, HidroFont, BaseFontSize, BaseFontWeight, BackgroundColor, ThemePrimaryColor, ThemeSecondaryColor,
	ThemeHoveredColor, ThemeDisabledColor, TextPrimaryColor, TextHighlightedColor, TextDisabledColor, LinkPrimaryColor,
	LinkHoveredColor, LinkDisabledColor, ColorSuccess, ColorWarning, ColorDanger, ReservedColor, BaseShadow, HoveredShadow,
	LineHeight, BaseRadius, BaseOpacity, BaseMargin, BasePadding, BorderWeight, BorderColor
) VALUES (
	1, 'Liquid Oxygen', 'KoHo', 20, 300, '#FFFFFF', '#81CDEF', '#EFA681', '#EA8F61',
	'#FCEFE8', '#0D0D0D', '#49A9D6', '#999999', '#49A9D6', '#81CDEF', '#D5EBF6', '#81EFA4',
	'#EFDD81', '#EF81A8', '#C981EF','0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24)',
	'0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24)', 25, 4, 90, '15;10;15;10', '20;15;20;15', 1, '#49A9D6'
);

SET IDENTITY_INSERT [dbo].[HidroTheme] OFF;


SET IDENTITY_INSERT [dbo].[HidroRole] ON;

--The roles
INSERT INTO [dbo].[HidroRole] (
	Id, RoleName, RoleDescription
) VALUES (1, 'Customer', 'Users with Customer role have the right to make purchases, create listings and open stores.');
INSERT INTO [dbo].[HidroRole] (
	Id, RoleName, RoleDescription
) VALUES (2, 'Supporter', 'Users with Supporter role have the right to review listings and purchases made by Customers and take appropriate actions according to their permissions.');
INSERT INTO [dbo].[HidroRole] (
	Id, RoleName, RoleDescription
) VALUES (3, 'Moderator', 'Users with Moderator role inherit the rights of Inspectors and have additional right to manage Suppoters according to their permissions.');
INSERT INTO [dbo].[HidroRole] (
	Id, RoleName, RoleDescription
) VALUES (4, 'Admintrator', 'Administrators have the right to manage Supporters and Moderators including their permissions and actions.');

SET IDENTITY_INSERT [dbo].[HidroRole] OFF;


SET IDENTITY_INSERT [dbo].[Country] ON;

--The countries
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (1, 'Australia', 'AUS', 'Oceania', 'AUD', 'Australian Dollar');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (2, 'Viet Nam', 'VNA', 'South-East Asia', 'VND', 'Vietnam Dong');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (3, 'America', 'USA', 'North America', 'USD', 'American Dollar');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (4, 'Canada', 'CAN', 'North America', 'CAD', 'Canadian Dollar');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (5, 'China', 'CH', 'East Asia', 'CNY', 'Chinese Yuan');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (6, 'Japan', 'JP', 'East Asia', 'JPY', 'Japanese Yen');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (7, 'France', 'FR', 'Western Europe', 'EUR', 'Euro');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (8, 'United Kingdom', 'GB', 'North-West Europe', 'GBP', 'Pound Sterling');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (9, 'Korea', 'KR', 'East Asia', 'KRW', 'Korean Won');
INSERT INTO [dbo].[Country] (
	Id, CountryName, CountryCode, Continent, CurrencyCode, CurrencyName
) VALUES (10, 'Nigeria', 'NG', 'West Africa', 'NGN', 'Nigerian Naira');


SET IDENTITY_INSERT [dbo].[Country] OFF;