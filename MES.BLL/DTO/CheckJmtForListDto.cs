using System;

namespace MES.BLL.DTO
{
    public class CheckJmtForListDto : IdProvider
    {
        public string ProductName { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int? Airtight { get; set; }
    }
}