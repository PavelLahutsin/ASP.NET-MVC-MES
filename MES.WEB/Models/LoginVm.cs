using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class LoginVm
    {
        [Display(Name = "Имя")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}