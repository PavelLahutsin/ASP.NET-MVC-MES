using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DetailInProductVm 
    {
        [Display(Name = "Название детали")]
        [Required]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Название детали")]
        public string Name { get; set; }
        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int Quantity { get; set; }
    }
}