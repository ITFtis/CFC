--係數設定資料

--調整(Fuel_properties)
alter TABLE Fuel_properties ALTER COLUMN [CO2] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [CH4] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [NO2] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCO2R4] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCH4R4] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GNO2R4] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCO2R5] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCH4R5] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GNO2R5] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCO2R6] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GCH4R6] [decimal](15, 10) NOT NULL
alter TABLE Fuel_properties ALTER COLUMN [GNO2R6] [decimal](15, 10) NOT NULL
Go

--調整(Elec_properties)
alter TABLE Elec_properties ALTER COLUMN [Co2e] [decimal](15, 10) NOT NULL
Go

--調整(Specific_properties)	
alter TABLE Specific_properties ALTER COLUMN [CO2] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [CH4] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [N2O] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [HFCs] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [PFCs] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [SF6] [decimal](15, 10) NULL
alter TABLE Specific_properties ALTER COLUMN [NF3] [decimal](15, 10) NULL
Go

--調整(Escape_properties)
alter TABLE Escape_properties ALTER COLUMN [CO2] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [CH4] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [N2O] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [HFCs] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [PFCs] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [SF6] [decimal](15, 10) NULL
alter TABLE Escape_properties ALTER COLUMN [NF3] [decimal](15, 10) NULL
Go

