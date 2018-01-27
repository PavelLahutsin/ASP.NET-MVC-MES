using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class AssemblyVm : IdProvider
    {
        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int? Quantity { get; set; }

        [Display(Name = "Название продукта")]
        public string ProductName { get; set; }

        [Display(Name = "Название продукта")]
        [Required]
        public int ProductId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Добавил")]
        public string UserName { get; set; }
    }
}