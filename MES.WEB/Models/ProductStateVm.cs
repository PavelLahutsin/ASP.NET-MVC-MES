using System.ComponentModel.DataAnnotations;
using MES.DAL.Enums;

namespace MES.WEB.Models
{
    public class ProductStateVm : IdProvider
    {
       
        public int ProductId { get; set; }
        [Display(Name = "Название продукта")]
        public string ProductName { get; set; }
        public VariantStateProduct StateProduct { get; set; }
        [Required]
        [Display(Name = "Колличество")]
        [Range(0, 100000, ErrorMessage = "Min = 0, Max = 1000000")]
        public int Quantity { get; set; }
    }
}