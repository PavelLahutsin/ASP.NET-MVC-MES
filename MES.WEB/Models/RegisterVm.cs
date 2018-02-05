using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class RegisterVm 
    {
        [Display(Name = "Имя")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Повторить пароль")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Image { get; set; } // данные изображения

        [ScaffoldColumn(false)]
        public string MimeType { get; set; } // Mime - тип данных изображения

    }
}