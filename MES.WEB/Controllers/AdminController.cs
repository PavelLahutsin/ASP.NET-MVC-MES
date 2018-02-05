using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.WEB.Models;

namespace MES.WEB.Controllers
{
    public class AdminController : Controller
    {
        private IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
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


    }
}