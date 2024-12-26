--係數設定資料

--alter TABLE xxxxxx ALTER COLUMN ooooooooo

Select * From Fuel_properties
Select * From Elec_properties
Select * From Specific_properties
Select * From Escape_properties

--調整(Escape_properties)
alter TABLE Escape_properties ALTER COLUMN [CO2] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [CH4] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [N2O] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [HFCs] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [PFCs] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [SF6] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [NF3] [decimal](15, 10) NULL
