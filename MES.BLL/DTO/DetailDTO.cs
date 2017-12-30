namespace MES.BLL.DTO
{
    public class DetailDTO : IdProvider
    {
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public int Quantity { get; set; }
        public int GroupProductId { get; set; }
    }
}