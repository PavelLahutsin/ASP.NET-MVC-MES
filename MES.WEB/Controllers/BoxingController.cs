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
    public class BoxingController : Controller
    {
        private readonly IBoxingService _service;
        private readonly IUnitOfWork _db;

        public BoxingController(IBoxingService service, IUnitOfWork db)
        {
            _service = service;
            _db = db;
        }

        // GET: Boxing
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

        public async Task<ActionResult> AddBoxingPartial()
        {
            var boxing = new BoxingVm() { Date = DateTime.Now };
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return PartialView(boxing);
        }

        [HttpPost]
        public async Task<ActionResult> AddBoxingPartial(BoxingVm boxing)
        {
            boxing.UserId = User.Identity.GetUserId<int>();
            var products = Mapper.Map<IEnumerable<Product>, List<ProductVm>>(await _db.Products.GetAllAsync());
            ViewBag.Products = new SelectList(products, "Id", "Name");
            
            if (!ModelState.IsValid)
            {
                return PartialView(boxing);
            }

            var result = await _service.AddBoxingAsync(Mapper.Map<BoxingDto>(boxing));

            if (result.Succedeed)
            {
                string prodName = (await _db.Products.GetAsync(boxing.ProductId)).Name;
                ChatUser user = Mapper.Map<ChatUser>(await _db.Users.GetAsync(boxing.UserId));
                ChatMessage message = new ChatMessage
                {
                    Date = DateTime.Now,
                    Text = $"Я упаковал {boxing.Quantity}шт. {prodName}",
                    User = user
                };
                if (ChatController.listMessage == null) ChatController.listMessage = new List<ChatMessage>();
                ChatController.listMessage.Add(message);
            }
            return Json(result);
        }

        public async Task<ActionResult> HistBoxingPartial(string startDate, string endDate)
        {
            var boxings = Mapper.Map<IEnumerable<BoxingDto>, List<BoxingVm>>(await _service.ShowBoxingsAsync(startDate, endDate));
            return PartialView(boxings);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteBoxing(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}