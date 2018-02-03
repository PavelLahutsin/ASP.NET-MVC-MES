using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class CheckJmtForListVm : IdProvider
    {
        [Display(Name = "Название продукта")]
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int Count { get; set; }
        [Display(Name = "Целых")]
        [Range(0, 100000, ErrorMessage = "Min = 0, Max = 100000")]
        [Required]
        public int? Airtight { get; set; }
    }
}