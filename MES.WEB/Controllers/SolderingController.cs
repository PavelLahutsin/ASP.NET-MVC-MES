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
    public class SolderingController : Controller
    {
        private readonly ISolderingService _solderingService;
        private readonly IUnitOfWork _db;

        public SolderingController(ISolderingService solderingService, IUnitOfWork db)
        {
            _solderingService = solderingService;
            _db = db;
        }


        /// <summary>
        /// Главная страница паек
        /// </summary>
        /// <returns></returns>
        public ActionResult Soldering()
        {
            var now = DateTime.Now;
            var date = new DateVm
            {
                StartDate = new DateTime(now.Year, now.Month, 1),
                EndDate = now
            };
            return View(date);
        }

        public async Task<ActionResult> AddSolderingPartial()
        {
            var soldering = new SolderingVm { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return PartialView(soldering);
        }

        [HttpPost]
        public async Task<ActionResult> AddSolderingPartial(SolderingVm soldering)
        {
            soldering.UserId = User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            if (!ModelState.IsValid) return PartialView(soldering);
            var result = await _solderingService.AddSolderingAsync(Mapper.Map<SolderingDto>(soldering));
            return Json(result);
        }


       

        public async Task<ActionResult> ListPartial(string startDate, string endDate)
        {
            
         
            var solderings = Mapper.Map<IEnumerable<SolderingDto>, List<SolderingVm>>(
                await _solderingService.ShowSolderingsAsync(startDate, endDate));
            return PartialView(solderings);
        }


        public async Task<ActionResult> ShowSolderingCountPartial(string startDate, string endDate)
        {
           

            var solderings = Mapper.Map<IEnumerable<SolderingCountDto>, List<SolderingCountVm>>(
                await _solderingService.ShowSolderingsCountAsync(startDate, endDate));

            return PartialView(solderings);

        }

        public async Task<ActionResult> DeleteSoldering(int id)
        {

            var result = await _solderingService.DeleteSoldering(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        
    }
}