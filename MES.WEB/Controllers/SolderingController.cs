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
            if (ModelState.IsValid)
            {
                var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
                ViewBag.Products = new SelectList(products, "Id", "Name");

                var result = await _solderingService.AddSolderingAsync(Mapper.Map<SolderingDto>(soldering));

                if (result.Succedeed)
                    return RedirectToAction("Soldering");
                else
                    ModelState.AddModelError(result.Property, result.Message);
            }
            return PartialView(soldering);
        }


       

        public async Task<ActionResult> ShowSolderingListPartial(string startDate, string endDate)
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
            return RedirectToAction("Soldering");

        }

        //public async Task<ActionResult> EditSoldering(int id)
        //{
        //    var soldering = Mapper.Map<SolderingVm>(await _db.Solderings.GetAsync(id));
        //    return PartialView(soldering);

        //}


    }
}