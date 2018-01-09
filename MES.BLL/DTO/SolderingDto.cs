using System;

namespace MES.BLL.DTO
{
    public class SolderingDto : IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
    }
}