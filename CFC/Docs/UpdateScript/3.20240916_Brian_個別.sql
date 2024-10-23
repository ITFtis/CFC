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

INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (2, N'A1', N'�n�J�K�ߴ���')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (3, N'A2', N'�U�ƭp��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (22, N'A3', N'�N�C�h���p��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (23, N'A4', N'��L�h��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (24, N'A5', N'�S��s�{�p��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (25, N'A6', N'�q�O�p��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (26, N'A7', N'�]��p��')
INSERT [dbo].[Sys_content] ([Id], [Code], [Name]) VALUES (27, N'A8', N'��L���O')
SET IDENTITY_INSERT [dbo].[Sys_content] OFF
GO
SET IDENTITY_INSERT [dbo].[Sys_contentDetail] ON 

INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (72, 2, NULL, N'�Ъ`�N��J���ζq���(�U�ơB�q�O�B�N�C�ζq)�C ���p��u��ȨѦۦ��ˬd�ūǮ���Ʃ�q�C�p�ݳq�L�Ʃ�d�ҩM�L�d�n���n�D�A���̷� ISO �����W�d�M���O�p���@�~���ޡC ���p��u�����o���������(�@��Χ޳N�B�ӷ~���) �A�t���O�K�d���C ���p��u��Ҵ��Ѫ������޳N��T(�t���~�B�޳N�ΪA��) �A�b���g�������v�U�A���o���N�X���B�ƻs�B��ŧ�B�ޥΡC ���p��u��Ҩϥά����Ʃ�Y�ơBGWP�ȡB���ȻP�h���v�]�l�ҬO�ޥ�IPCC 2006�~�ƾڡBAR4���i�P���O�p���i���ūǮ���Ʃ�Y�ƺ޲z��6.0.4���C')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (73, 3, N'��ڥζq�ӷ��i�Ѧҳ]�ƾާ@����B����B�~���C', N'(�i�V�t�Ȭ����������o)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (74, 3, N'��i�α��ʪ��B�^���ζq(�L�k���o��ڥζq�ɭ�)�C', N'(�i�V���ʬ����������o)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (77, 22, N'���B���N�C�]�ƪ���l��R�q�A�i��ӳ]�Ƥ��ʵP���o�C', N'(�i�V�t�Ȭ����������o)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (78, 22, N'�B�c�B�B�����B�N�𵥳y���N�C�h�����]�ƬҶ��ǤJ', NULL)
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (79, 25, N'�ιq�q�i�ѥx�q�U����q�O����o�A�[�`�~�ץιq�q�C', N'(�i�V�t�Ȭ����������o)')
INSERT [dbo].[Sys_contentDetail] ([Id], [ContentId], [Title], [Note]) VALUES (80, 25, N'�Ȩѥ~�ʥx�q�q�O�����p��A�Y�ưѷӥx�q�̷s���i�C', N'(�D�x�q�q���~�ʹq�O�ݰѷө_�����Ӥ��Ʃ�Y��)')
SET IDENTITY_INSERT [dbo].[Sys_contentDetail] OFF
GO
