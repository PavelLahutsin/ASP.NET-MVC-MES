using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class DefectController : Controller
    {
        private readonly IDefectService _service;

        public DefectController(IDefectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Добавить детали в брак
        /// </summary>
        /// <returns>PartialView</returns>
        public ActionResult AddDefectJmtDetailPartial()
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            DefectDetailVm defect = new DefectDetailVm { Date = DateTime.Now };
            return PartialView(defect);
        }

        [HttpPost]
        public async Task<ActionResult> AddDefectJmtDetailPartial(DefectDetailVm defect)
        {
            defect.UserId = User.Identity.GetUserId<int>();
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return PartialView(defect);

            var result = await _service.AddDefectDetailAsync(Mapper.Map<DefectDetailDto>(defect));
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //История брака на склад
        public async Task<ActionResult> HistDefectPartial()
        {
            var defect = Mapper.Map<IEnumerable<DefectDetailDisplayDto>, List<DefectDetailDisplayVm>>(await _service.ShowDefectDetailAsync());
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
            var result = await _service.DeleteDefectDetailAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Редактирование Данных о браковке деталей
        public async Task<ActionResult> EditDefectPartial(int id)
        {
            var defect = Mapper.Map<DefectDetailVm>(await _service.GetDefect(id));

            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            return PartialView(defect);
        }

        [HttpPost]
        public async Task<ActionResult> EditDefectPartial(DefectDetailVm defect)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_service.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;

            if (!ModelState.IsValid) return View(defect);

            var result = await _service.EditDefectDetailAsync(Mapper.Map<DefectDetail>(defect));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}