--�|���إߤ��(User_Properties_Advance)

alter Table User_Properties_Advance add [BDate] [nvarchar](14) NULL
alter Table User_Properties_Advance add [BId] [nvarchar](100) NULL
alter Table User_Properties_Advance add [UDate] [nvarchar](14) NULL
alter Table User_Properties_Advance add [UId] [nvarchar](100) NULL
Go

Update User_Properties_Advance
Set BDate = '2024/09/01',
    BId = Id

Go
