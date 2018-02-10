using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;
        private readonly IStockService _stock;

        public AdminController(IAdminService service, IStockService stock)
        {
            _service = service;
            _stock = stock;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListProduct()
        {
            var products = Mapper.Map<IEnumerable<ProductDto>, List<ProductVm>>(await _service.ListProduct());
            return PartialView("_ListProduct", products);
        }

        public async Task<ActionResult> ListStructProduct(int id)
        {
            var structProducts = Mapper.Map<IEnumerable<DetailInProductDto>, List<DetailInProductVm>>(await _service.ListStructOfTheProduct(id));
            return PartialView("_ListStructProduct", structProducts);
        }


        public ActionResult AddProduct()
        {
            return PartialView("_AddProduct");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductVm productVm)
        {
            if(!ModelState.IsValid) return PartialView("_AddProduct", productVm);
            var result = await _service.CreateProduct(Mapper.Map<ProductDto>(productVm));
            return Json(result);
        }

        public ActionResult AddStructProduct(int id)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stock.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            DetailInProductVm detail = new DetailInProductVm {ProductId = id};
            return PartialView("_AddStructProduct", detail);
        }
        [HttpPost]
        public async Task<ActionResult> AddStructProduct(DetailInProductVm detail)
        {
            var details = Mapper.Map<IEnumerable<DetailDTO>, List<DetailVm>>(_stock.GetDetailsJmt());
            SelectList detailList = new SelectList(details, "Id", "Name");
            ViewBag.Detail = detailList;
            if (!ModelState.IsValid) return PartialView("_AddStructProduct", detail);

            var result = await _service.CreateStructProduct(Mapper.Map<DetailInProductDto>(detail));
            return Json(result);
        }

        public async Task<ActionResult> DeleteFromProduct(int detailid, int productId)
        {
            var result = await _service.DeleteDetailOnStructProduct(detailid, productId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _service.DeleteProduct(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}