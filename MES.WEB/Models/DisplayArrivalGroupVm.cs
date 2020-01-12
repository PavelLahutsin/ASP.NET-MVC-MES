using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DisplayArrivalGroupVm : IdProvider
    {
        public DisplayArrivalGroupVm()
        {
            
        }

        public DisplayArrivalGroupVm(string nameDetail, int count)
        {
            NameDetail = nameDetail;
            Count = count;
        }

        [Display(Name = "Название детали")]
        public string NameDetail { get; set; }

        [Display(Name = "Количество")]
        public int Count { get; set; }
    }
}