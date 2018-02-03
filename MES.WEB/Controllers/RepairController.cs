using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class RepairController : Controller
    {
        private readonly IUnitOfWork _db;

        private readonly IRepairService _service;

        public RepairController(IRepairService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

        // GET: Repair
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

        public async Task<ActionResult> AddRepairPartial()
        {
            var repairVm = new RepairVm() { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return PartialView(repairVm);
        }

        [HttpPost]
        public async Task<ActionResult> AddRepairPartial(RepairVm repairVm)
        {
            repairVm.UserId = User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return PartialView(repairVm);
            }

            var result = await _service.AddRepairAsync(Mapper.Map<RepairDto>(repairVm));
            return Json(result);
        }

        public async Task<ActionResult> ListPartial(string startDate, string endDate)
        {
            var repairs = Mapper.Map<IEnumerable<RepairDto>, List<RepairVm>>(await _service.ListRepairAsync(startDate, endDate));
            return PartialView(repairs);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteRepair(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
