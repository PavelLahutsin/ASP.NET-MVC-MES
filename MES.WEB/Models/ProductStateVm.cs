using MES.DAL.Enums;

namespace MES.WEB.Models
{
    public class ProductStateVm : IdProvider
    {
        public int ProductId { get; set; }
        public VariantStateProduct StateProduct { get; set; }
        public int Quantity { get; set; }
    }
}