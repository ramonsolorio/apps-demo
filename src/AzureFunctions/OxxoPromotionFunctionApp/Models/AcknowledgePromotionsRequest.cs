using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp.AcknowledgeStorePromotionsRequest
{
    public class AcknowledgePromotionsRequest
    {
        public string CRPlaza { get; set; }
        public string CRTienda { get; set; }
        public string source { get; set; }
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string PVDocName { get; set; }
        public string PVDocType { get; set; }
        public string PVStatus { get; set; }
        public string PVEventDate { get; set; }
    }
}
