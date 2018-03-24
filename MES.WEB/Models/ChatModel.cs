using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.WEB.Models
{
    public class ChatModel
    {
        // Все пользователи чата
        public List<ChatUser> Users;

        // все сообщения
        public List<ChatMessage> Messages;

        public ChatModel()
        {
            Users = new List<ChatUser>();
            Messages = new List<ChatMessage>();

            Messages.Add(new ChatMessage()
            {
                Text = "Чат запущен " + DateTime.Now
            });
        }
    }
}