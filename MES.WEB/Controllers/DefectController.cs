using System;
using System.Collections.Generic;
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
    public class DefectController : Controller
    {
        private readonly IDefectService _service;
        private readonly IUnitOfWork _db;

        public DefectController(IDefectService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
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

            if (result.Succedeed)
            {
                string prodName = (await _db.Details.GetAsync(defect.DetailId)).Name;
                ChatUser user = Mapper.Map<ChatUser>(await _db.Users.GetAsync(defect.UserId));
                ChatMessage message = new ChatMessage
                {
                    Date = DateTime.Now,
                    Text = $"Я добавил в брак {prodName} {defect.Count}шт.",
                    User = user
                };
                if (ChatController.listMessage == null) ChatController.listMessage = new List<ChatMessage>();
                ChatController.listMessage.Add(message);
            }
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