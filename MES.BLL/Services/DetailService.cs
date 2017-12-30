using System;
using System.Collections.Generic;
using System.Linq;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class DetailService : IDetailService
    {

        private readonly IUnitOfWork _uof;

        public DetailService(IUnitOfWork uof)
        {
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
        }

        /// <summary>
        /// Возвращает сколько деталей расходуется на 1 продукт
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<DetailDTO> GetDetail(string name)
        {
            return _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == name).Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantity = x.Detail.Quantity / x.Quantity
            });
            
        }
    }
}