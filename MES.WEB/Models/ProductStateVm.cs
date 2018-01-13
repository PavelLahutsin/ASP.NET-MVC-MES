namespace MES.WEB.Models
{
    public class ProductStateVm : IdProvider
    {
        public int ProductId { get; set; }
        public int VariantStateProductId { get; set; }
        public int Quantity { get; set; }
    }
}