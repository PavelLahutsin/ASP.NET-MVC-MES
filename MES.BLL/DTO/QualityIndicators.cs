using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.BLL.DTO
{
    public class QualityIndicators
    {
        public string ProducName { get; set; }
        public int Quantity { get; set; }
        public int? QuantityDefect { get; set; }
    }
}
