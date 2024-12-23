--會員製造業與非製造業資料修正
--1.非製造業,其它製造業(ex.2,3,...99等)

Select *
Into User_Properties_Advance_20241223 
From User_Properties_Advance
Go

Update User_Properties_Advance
Set IndustrialTypeId = 99
Where IndustrialTypeId <> 1
Go
