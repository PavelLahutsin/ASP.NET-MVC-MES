using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockService _stockOn;
        private readonly IUnitOfWork _db;

        public HomeController(IStockService stockOn, IUnitOfWork db)
        {
            _stockOn = stockOn;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            var stPr = Mapper.Map<IEnumerable<ProductState>, List<ProductStateVm>>( await _db.ProductStates.GetAllAsync());
            var state = _db.VariantStateProducts.Entities.Select(x => x.Name).ToList();
            return View(stPr);
        }

       

      
    }
}