using System.Web.Mvc;

namespace MES.BLL.DTO
{
    public class DetailDTO : IdProvider
    {
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public int Quantityq { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int GroupProductId { get; set; }
    }
}