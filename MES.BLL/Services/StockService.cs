using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class StockService : IStockService
    {

        private readonly IUnitOfWork _uof;

        public StockService(IUnitOfWork uof)
        {
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
        }
        
       
        /// <summary>
        /// Возвращает сколько деталей расходуется на 1 продукт
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <returns>список деталей</returns>
        public async Task<IEnumerable<DetailDTO>> GetDetailProduct(string name)
        {
            return await _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == name).Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantityq = x.Detail.Quantityq / x.Quantity
            }).ToListAsync();
            
        }

        public async Task<DetailDTO> GetDetail(int id) =>
            Mapper.Map<DetailDTO>(await _uof.Details.GetAsync(id));

        public async Task<OperationDetails> CreateDetail(DetailDTO detailDto)
        {
            try
            {
                if (await _uof.Details.Entities.AnyAsync(a=>a.Name==detailDto.Name || a.VendorCode==detailDto.VendorCode))
                {
                    throw new Exception("Такая деталь уже есть в базе");
                }

                var detail = new Detail
                {
                    Name = detailDto.Name,
                    VendorCode = detailDto.VendorCode,
                    Quantityq = 0,
                    GroupProductId = 1
                };
                _uof.Details.Create(detail);
                await _uof.Commit();

                return new OperationDetails(true, "Данные удалены", "/Stock/StockBalanceJmt");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }

        }

        public async Task<OperationDetails> EditDetail(DetailDTO detailDto)
        {
            try
            {
                var detail = await _uof.Details.GetAsync(detailDto.Id);

                detail.Name = detailDto.Name;
                detail.Quantityq = detailDto.Quantityq;
                detail.VendorCode = detailDto.VendorCode;

                _uof.Details.Update(detail);

                await _uof.Commit();

                return new OperationDetails(true, "Данные изменены", "/Stock/StockBalanceJmt");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }

        }


        /// <summary>
        /// Возвращает список деталей группы ЖМТ
        /// </summary>
        /// <returns>список деталей</returns>
        public List<DetailDTO> GetDetailsJmt() => Mapper.Map<IEnumerable<Detail>, List<DetailDTO>>(_uof.Details.Entities.Where(w => w.GroupProduct.Name == "JMT").ToList());
        

       

        public async Task<OperationDetails> DeleteDetailAsync(int id)
        {
            try
            {
                
                var detail = await _uof.Details.GetAsync(id);
                if (detail == null) throw new Exception("Нет такой детали в базе");
                
                _uof.Details.Delete(id);
                
                await _uof.Commit();

                return new OperationDetails(true, "Данные удалены", "");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Данные удалены", "");
            }

        }

       
    }

}