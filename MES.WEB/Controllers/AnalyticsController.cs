using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Enums;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _service;
        private readonly IUnitOfWork _db;

        public AnalyticsController(IAnalyticsService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

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

        public async Task<ActionResult> RemainingDetails()
        {
            var d52001 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(await _service.GetDetailOnProduct("5200-01"));
            //Random random = new Random();
            //for (int i = 0; i < 100; i++)
            //{
            //    DateTime date1 = new DateTime(2017, random.Next(1, 12), random.Next(1, 30));
            //    _db.Shipments.Create(new Shipment
            //    {
            //        Date = date1,
            //        BoxingVariant = BoxingVariant.Годная,
            //        ProductId = 3,
            //        UserId = 1,
            //        Quantity = random.Next(300, 3000)

            //    });
            //}
            //await _db.Commit();
            return PartialView("_RemainingDetails", d52001);
        }

        public async Task<ActionResult> RemainingDetailsIndex()
        {
            var d52001 = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(await _service.GetDetailOnProduct("5200-01"));
            return PartialView("_RemainingDetailsIndex", d52001);
        }

        public async Task<ActionResult> HowManySoldered(string startDate, string endDate)
        {
            var solderings = Mapper.Map<IEnumerable<SolderingCountDto>, List<SolderingCountVm>>(
                await _service.ShowSolderingsCountAsync(startDate, endDate));

            return PartialView("_HowManySoldered", solderings);
        }

        public async Task<ActionResult> HowManySolderedModal(string startDate, string endDate)
        {
            var solderings = Mapper.Map<IEnumerable<SolderingCountDto>, List<SolderingCountVm>>(
                await _service.ShowSolderingsCountAsync(startDate, endDate));

            return PartialView("_HowManySolderedModal", solderings);
        }

        public async Task<ActionResult> CheckInfo(string startDate, string endDate)
        {
            var check = Mapper.Map<ChekDetailsVm>(
                await _service.ShowCheckInfo(startDate, endDate));

            return PartialView("_CheckInfo", check);
        }

        public async Task<ActionResult> CheckInfoModal(string startDate, string endDate)
        {
            var check = Mapper.Map<ChekDetailsVm>(
                await _service.ShowCheckInfo(startDate, endDate));
            return PartialView("_CheckInfoModal", check);
        }

        public  async Task<ActionResult> ShipmentChart(string startDate, string endDate)
        {
            var check = Mapper.Map<IEnumerable<ShipmentChartDto>, List<ShipmentChartVm>>(
                 await _service.ShowShipmentAsync(startDate, endDate));

            return PartialView("_ShipmentChart", check);
        }
    }
}