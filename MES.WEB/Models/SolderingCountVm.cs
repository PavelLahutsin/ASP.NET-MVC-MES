using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class SolderingCountVm
    {
        [Display(Name = "Название продукта")]
        [Required]
        public string ProductName { get; set; }
        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int Quantity { get; set; }
    }
}