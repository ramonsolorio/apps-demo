using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp.Models
{
    public class EnqueuePromotion
    {
        public string BATCH_ID { get; set; }
        public string LOAD_BATCH_ID { get; set; }
        public string LOCATION { get; set; }
        public string LOAD_WEEK { get; set; }
        public string RUN_IDENTIFIER { get; set; }
    }
}
