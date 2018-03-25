using AutoMapper;
using MES.BLL.Infrastructure;
using MES.DAL.Interfaces;
using MES.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.WEB.Controllers
{
    public class ChatController : Controller
    {
        IUnitOfWork _unitOfWork;

        public ChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public static List<ChatMessage> listMessage;

        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Chat()
        {
            return PartialView("_Chat");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Messege(string message)
        {
            if (listMessage == null) listMessage = new List<ChatMessage>();

            //оставляем только последние 90 сообщений
            if (listMessage.Count > 15)
                listMessage.RemoveRange(0, 10);

            if (!Request.IsAjaxRequest())
            {
                return PartialView("_Messege", listMessage);
            }

            if(!string.IsNullOrEmpty(message))
            {
                var userId = User.Identity.GetUserId<int>();
                ChatUser user = Mapper.Map<ChatUser>(await _unitOfWork.Users.GetAsync(userId));
                ChatMessage chatMessage = new ChatMessage { User = user, Text = message, Date = DateTime.Now };
                listMessage.Add(chatMessage);
            }
            return PartialView("_Messege", listMessage);
        }
    }
}