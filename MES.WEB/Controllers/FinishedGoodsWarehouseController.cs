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
    [Authorize]
    public class FinishedGoodsWarehouseController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IShipmentService _service;

        public FinishedGoodsWarehouseController(IUnitOfWork setvice, IShipmentService finishedGoods)
        {
            _db = setvice;
            _service = finishedGoods;
        }

        // GET: FinishedGoodsWarehouseServiceService
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
        
        public async Task<ActionResult> AddShipmentPartial()
        {
            var shipmentVm = new ShipmentVm { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return PartialView(shipmentVm);
           
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddShipmentPartial(ShipmentVm shipment)
        {
            shipment.UserId = User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            if (!ModelState.IsValid) return PartialView(shipment);
            var result = await _service.AddShipmentAsync(Mapper.Map<ShipmentDto>(shipment));
            return Json(result);
        }

        public async Task<ActionResult> ListPartial(string startDate, string endDate)
        {
            var shipmentList = Mapper.Map<IEnumerable<ShipmentDto>, List<ShipmentVm>>(
                await _service.ShowShipmentAsync(startDate, endDate));
            return PartialView(shipmentList);
        }

        public async Task<ActionResult> PackagedCount(string startDate, string endDate)
        {
            var packagedList = Mapper.Map<IEnumerable<ProductStateDto>, List<ProductStateVm>>(await _service.PackagedShow());
            return PartialView(packagedList);
        }

        public async Task<ActionResult> Delete(int id)
        {

            var result = await _service.DeleteShipment(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}