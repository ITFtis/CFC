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
