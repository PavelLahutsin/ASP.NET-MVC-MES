using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DisplayArrivalOfDetailVm : IdProvider
    {
        [Display(Name = "Название детали")]
        public string NameDetail { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Добавил")]
        public string UserName { get; set; }
    }
}