using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MES.BLL.Interfaces;
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _uof;

        public HomeService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<string> ProductInfo()
        {
            //Получаем все имена
            var names = await _uof.ProductStates.Entities.Select(x => x.Product.Name).Distinct().ToListAsync();


            //Далее генерирую HTML таблицу
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<table class=\"table table-striped table-bordered\">");

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

                //Проходим по именам
                foreach (var name in names)
                {
                    //находим в бд объект где имя и статус совподают с нашими и добовляем к нам в string 
                    var item = _uof.ProductStates.Entities.First(x =>
                        x.StateProduct == state && x.Product.Name == name);
                    AddCell(stringBuilder, item.Quantity.ToString());

                }
                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");

            return stringBuilder.ToString();
        }

        private static void AddCell(StringBuilder stringBuilder, string data)
        {
            stringBuilder.AppendLine($"<td width=\"100px\">{data}</td>");
        }
    }
}