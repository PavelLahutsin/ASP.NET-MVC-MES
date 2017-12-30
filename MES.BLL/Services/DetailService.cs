using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
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

        
        // Возвращает сколько деталей расходуется на 1 продукт
        public IEnumerable<DetailDTO> GetDetail(string name)
        {
            return _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == name).Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantity = x.Detail.Quantity / x.Quantity
            });
            
        }

        // Возвращает список деталей группы ЖМТ
        public List<DetailDTO> GetDetailsJmt() => Mapper.Map<IEnumerable<Detail>, List<DetailDTO>>(_uof.Details.Entities.Where(w => w.GroupProduct.Name == "JMT").ToList());

        public async Task AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival)
        {
            var detail = await _uof.Details.GetAsync(arrival.DetailId);
            if (detail == null) throw new Exception();
            var arrivalOfDetail = Mapper.Map<ArrivalOfDetail>(arrival);
            _uof.ArrivalOfDetails.Create(arrivalOfDetail);


            detail.Quantity += arrival.Count;
            _uof.Details.Update(detail);
            await _uof.Commit();
        }
    }
    
}