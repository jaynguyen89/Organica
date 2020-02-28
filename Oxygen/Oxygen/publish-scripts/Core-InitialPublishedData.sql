SET IDENTITY_INSERT [dbo].[HidroTheme] ON;

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