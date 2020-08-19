
CREATE TABLE [dbo].[IMP_SAP_Laender](
	[Code] [varchar](50) NOT NULL,
	[Land] [varchar](250) NULL,
	[Kontinent] [varchar](250) NULL,
	[DACH] [int] NULL,
	[ACH] [int] NULL,
	[Filter_Report] [varchar](250) NULL,
 CONSTRAINT [PK_IMP_SAP_Laender] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AC', N'Ascension', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AD', N'Andorra', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AE', N'Vereinigte Arabische Emirate', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AF', N'Afghanistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AG', N'Antigua und Barbuda', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AI', N'Anguilla', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AL', N'Albanien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AM', N'Armenien', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AN', N'Niederländische Antillen', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AO', N'Angola', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AQ', N'Antarktis', N'Antarktis', 0, 0, N'Antarktis')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AR', N'Argentinien', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AS', N'Amerikanisch-Samoa', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AT', N'Österreich', N'Europa', 1, 1, N'AT / CH')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AU', N'Australien', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AW', N'Aruba', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AX', N'Aland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'AZ', N'Aserbaidschan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BA', N'Bosnien und Herzegowina', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BB', N'Barbados', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BD', N'Bangladesch', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BE', N'Belgien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BF', N'Burkina Faso', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BG', N'Bulgarien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BH', N'Bahrain', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BI', N'Burundi', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BJ', N'Benin', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BM', N'Bermuda', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BN', N'Brunei', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BO', N'Bolivien', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BR', N'Brasilien', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BS', N'Bahamas', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BT', N'Bhutan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BV', N'Bouvetinsel', N'Antarktis', 0, 0, N'Antarktis')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BW', N'Botswana', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BY', N'Weißrussland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'BZ', N'Belize', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CA', N'Kanada', N'North America', 0, 0, N'USA, Kanada, Mexico')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CC', N'Kokosinseln', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CD', N'Kongo, Demokratische Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CF', N'Zentralafrikanische Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CG', N'Kongo, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CH', N'Schweiz', N'Europa', 1, 1, N'AT / CH')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CI', N'Cote d''Ivoire', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CK', N'Cookinseln', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CL', N'Chile', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CM', N'Kamerun', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CN', N'China, Volksrepublik', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CO', N'Kolumbien', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CR', N'Costa Rica', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CS', N'Serbien und Montenegro', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CU', N'Kuba', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CV', N'Kap Verde, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CX', N'Weihnachtsinsel', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CY', N'Zypern, Republik', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'CZ', N'Tschechische Republik', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DE', N'Deutschland', N'Europa', 1, 0, N'Deutschland')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DG', N'Diego Garcia', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DJ', N'Dschibuti', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DK', N'Dänemark', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DM', N'Dominica', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DO', N'Dominikanische Republik', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'DZ', N'Algerien', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'EC', N'Ecuador', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'EE', N'Estland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'EG', N'Ägypten', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'EH', N'Westsahara', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ER', N'Eritrea', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ES', N'Spanien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ET', N'Äthiopien', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'EU', N'Europäische Union', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FI', N'Finnland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FJ', N'Fidschi', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FK', N'Falklandinseln', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FM', N'Mikronesien, Föderierte Staaten von', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FO', N'Färöer', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'FR', N'Frankreich', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GA', N'Gabun', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GB', N'Vereinigtes Königreich von Großbritannien und Nordirland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GD', N'Grenada', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GE', N'Georgien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GF', N'Französisch-Guayana', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GG', N'Guernsey, Vogtei', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GH', N'Ghana, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GI', N'Gibraltar', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GL', N'Grönland', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GM', N'Gambia', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GN', N'Guinea, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GP', N'Guadeloupe', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GQ', N'Äquatorialguinea, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GR', N'Griechenland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GS', N'Südgeorgien und die Südlichen Sandwichinseln', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GT', N'Guatemala', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GU', N'Guam', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GW', N'Guinea-Bissau, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'GY', N'Guyana', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HK', N'Hongkong', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HM', N'Heard und McDonaldinseln', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HN', N'Honduras', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HR', N'Kroatien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HT', N'Haiti', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'HU', N'Ungarn', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IC', N'Kanarische Inseln', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ID', N'Indonesien', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IE', N'Irland, Republik', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IL', N'Israel', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IM', N'Insel Man', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IN', N'Indien', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IO', N'Britisches Territorium im Indischen Ozean', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IQ', N'Irak', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IR', N'Iran', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IS', N'Island', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'IT', N'Italien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'JE', N'Jersey', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'JM', N'Jamaika', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'JO', N'Jordanien', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'JP', N'Japan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KE', N'Kenia', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KG', N'Kirgisistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KH', N'Kambodscha', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KI', N'Kiribati', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KM', N'Komoren', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KN', N'St. Kitts und Nevis', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KP', N'Korea, Demokratische Volkrepublik', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KR', N'Korea, Republik', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KW', N'Kuwait', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KY', N'Kaimaninseln', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'KZ', N'Kasachstan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LA', N'Laos', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LB', N'Libanon', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LC', N'St. Lucia', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LI', N'Liechtenstein, Fürstentum', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LK', N'Sri Lanka', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LR', N'Liberia, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LS', N'Lesotho', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LT', N'Litauen', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LU', N'Luxemburg', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LV', N'Lettland', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'LY', N'Libyen', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MA', N'Marokko', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MC', N'Monaco', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MD', N'Moldawien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ME', N'Montenegro', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MG', N'Madagaskar, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MH', N'Marshallinseln', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MK', N'Mazedonien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ML', N'Mali, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MM', N'Myanmar', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MN', N'Mongolei', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MO', N'Macao', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MP', N'Nördliche Marianen', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MQ', N'Martinique', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MR', N'Mauretanien', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MS', N'Montserrat', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MT', N'Malta', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MU', N'Mauritius, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MV', N'Malediven', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MW', N'Malawi, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MX', N'Mexiko', N'North America', 0, 0, N'USA, Kanada, Mexico')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MY', N'Malaysia', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'MZ', N'Mosambik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NA', N'Namibia, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NC', N'Neukaledonien', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NE', N'Niger', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NF', N'Norfolkinsel', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NG', N'Nigeria', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NI', N'Nicaragua', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NL', N'Niederlande', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NO', N'Norwegen', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NP', N'Nepal', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NR', N'Nauru', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NT', N'Neutrale Zone (Irak)', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NU', N'Niue', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'NZ', N'Neuseeland', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'OM', N'Oman', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PA', N'Panama', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PE', N'Peru', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PF', N'Französisch-Polynesien', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PG', N'Papua-Neuguinea', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PH', N'Philippinen', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PK', N'Pakistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PL', N'Polen', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PM', N'St. Pierre und Miquelon', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PN', N'Pitcairninseln', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PR', N'Puerto Rico', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PS', N'Palästinensische Autonomiegebiete', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PT', N'Portugal', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PW', N'Palau', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'PY', N'Paraguay', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'QA', N'Katar', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'RE', N'Réunion', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'RO', N'Rumänien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'RS', N'Serbien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'RU', N'Russische Föderation', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'RW', N'Ruanda, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SA', N'Saudi-Arabien, Königreich', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SB', N'Salomonen', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SC', N'Seychellen, Republik der', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SD', N'Sudan', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SE', N'Schweden', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SG', N'Singapur', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SH', N'Die Kronkolonie St. Helena und Nebengebiete', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SI', N'Slowenien', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SJ', N'Svalbard und Jan Mayen', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SK', N'Slowakei', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SL', N'Sierra Leone, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SM', N'San Marino', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SN', N'Senegal', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SO', N'Somalia, Demokratische Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SR', N'Suriname', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ST', N'São Tomé und Príncipe', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SU', N'Union der Sozialistischen Sowjetrepubliken', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SV', N'El Salvador', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SY', N'Syrien', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'SZ', N'Swasiland', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TA', N'Tristan da Cunha', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TC', N'Turks- und Caicosinseln', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TD', N'Tschad, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TF', N'Französische Süd- und Antarktisgebiete', N'Antarktis', 0, 0, N'Antarktis')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TG', N'Togo, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TH', N'Thailand', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TJ', N'Tadschikistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TK', N'Tokelau', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TL', N'Timor-Leste, Demokratische Republik', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TM', N'Turkmenistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TN', N'Tunesien', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TO', N'Tonga', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TR', N'Türkei', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TT', N'Trinidad und Tobago', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TV', N'Tuvalu', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TW', N'Taiwan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'TZ', N'Tansania, Vereinigte Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'UA', N'Ukraine', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'UG', N'Uganda, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'US', N'Vereinigte Staaten von Amerika', N'North America', 0, 0, N'USA, Kanada, Mexico')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'UY', N'Uruguay', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'UZ', N'Usbekistan', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VA', N'Vatikanstadt', N'Europa', 0, 0, N'Europa')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VC', N'St. Vincent und die Grenadinen (GB)', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VE', N'Venezuela', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VG', N'Britische Jungferninseln', N'North America', 0, 0, N'North America')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VI', N'Amerikanische Jungferninseln', N'Südamerika', 0, 0, N'Südamerika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VN', N'Vietnam', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'VU', N'Vanuatu', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'WF', N'Wallis und Futuna', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'WS', N'Samoa', N'Australien', 0, 0, N'Australien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'YE', N'Jemen', N'Asien', 0, 0, N'Asien')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'YT', N'Mayotte', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ZA', N'Südafrika, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ZM', N'Sambia, Republik', N'Afrika', 0, 0, N'Afrika')
GO
INSERT [dbo].[IMP_SAP_Laender] ([Code], [Land], [Kontinent], [DACH], [ACH], [Filter_Report]) VALUES (N'ZW', N'Simbabwe, Republik', N'Afrika', 0, 0, N'Afrika')
GO
