using System;
using System.ComponentModel.DataAnnotations;
using MES.DAL.Enums;

namespace MES.WEB.Models
{
    public class ChekDetailsVm :IdProvider
    {
        [Display(Name = "Название продукта")]
        [Required]
        public string ProductName { get; set; }

        [Display(Name = "От куда")]
        [Required]
        public StateFoTest State { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Количество")]
        [Range(1, 100000, ErrorMessage = "Min = 1, Max = 100000")]
        [Required]
        public int? Count { get; set; }

        [Display(Name = "Целых")]
        [Range(0, 100000, ErrorMessage = "Min = 0, Max = 100000")]
        [Required]
        public int? Airtight { get; set; }

        [Display(Name = "Крышка М")]
        public int? CapM { get; set; } //КрышкаМ

        [Display(Name = "Крышка №")]
        public int? CapN { get; set; } //Крышка№

        [Display(Name = "Корпус")]
        public int? Housing { get; set; } //Корпус

        [Display(Name = "Трубка")]
        public int? Tube { get; set; }

        [Display(Name = "Центр")]
        public int? Center { get; set; }

        [Display(Name = "Брак")]
        public int? Defect { get; set; }

        [Display(Name = "Прочее")]
        public int? Other { get; set; }

        [Display(Name = "На медь")]
        public int? RepairCu { get; set; }
        [Display(Name = "На никель")]
        public int? RepairNi { get; set; }
        [Display(Name = "На центр")]
        public int? RepairCentre { get; set; }

        [Display(Name = "Добавил")]
        public string UserName { get; set; }
    }
}