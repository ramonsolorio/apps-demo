using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp.Models
{

    public class EnqueuePromotionsRequest
    {
        public string RUN_IDENTIFIER { get; set; }
        public string QUEUE_NAME { get; set; }
    }
}
