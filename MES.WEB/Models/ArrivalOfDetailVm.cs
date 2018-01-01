using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class ArrivalOfDetailVm
    {
        [Display(Name = "Дата")]
        [Required]
        public int DetailId { get; set; }

        [Display(Name = "Количество")]
        [Required]
        public int Count { get; set; }

        
        [Required]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
    }
}