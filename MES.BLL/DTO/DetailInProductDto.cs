namespace MES.BLL.DTO
{
    public class DetailInProductDto : IdProvider
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}