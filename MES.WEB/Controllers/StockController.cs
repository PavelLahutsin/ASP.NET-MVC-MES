using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class StockController : Controller
    {
        private readonly IDetailService _detailOn;
        private readonly IUnitOfWork _db;

        public StockController(IDetailService detailOn, IUnitOfWork db)
        {
            _detailOn = detailOn;
            _db = db;
        }


        // Остаток деталей на складе
        public async Task<ActionResult> StockBalanceOw()
        {
            var d52001 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_detailOn.GetDetail("5200-01"));

            var d6500 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_detailOn.GetDetail("6500"));
            var detail = Mapper.Map<IEnumerable<Detail>, List<DetailVm>>(await _db.Details.GetAllAsync());

            ViewBag.d52001 = d52001.Select(t => t.Name);
            ViewBag.d52001Count = d52001.Select(t => t.Quantity);

            ViewBag.d6500 = d6500.Select(t => t.Name);
            ViewBag.d6500Count = d6500.Select(t => t.Quantity);



            return View(detail);
        }

        //прихон на склад
        public ActionResult AddDetail()
        {
            var details = _detailOn.GetDetailsJmt();

            ViewBag.nameDetails = details.Select(t=>t.Name);
            ViewBag.Id = details.Select(t => t.Id);
            ViewBag.Count = details.Count;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddDetail(ArrivalOfDetailVm arrival)
        {
            if (!ModelState.IsValid) return View(arrival);

            await _detailOn.AddArrivalOfDetailAsync(Mapper.Map<ArrivalOfDetailDto>(arrival));
            return RedirectToAction("AddDetail");
        }
    }
}