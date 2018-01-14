using System;

namespace MES.DAL.Entities
{
    public class Assembly : IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}