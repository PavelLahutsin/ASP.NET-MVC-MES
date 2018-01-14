using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DefectDetailDisplayVm : IdProvider
    {
        [Display(Name = "Название детали")]
        public string NameDetail { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }
        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}