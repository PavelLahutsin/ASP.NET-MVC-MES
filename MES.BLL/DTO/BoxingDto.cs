using System;
using MES.DAL.Enums;

namespace MES.BLL.DTO
{
    public class BoxingDto : IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public BoxingVariant BoxingVariant { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
    }
}
