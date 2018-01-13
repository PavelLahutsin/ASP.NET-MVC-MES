namespace MES.BLL.DTO
{
    public class ProductStateDto : IdProvider
    {
        public int ProductId { get; set; }
        public int VariantStateProductId { get; set; }
        public int Quantity { get; set; }
    }
}