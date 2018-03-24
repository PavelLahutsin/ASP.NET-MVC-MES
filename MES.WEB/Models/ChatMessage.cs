using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.WEB.Models
{
    public class ChatMessage
    { 
        // автор сообщения, если null - автор сервер
        public ChatUser User;
        // время сообщения
        public DateTime Date = DateTime.Now;
        // текст
        public string Text = "";
    }
}