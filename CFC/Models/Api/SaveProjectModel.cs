using CFC.Models.Prj;
using System.Collections.Generic;
using System.Web;

namespace CFC.Models.Api
{
    public class SaveProjectModel : ProjectProperties
    {
        public int RowID { get; set; }
        public string ProjectName { get; set; }

        public string FactoryRegistration { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string BDate { get; set; }

        public string BId { get; set; }

        public string ProjectMemo { get; set; }



    }
}