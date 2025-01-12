using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp.ReturnPromotionsResponse
{

    public class ReturnPromotionsResponse
    {
        public string WMCode { get; set; }
        public string WMDesc { get; set; }
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string PVMime { get; set; }
        public string PVDocName { get; set; }
        public string PVDocType { get; set; }
        public string PVStatus { get; set; }
        public string PVData { get; set; }
    }
}
