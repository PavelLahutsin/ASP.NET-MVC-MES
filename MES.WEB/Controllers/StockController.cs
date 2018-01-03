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
        private readonly IStockService _stockOn;
        private readonly IUnitOfWork _db;

        public StockController(IStockService stockOn, IUnitOfWork db)
        {
            _stockOn = stockOn;
            _db = db;
        }


        // Остаток деталей на складе
        public async Task<ActionResult> StockBalanceJmt()
        {
            var d52001 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetail("5200-01"));

            var d6500 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetail("6500"));
            var detail = Mapper.Map<IEnumerable<Detail>, List<DetailVm>>(await _db.Details.GetAllAsync());

            ViewBag.d52001 = d52001.Select(t => t.Name);
            ViewBag.d52001Count = d52001.Select(t => t.Quantityq);

            ViewBag.d6500 = d6500.Select(t => t.Name);
            ViewBag.d6500Count = d6500.Select(t => t.Quantityq);



            return View(detail);
        }

        //прихон на склад 
        public ActionResult AddJmtDetail()
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            ArrivalOfDetailVm arrivalOfDetail = new ArrivalOfDetailVm {Date = DateTime.Now};
            return View(arrivalOfDetail);
        }

        [HttpPost]
        public async Task<ActionResult> AddJmtDetail(ArrivalOfDetailVm arrival)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return View(arrival);

            var result = await _stockOn.AddArrivalOfDetailAsync(Mapper.Map<ArrivalOfDetailDto>(arrival));
            return RedirectToAction("AddJmtDetail");
        }

        //Редактирование деталей
        public async Task<ActionResult> EditJmtDetail(int id)
        {
            var detail = Mapper.Map<DetailVm>(await _db.Details.GetAsync(id));
            return PartialView(detail);
        }
        [HttpPost]
        public async Task<ActionResult> EditJmtDetail(DetailVm detail)
        {
            if (!ModelState.IsValid) return PartialView(detail);
            
            _db.Details.Update(Mapper.Map<Detail>(detail));
            await _db.Commit();
            return RedirectToAction("StockBalanceJmt");
        }

        //История прихода на склад
        public async Task<ActionResult> HistArrival()
        {
            var arrivals = Mapper.Map<IEnumerable<DisplayArrivalOfDetailDto>, List<DisplayArrivalOfDetailVm>>(await _stockOn.ShowArryvalOfDedails());
            return View(arrivals);
        }

        //Удаление Данных о добавлении на склад
        public async Task<ActionResult> DeleteArrival(int id)
        {
            var result = await _stockOn.DeleteArrivalOfDetail(id);
            return RedirectToAction("HistArrival");
        }

        //Редактирование Данных о добавлении на склад
        public async Task<ActionResult> EditArrival(int id)
        {
            var arrival = Mapper.Map<ArrivalOfDetailVm>(await _db.ArrivalOfDetails.GetAsync(id));

            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            return View(arrival);
        }

        [HttpPost]
        public async Task<ActionResult> EditArrival(ArrivalOfDetailVm arrival)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return View(arrival);

            var result = await _stockOn.EditArrivalOfDetail(Mapper.Map<ArrivalOfDetail>(arrival));

            return RedirectToAction("HistArrival");
        }





    }
}