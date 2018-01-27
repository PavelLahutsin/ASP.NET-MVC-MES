using System.ComponentModel.DataAnnotations;

namespace MES.BLL.DTO
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public int RoleId { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Image { get; set; } // данные изображения

        [ScaffoldColumn(false)]
        public string MimeType { get; set; } // Mime - тип данных изображения
    }
}