using CFC.Controllers.Api.DataPrint;
using CFC.Controllers.FileDownload;
using CFC.Controllers.FileDownload.ExcelManagerF;
using CFC.Models.Api;
using CFC.Models.Joined.Calculate;
using System;
using System.Collections.Generic;
using System.Web.Http;
using static CFC.Controllers.FileDownload.ExcelManager;

namespace CFC.Controllers.Api
{
    public class DataPrintController : ApiController
    {
        [System.Web.Mvc.Route("api/DataPrint/DownloadExcel")]
        [HttpPost]
        public ReturnModel DownloadExcel(SaveProjectModel input)
        {
            return new ExcelManager().DownloadExcel(input);
        }
    }
}