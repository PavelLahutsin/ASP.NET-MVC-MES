using System;

namespace MES.WEB.Models
{
    public class CheckJmtForListVm : IdProvider
    {
        public string ProductName { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int? Airtight { get; set; }
    }
}