using System;

namespace MES.BLL.DTO
{
    public class ArrivalOfDetailDto : IdProvider
    {
        public int DetailId { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}