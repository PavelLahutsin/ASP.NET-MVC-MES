using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class ProductVm : IdProvider
    {
        [Display(Name = "Название:")]
        public string Name { get; set; }
    }
}