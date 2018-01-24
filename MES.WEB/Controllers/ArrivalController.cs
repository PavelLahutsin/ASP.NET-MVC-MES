using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class ArrivalController : Controller
    {
        private readonly IArrivalService _service;
        private readonly IUnitOfWork _db;

        public ArrivalController(IArrivalService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

        // GET: Arrival
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

        //История прихода на склад
        public async Task<ActionResult> HistArrivalPartial(string startDate, string endDate)
        {
            var arrivals = Mapper.Map<IEnumerable<DisplayArrivalOfDetailDto>, List<DisplayArrivalOfDetailVm>>(await _service.ShowArryvalOfDedailsAsync(startDate, endDate));
            return PartialView(arrivals);
        }

        //Удаление Данных о добавлении на склад
        public async Task<ActionResult> DeleteArrival(int id)
        {
            var result = await _service.DeleteArrivalOfDetailAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Редактирование Данных о добавлении на склад
        public async Task<ActionResult> EditArrivalPartial(int id)
        {
            var arrival = Mapper.Map<ArrivalOfDetailVm>(await _db.ArrivalOfDetails.GetAsync(id));

            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            return PartialView(arrival);
        }

        [HttpPost]
        public async Task<ActionResult> EditArrivalPartial(ArrivalOfDetailVm arrival)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return View(arrival);

            var result = await _service.EditArrivalOfDetailAsync(Mapper.Map<ArrivalOfDetail>(arrival));

            return Json(result);
        }

        //прихон на склад 
        public ActionResult AddJmtDetailPartial()
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            ArrivalOfDetailVm arrivalOfDetail = new ArrivalOfDetailVm { Date = DateTime.Now };
            return PartialView(arrivalOfDetail);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddJmtDetailPartial(ArrivalOfDetailVm arrival)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            var detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return PartialView(arrival);
            
            var result = await _service.AddArrivalOfDetailAsync(Mapper.Map<ArrivalOfDetailDto>(arrival));
            return Json(result);
        }
    }
}