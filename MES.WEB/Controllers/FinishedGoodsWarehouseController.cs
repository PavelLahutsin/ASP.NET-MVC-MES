using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    [Authorize]
    public class FinishedGoodsWarehouseController : Controller
    {
        private IUnitOfWork _setvice;
        private readonly IFinishedGoodsWarehouseService _finishedGoods;

        public FinishedGoodsWarehouseController(IUnitOfWork setvice, IFinishedGoodsWarehouseService finishedGoods)
        {
            _setvice = setvice;
            _finishedGoods = finishedGoods;
        }

        // GET: FinishedGoodsWarehouseServiceService
        public async Task<ActionResult> Index()
        {
            var packagedList = Mapper.Map<IEnumerable<ProductStateDto>, List<ProductStateVm>>(await _finishedGoods.PackagedList());
            return View(packagedList);
        }
    }
}