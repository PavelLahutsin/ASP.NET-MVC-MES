using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MES.DAL.Enums;
using MES.DAL.Interfaces;
using Microsoft.Ajax.Utilities;

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
            //public class ProductStateVm : IdProvider
            //{
            //public string ProductName { get; set; }

            //public VariantStateProduct StateProduct { get; set; }
            
            //public int Quantity { get; set; }
            //}

        //Получаем все имена
        var names = await _db.ProductStates.Entities.Select(x => x.Product.Name).Distinct().ToListAsync();
           

            //Далее генерирую HTML таблицу
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<table class=\"table table-striped table-bordered\">");

            stringBuilder.AppendLine("<tr class=\"text-red\">");
            AddCell(stringBuilder, string.Empty);

            foreach (var name in names)
            {
                AddCell(stringBuilder, name);
            }

            stringBuilder.AppendLine("</tr>");
           
            foreach (VariantStateProduct state in Enum.GetValues(typeof(VariantStateProduct)))
            {
                    stringBuilder.AppendLine("<tr class=\"text-green\">");

                    AddCell(stringBuilder, state.ToString());

                //Проходим по именам
                foreach (var name in names)
                {
                    //находим в бд объект где имя и статус совподают с нашими и добовляем к нам в string 
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