using CFC.Models.Prj;
using System.Collections.Generic;

namespace CFC.Models.Api
{
    public class SaveProjectModel : ProjectProperties
    {
        public int RowID { get; set; }
        public string ProjectName { get; set; }
    }
}