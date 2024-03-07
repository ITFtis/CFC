using CFC.Models.Prj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CFC.Controllers.FileDownload.ExcelManagerF
{
    public class FuelInput {
        public Fuel_properties property {get;set;}
        public Fuel_volume volume { get;set;}
    }

    public class RefrigInput
    {
        public Refrigerant_volume volume { get; set; }
        public Refrigerant_type property { get; set; }
        public Refrigerant_equip equip { get; set; }
    }

    public class EscapeInput { 
        public Escape_properties property { get; set; }
        public Escape_volume volume { get; set; }
    }

    public class CreateInput { 
        public Specific_volume volume { get; set; }
        public Specific_properties property { get; set; }
    }


}