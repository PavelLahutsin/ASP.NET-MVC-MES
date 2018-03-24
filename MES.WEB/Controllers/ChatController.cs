using MES.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.WEB.Controllers
{
    public class ChatController : Controller
    {

        static ChatModel chatModel;

        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Chat()
        {
            return PartialView("_Chat");
        }
    }
}