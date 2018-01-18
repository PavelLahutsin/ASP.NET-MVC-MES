using MES.DAL.Enums;

namespace MES.BLL.DTO
{
    public class ProductStateDto : IdProvider
    {
        public int ProductId { get; set; }
        public VariantStateProduct StateProduct { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}