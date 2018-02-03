using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DefectDetailVm : IdProvider
    {
        [Display(Name = "Название детали")]
        [Required]
        public int DetailId { get; set; }

        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int? Count { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
    }
}