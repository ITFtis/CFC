--Brian
--1.�s�W��ƪ�(Cals_type)
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

INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FA', N'���O�T-�B��', 1)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FB', N'���O�|-��´�ϥβ��~', 2)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FC', N'���O��-�ϥβ�´���~', 3)
INSERT [dbo].[Cals_type] ([Id], [Name], [DisplayOrder]) VALUES (N'FD', N'���O��-��L�Ʃ�', 4)
GO

--2.�s�W��ƪ�(Cals_properties)
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

INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA1', N'�W��쪫�ưt�e��q', N'FA', N'����CO2e/�~', 1)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA2', N'�ӰȮȹC', N'FA', N'����CO2e/�~', 2)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA3', N'���u�q��', N'FA', N'����CO2e/�~', 3)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FA4', N'�U��B��ΰt�e', N'FA', N'����CO2e/�~', 4)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB1', N'����', N'FB', N'����CO2e/�~', 5)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB2', N'�ꥻ', N'FB', N'����CO2e/�~', 6)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB3', N'�෽��������', N'FB', N'����CO2e/�~', 7)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB4', N'��B�o��', N'FB', N'����CO2e/�~', 8)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FB5', N'�W��겣����', N'FB', N'����CO2e/�~', 9)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC1', N'�[�u', N'FC', N'����CO2e/�~', 10)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC2', N'�ϥ�', N'FC', N'����CO2e/�~', 11)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC3', N'���o', N'FC', N'����CO2e/�~', 12)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC4', N'�U�寲��', N'FC', N'����CO2e/�~', 13)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC5', N'�[��', N'FC', N'����CO2e/�~', 14)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FC6', N'���', N'FC', N'����CO2e/�~', 15)
INSERT [dbo].[Cals_properties] ([Id], [Name], [Type], [Unit], [DisplayOrder]) VALUES (N'FD1', N'��L�Ʃ�', N'FD', N'����CO2e/�~', 16)
GO

--3.�Ϥ�, �s�W��ƪ�(Sys_content, Sys_contentDetail) + ���
