using System;

namespace MES.DAL.Entities
{
    //Информация о пайке
    public class Soldering : IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}