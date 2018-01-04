using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES.WEB.Models
{
    public class DetailVm : IdProvider
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        public string VendorCode { get; set; }

        [Display(Name = "Количество")]
        [Range(0, 100000, ErrorMessage = "Min = 0, Max = 1000000")]
        public int Quantityq { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int GroupProductId { get; set; }

    }
}