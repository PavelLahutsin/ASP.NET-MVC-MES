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
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class BoxingService : IBoxingService
    {
        private readonly IUnitOfWork _uof;

        public BoxingService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> AddBoxingAsync(BoxingDto boxing)
        {
            try
            {
                var box = new Boxing{Date = boxing.Date, ProductId =boxing.ProductId, Quantity = boxing.Quantity, BoxingVariant = boxing.BoxingVariant};

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Проверено).FirstOrDefaultAsync();

                if((prSt1.Quantity -= boxing.Quantity)<0) throw new Exception("Столько не проверено");

                ProductState prSt2 = new ProductState();
                switch (boxing.BoxingVariant)
                {
                    case BoxingVariant.Годная:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Упаковано).FirstOrDefaultAsync();
                        prSt2.Quantity += boxing.Quantity;
                            break;
                    }

                    case BoxingVariant.Вторичка:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Вторичка).FirstOrDefaultAsync();
                        prSt2.Quantity += boxing.Quantity;
                        break;
                    }

                    case BoxingVariant.Запаски:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Запаски).FirstOrDefaultAsync();
                        prSt2.Quantity += boxing.Quantity;
                        break;
                    }
                }

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Boxings.Create(box);
                await _uof.Commit();
                return new OperationDetails(true, "Упаковка успешно добавлена", "/Boxing/HistBoxingPartial");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(false, e.Message, "/Boxing/HistBoxingPartial");
            }
        }

        public async Task<IEnumerable<BoxingDto>> ShowBoxingsAsync(string startDate, string endDate)
        {
            DateTime myEndDate;
            DateTime myStartDate;
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                myEndDate = DateTime.Now;
                myStartDate = new DateTime(myEndDate.Year, myEndDate.Month, 1);
            }
            else
            {
                myEndDate = DateTime.Parse(endDate);
                myStartDate = DateTime.Parse(startDate);
            }
            
            return await _uof.Boxings.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new BoxingDto()
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    BoxingVariant = x.BoxingVariant
                }).OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<OperationDetails> DeleteBoxing(int id)
        {
            try
            {
                var boxing = await _uof.Boxings.Entities.FirstOrDefaultAsync(f=>f.Id==id);


                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Проверено).FirstOrDefaultAsync();

                prSt1.Quantity += boxing.Quantity;

                ProductState prSt2 = new ProductState();
                switch (boxing.BoxingVariant)
                {
                    case BoxingVariant.Годная:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Упаковано).FirstOrDefaultAsync();
                        if((prSt2.Quantity -= boxing.Quantity)<0) throw new Exception("Нельзя удалить больше, чем есть");
                        break;
                    }

                    case BoxingVariant.Вторичка:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Вторичка).FirstOrDefaultAsync();
                        prSt2.Quantity -= boxing.Quantity;
                        break;
                    }

                    case BoxingVariant.Запаски:
                    {
                        prSt2 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == boxing.ProductId && w.StateProduct == VariantStateProduct.Запаски).FirstOrDefaultAsync();
                        prSt2.Quantity -= boxing.Quantity;
                        break;
                    }
                }

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Boxings.Delete(id);
                await _uof.Commit();

                return new OperationDetails(true, "Упаковка успешно удалена", "");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(false, e.Message, "");
            }
        }
    }
}