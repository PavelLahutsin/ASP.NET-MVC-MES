﻿using System;
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
    public class CheckJmtController : Controller
    {
        private readonly ICheckJmtService _service;
        private readonly IUnitOfWork _db;

        public CheckJmtController(ICheckJmtService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

        // GET: CheckJmt
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

        //История проверок нов
        public async Task<ActionResult> HistCheckJmtNewPartial(string startDate, string endDate)
        {
            var arrivals = Mapper.Map<IEnumerable<CheckJmtForListDto>, List<CheckJmtForListVm>>(await _service.ShowCheckJmtNewAsync(startDate, endDate));
            return PartialView(arrivals);
        }

        //История проверок ремонт
        public async Task<ActionResult> HistCheckJmtOldPartial(string startDate, string endDate)
        {
            var arrivals = Mapper.Map<IEnumerable<CheckJmtForListDto>, List<CheckJmtForListVm>>(await _service.ShowCheckJmtOldAsync(startDate, endDate));
            return PartialView(arrivals);
        }

        public async Task<ActionResult> AddCheckJmtPartial()
        {
            var checkJmtVm = new CheckJmtVm { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return PartialView(checkJmtVm);
        }

        [HttpPost]
        public async Task<ActionResult> AddCheckJmtPartial(CheckJmtVm chek)
        {
            chek.UserId= User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            if (!ModelState.IsValid) return PartialView(chek);
            var result = await _service.AddCheckJmtAsync(Mapper.Map<CheckJmtDto>(chek));
            return Json(result);
        }

        public async Task<ActionResult> Details(int id)
        {
            var check = Mapper.Map<ChekDetailsVm>(await _service.DetailsCheck(id));
            
            return PartialView(check);
        }
        
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteCheck(id);


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}