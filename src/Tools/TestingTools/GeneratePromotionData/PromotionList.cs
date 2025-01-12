using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePromotionData
{

    public class PromotionList
    {
        public List<Promotion> promotions { get; set; }
    }

    public class Promotion
    {
        public string batchId { get; set; }
        public string location { get; set; }
        public string loadWeek { get; set; }
        public string loadBatchId { get; set; }
        public string loadTimestamp { get; set; }
    }
}
