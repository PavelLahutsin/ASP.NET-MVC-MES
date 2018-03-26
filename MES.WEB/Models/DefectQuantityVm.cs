using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DefectQuantityVm
    {
        [Display(Name = "Название детали")]
        public string NameDetail { get; set; }
        [Display(Name = "Количество")]
        public int Count { get; set; }
    }
}