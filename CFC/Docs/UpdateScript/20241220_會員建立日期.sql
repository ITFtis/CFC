--會員建立日期(User_Properties_Advance)

alter Table User_Properties_Advance add [BDate] DateTime NULL
alter Table User_Properties_Advance add [BId] [nvarchar](100) NULL
alter Table User_Properties_Advance add [UDate] DateTime NULL
alter Table User_Properties_Advance add [UId] [nvarchar](100) NULL
Go

Update User_Properties_Advance
Set BDate = '2024/09/01',
    BId = Id

Go
