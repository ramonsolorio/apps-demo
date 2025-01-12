using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp.ListPromotionsResponse
{

    public class ListPromotionsResponse
    {
        public string WMCode { get; set; }
        public string WMDesc { get; set; }
        public string maxFiles { get; set; }
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string PVMime { get; set; }
        public string PVDocName { get; set; }
        public string PVDocType { get; set; }
        public string PVSize { get; set; }
        public string PVRecords { get; set; }
        public string PVPriority { get; set; }
    }
}
