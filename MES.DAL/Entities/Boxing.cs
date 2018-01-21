using System;
using MES.DAL.Enums;

namespace MES.DAL.Entities
{
    public class Boxing:IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public BoxingVariant BoxingVariant { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}