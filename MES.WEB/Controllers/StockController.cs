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
    [Authorize]
    public class StockController : Controller
    {
        private readonly IStockService _stockOn;
        private readonly IUnitOfWork _db;

        public StockController(IStockService stockOn, IUnitOfWork db)
        {
            _stockOn = stockOn;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        // Остаток деталей на складе
        public async Task<ActionResult> StockBalanceJmt()
        {
            var detail = Mapper.Map<IEnumerable<Detail>, List<DetailVm>>(await _db.Details.GetAllAsync());
            
            return PartialView("_StockBalanceJmt",detail);
        }

        
       

        public ActionResult AddDetail()
        {
            return PartialView("_AddDetail");
        }

        [HttpPost]
        public async Task<ActionResult> AddDetail(DetailVm modal)
        {
            if (!ModelState.IsValid) return PartialView("_AddDetail",modal);
            var result = await _stockOn.CreateDetail(Mapper.Map<DetailDTO>(modal));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditJmtDetail(int id)
        {
            var detail = Mapper.Map<DetailVm>(await _stockOn.GetDetail(id));
            return PartialView("_EditJmtDetail", detail);
        }

        [HttpPost]
        public async Task<ActionResult> EditJmtDetail(DetailVm detail)
        {
            if (!ModelState.IsValid) return PartialView("_EditJmtDetail", detail);
            var result = await _stockOn.EditDetail(Mapper.Map<DetailDTO>(detail));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _stockOn.DeleteDetailAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}