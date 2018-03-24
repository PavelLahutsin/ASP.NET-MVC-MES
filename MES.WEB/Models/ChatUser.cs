namespace MES.WEB.Models
{
    public class ChatUser:IdProvider
    {
       
        public string UserName { get; set; }
        
        public byte[] Image { get; set; } // данные изображения
        
        public string MimeType { get; set; } // Mime - тип данных изображения
    }
}