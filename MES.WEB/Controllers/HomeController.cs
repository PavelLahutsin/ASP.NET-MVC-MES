using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockService _stockOn;

        public HomeController(IStockService stockOn)
        {
            _stockOn = stockOn;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}