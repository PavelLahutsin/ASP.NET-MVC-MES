using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class ChatUser:IdProvider
    {
        [Display(Name = "Имя")]
        public string UserName { get; set; }       
       
    }
}