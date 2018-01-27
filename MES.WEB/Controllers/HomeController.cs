using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly IUnitOfWork _db;

        public HomeController(IUnitOfWork db)
        {
           
            _db = db;
        }

        /// <summary>
        /// Добавление ячейки
        /// </summary>
        private static void AddCell(StringBuilder stringBuilder, string data)
        {
            stringBuilder.AppendLine($"<td width=\"100px\">{data}</td>");
        }

        public async Task<ActionResult> Index()
        {
            //Получаем все имена и статусы
            var names = await _db.ProductStates.Entities.Select(x => x.Product.Name).Distinct().ToListAsync();
           

            //Далее генерирую HTML таблицу
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<table class=\"table\">");

            stringBuilder.AppendLine("<tr>");
            AddCell(stringBuilder, string.Empty);

            foreach (var name in names)
            {
                AddCell(stringBuilder, name);
            }

            stringBuilder.AppendLine("</tr>");
            foreach (VariantStateProduct state in Enum.GetValues(typeof(VariantStateProduct)))
            {
                stringBuilder.AppendLine("<tr>");
                AddCell(stringBuilder, state.ToString());
                foreach (var name in names)
                {
                    var item = _db.ProductStates.Entities.First(x =>
                        x.StateProduct == state && x.Product.Name == name);
                    AddCell(stringBuilder, item.Quantity.ToString());
                }
                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");



            ViewBag.table = MvcHtmlString.Create(stringBuilder.ToString());


            return View();
        }

      

    }
}