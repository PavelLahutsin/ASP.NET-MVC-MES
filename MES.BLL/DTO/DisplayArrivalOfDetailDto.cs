using System;

namespace MES.BLL.DTO
{
    public class DisplayArrivalOfDetailDto:IdProvider
    {
        public string NameDetail { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}