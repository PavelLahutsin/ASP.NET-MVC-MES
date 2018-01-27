using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize]
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


        /// <summary>
        /// Добавить детали в брак
        /// </summary>
        /// <returns>PartialView</returns>
        public ActionResult AddDefectJmtDetailPartial()
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            DefectDetailVm defect = new DefectDetailVm { Date = DateTime.Now };
            return PartialView(defect);
        }

        [HttpPost]
        public async Task<ActionResult> AddDefectJmtDetailPartial(DefectDetailVm defect)
        {
            defect.UserId = User.Identity.GetUserId<int>();
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return PartialView(defect);

            var result = await _stockOn.AddDefectDetailAsync(Mapper.Map<DefectDetailDto>(defect));
            return Json(result);
        }


        //История брака на склад
        public async Task<ActionResult> HistDefectPartial()
        {
            var defect = Mapper.Map<IEnumerable<DefectDetailDisplayDto>, List<DefectDetailDisplayVm>>(await _stockOn.ShowDefectDetailAsync());
            return PartialView(defect);
        }

        public ActionResult Defect()
        {
            var now = DateTime.Now;
            var date = new DateVm
            {
                StartDate = new DateTime(now.Year, now.Month, 1),
                EndDate = now
            };
            return View(date);
        }


        //Удаление Данных о браковке деталей
        public async Task<ActionResult> DeleteDefect(int id)
        {
            var result = await _stockOn.DeleteDefectDetailAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Редактирование Данных о браковке деталей
        public async Task<ActionResult> EditDefectPartial(int id)
        {
            var defect = Mapper.Map<DefectDetailVm>(await _db.DefectDetails.GetAsync(id));

            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            return PartialView(defect);
        }

        [HttpPost]
        public async Task<ActionResult> EditDefectPartial(DefectDetailVm defect)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stockOn.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return View(defect);

            var result = await _stockOn.EditDefectDetailAsync(Mapper.Map<DefectDetail>(defect));

            return Json(result);
        }

    }
}