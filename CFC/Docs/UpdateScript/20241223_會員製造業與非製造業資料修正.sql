--�|���s�y�~�P�D�s�y�~��ƭץ�
--1.�D�s�y�~,�䥦�s�y�~(ex.2,3,...99��)

Select *
Into User_Properties_Advance_20241223 
From User_Properties_Advance
Go

Update User_Properties_Advance
Set IndustrialTypeId = 99
Where IndustrialTypeId <> 1
Go
