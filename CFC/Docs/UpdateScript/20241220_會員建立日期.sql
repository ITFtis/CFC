--會員建立日期(User_Properties_Advance)

alter Table User_Properties_Advance add [BDate] [nvarchar](14) NULL
alter Table User_Properties_Advance add [BId] DateTime NULL
alter Table User_Properties_Advance add [UDate] [nvarchar](14) NULL
alter Table User_Properties_Advance add [UId] DateTime NULL
Go

Update User_Properties_Advance
Set BDate = '2024/09/01',
    BId = Id

Go
