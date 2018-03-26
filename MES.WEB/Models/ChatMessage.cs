using System;
using System.Collections.Generic;

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

    public static class ChatModel
    {
        public static List<ChatMessage> listMessage;
    }
}