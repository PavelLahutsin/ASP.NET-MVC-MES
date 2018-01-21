using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES.BLL.Interfaces;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class BoxingController : Controller
    {
        private readonly IAssemblyService _service;
        private readonly IUnitOfWork _db;

        public BoxingController(IAssemblyService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

        // GET: Boxing
        public ActionResult Index()
        {
            var now = DateTime.Now;
            var date = new DateVm
            {
                StartDate = new DateTime(now.Year, now.Month, 1),
                EndDate = now
            };
            return View(date);
        }
    }
}