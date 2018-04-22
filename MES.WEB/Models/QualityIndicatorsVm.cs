using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.WEB.Models
{
    public class QualityIndicatorsVm
    {
        public string ProducName { get; set; }
        public int Quantity { get; set; }
        public int? QuantityDefect { get; set; }
    }
}