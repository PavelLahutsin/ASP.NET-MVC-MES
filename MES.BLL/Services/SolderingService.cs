using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class SolderingService : ISolderingService
    {
        private readonly IUnitOfWork _uof;

        public SolderingService(IUnitOfWork uof)
        {
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
        }

        /// <summary>
        /// Добавляет пайку, отнимает израсходованные материалы со склада
        /// </summary>
        /// <param name="soldering"></param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> AddSolderingAsync(SolderingDto soldering)
        {
            try
            {
                var sol = new Soldering
                {
                    ProductId = soldering.ProductId,
                    Quantity = soldering.Quantity,
                    Date = soldering.Date
                };

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == soldering.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                prSt1.Quantity -= soldering.Quantity;

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == soldering.ProductId && w.StateProduct == VariantStateProduct.Спаяно).FirstOrDefaultAsync();

                prSt2.Quantity += soldering.Quantity;

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Solderings.Create(sol);
                await _uof.Commit();
                return new OperationDetails(true, "Пайка успешно добавлена", "");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(true, e.Message, ""); ;
            }


        }

     
        
        /// <summary>
        /// Возвращает инф о пайках в заданный период
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolderingDto>> ShowSolderingsAsync(string startDate, string endDate)
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

            var s = await _uof.Solderings.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new SolderingDto
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name
                }).OrderByDescending(x=>x.Date).ToListAsync();
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Количество отпаяной продукции за период</returns>
        public async Task<IEnumerable<SolderingCountDto>> ShowSolderingsCountAsync(string startDate, string endDate)
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
            return await _uof.Solderings.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).GroupBy(x=>x.ProductId).Select(s=>new SolderingCountDto()
            {
                Quantity = s.Sum(x=>x.Quantity),
                ProductName = s.FirstOrDefault().Product.Name
            }).OrderBy(x => x.ProductName).ToListAsync();
            
        }

        public async Task<bool> DeleteSoldering(int id)
        {
            try
            {
            var soldering = await _uof.Solderings.GetAsync(id);
           

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == soldering.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                prSt1.Quantity += soldering.Quantity;

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == soldering.ProductId && w.StateProduct == VariantStateProduct.Спаяно).FirstOrDefaultAsync();

                prSt2.Quantity -= soldering.Quantity;

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Solderings.Delete(id);
            await _uof.Commit();

            return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }
        }
    }
}