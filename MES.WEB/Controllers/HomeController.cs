using System.Threading.Tasks;
using System.Web.Mvc;
using MES.BLL.Interfaces;

namespace MES.WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            _service = service;
        }


        public async Task<ActionResult> Index()
        {
            var str = MvcHtmlString.Create(await _service.ProductInfo());
           


            return View(str);
        }

      

    }
}