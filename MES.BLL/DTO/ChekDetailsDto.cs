using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MES.DAL.Enums;

namespace MES.BLL.DTO
{
   public class ChekDetailsDto
    {
        public string ProductName { get; set; }

        public StateFoTest State { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int? Airtight { get; set; }

        public int? CapM { get; set; } //КрышкаМ

        public int? CapN { get; set; } //Крышка№

        public int? Housing { get; set; } //Корпус

        public int? Tube { get; set; }

        public int? Center { get; set; }

        public int? Defect { get; set; }

        public int? Other { get; set; }

        public int? RepairCu { get; set; }

        public int? RepairNi { get; set; }

        public int? RepairCentre { get; set; }
        
        public string UserName { get; set; }
    }
}
