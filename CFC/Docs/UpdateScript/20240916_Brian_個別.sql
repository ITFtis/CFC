--Brian
--1.新增資料表(Cals_type)
CREATE TABLE [dbo].[Cals_type](
	[Id] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Cals_type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FA', N'類別三-運輸', 1)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FB', N'類別四-組織使用產品', 2)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FC', N'類別五-使用組織產品', 3)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FD', N'類別六-其他排放', 4)
GO

--2.新增資料表(Cals_properties)
CREATE TABLE [dbo].[Cals_properties](
	[Id] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](20) NULL,
	[Unit] [nvarchar](16) NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Cals_properties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA1', N'上游原物料配送當量', N'FA', N'公噸CO2e/年', 1)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA2', N'商務旅遊', N'FA', N'公噸CO2e/年', 2)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA3', N'員工通勤', N'FA', N'公噸CO2e/年', 3)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA4', N'下游運輸及配送', N'FA', N'公噸CO2e/年', 4)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB1', N'採購', N'FB', N'公噸CO2e/年', 5)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB2', N'資本', N'FB', N'公噸CO2e/年', 6)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB3', N'能源相關活動', N'FB', N'公噸CO2e/年', 7)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB4', N'營運廢棄物', N'FB', N'公噸CO2e/年', 8)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB5', N'上游資產租賃', N'FB', N'公噸CO2e/年', 9)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC1', N'加工', N'FC', N'公噸CO2e/年', 10)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC2', N'使用', N'FC', N'公噸CO2e/年', 11)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC3', N'報廢', N'FC', N'公噸CO2e/年', 12)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC4', N'下游租賃', N'FC', N'公噸CO2e/年', 13)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC5', N'加盟', N'FC', N'公噸CO2e/年', 14)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC6', N'投資', N'FC', N'公噸CO2e/年', 15)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FD1', N'其他排放', N'FD', N'公噸CO2e/年', 16)
GO

--3.圖文, 新增資料表(Sys_content, Sys_contentDetail) + 資料
CREATE TABLE [dbo].[Sys_content](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Sys_content] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Sys_contentDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_Sys_contentDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Sys_content] ON 

INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (2, N'A1', N'登入貼心提醒')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (3, N'A2', N'燃料計算')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (22, N'A3', N'冷媒逸散計算')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (23, N'A4', N'其他逸散')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (24, N'A5', N'特殊製程計算')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (25, N'A6', N'電力計算')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (26, N'A7', N'蒸氣計算')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (27, N'A8', N'其他類別')
SET IDENTITY_INSERT [dbo].[Sys_content] OFF
GO
SET IDENTITY_INSERT [dbo].[Sys_contentDetail] ON 

INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (72, 2, NULL, N'請注意輸入的用量單位(燃料、電力、冷媒用量)。 本計算工具僅供自行檢查溫室氣體排放量。如需通過排放查證和盤查登錄要求，須依照 ISO 相關規範和環保署的作業指引。 本計算工具所獲得的相關資料(一般或技術、商業資料) ，負有保密責任。 本計算工具所提供的相關技術資訊(含產品、技術或服務) ，在未經正式授權下，不得任意擴散、複製、抄襲、引用。 本計算工具所使用相關排放係數、GWP值、熱值與逸散率因子皆是引用IPCC 2006年數據、AR4報告與環保署公告之溫室氣體排放係數管理表6.0.4版。')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (73, 3, N'實際用量來源可參考設備操作日報、月報、年報。', N'(可向廠務相關部門取得)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (74, 3, N'亦可用採購金額回推用量(無法取得實際用量時候)。', N'(可向採購相關部門取得)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (77, 22, N'此處為冷媒設備的原始填充量，可於該設備之銘牌取得。', N'(可向廠務相關部門取得)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (78, 22, N'冰箱、冰水機、冷氣等造成冷媒逸散的設備皆須納入', NULL)
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (79, 25, N'用電量可由台電各月份電費單取得，加總年度用電量。', N'(可向廠務相關部門取得)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (80, 25, N'僅供外購台電電力部分計算，係數參照台電最新公告。', N'(非台電電網外購電力需參照奇供應商之排放係數)')
SET IDENTITY_INSERT [dbo].[Sys_contentDetail] OFF
GO
