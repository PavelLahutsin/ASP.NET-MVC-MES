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
    public class AssemblyController : Controller
    {
        private readonly IAssemblyService _service;
        private readonly IUnitOfWork _db;

        public AssemblyController(IAssemblyService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }


        /// <summary>
        /// Главная страница паек
        /// </summary>
        /// <returns></returns>
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

        public async Task<ActionResult> AddAssemblyPartial()
        {
            var assembly = new AssemblyVm { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");

            return PartialView(assembly);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddAssemblyPartial(AssemblyVm assembly)
        {
            assembly.UserId= User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            if (!ModelState.IsValid) return PartialView(assembly);//Json(new{ success = false});
            var result = await _service.AddAssemblyAsync(Mapper.Map<AssemblyDto>(assembly));
           
            if (result.Succedeed) {
                string prodName = (await _db.Products.GetAsync(assembly.ProductId)).Name;
                ChatUser user = Mapper.Map<ChatUser>(await _db.Users.GetAsync(assembly.UserId));
                ChatMessage message = new ChatMessage
                {
                    Date = DateTime.Now,
                    Text = $"Я собрал {assembly.Quantity}шт. {prodName}",
                    User = user
                };
                if (ChatController.listMessage == null) ChatController.listMessage = new List<ChatMessage>();
                ChatController.listMessage.Add(message);
            }

            return Json(result);
        }

        public async Task<ActionResult> ListPartial(string startDate, string endDate)
        {


            var assemblyVm = Mapper.Map<IEnumerable<AssemblyDto>, List<AssemblyVm>>(
                await _service.ShowAssemblysAsync(startDate, endDate));
            return PartialView(assemblyVm);
        }


        

        public async Task<ActionResult> DeleteAssembly(int id)
        {

            var result = await _service.DeleteAssembly(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
    
}
