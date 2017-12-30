using System;

namespace MES.BLL.DTO
{
    public class ArrivalOfDetailDTO : IdProvider
    {
        public int DetailId { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        
    }
}