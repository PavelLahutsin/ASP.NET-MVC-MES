using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            var states = await _db.ProductStates.Entities.Select(x => x.VariantStateProduct.Name).Distinct().ToListAsync();

            //Далее генерирую HTML таблицу
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<table class=\"table table - striped\">");

            stringBuilder.AppendLine("<tr>");
            AddCell(stringBuilder, string.Empty);

            foreach (var name in names)
            {
                AddCell(stringBuilder, name);
            }

            stringBuilder.AppendLine("</tr>");
            foreach (var state in states)
            {
                stringBuilder.AppendLine("<tr>");
                AddCell(stringBuilder, state);
                foreach (var name in names)
                {
                    var item = _db.ProductStates.Entities.First(x =>
                        x.VariantStateProduct.Name == state && x.Product.Name == name);
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