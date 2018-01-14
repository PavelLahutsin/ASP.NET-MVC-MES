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
    public class CheckJmtService : ICheckJmtService
    {
        private readonly IUnitOfWork _uof;

        public CheckJmtService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        /// <summary>
        /// Добавить инф о проверке
        /// </summary>
        /// <param name="checkJmtDto"></param>
        /// <returns></returns>
        public async Task<OperationDetails> AddCheckJmtAsync(CheckJmtDto checkJmtDto){
            try
            {
                _uof.CheckJmts.Create(Mapper.Map<CheckJmt>(checkJmtDto));
                var prod = await _uof.ProductStates.Entities.Where(w => w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.Спаяно).FirstOrDefaultAsync();
                prod.Quantity -= checkJmtDto.Airtight ?? 0;

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.Проверено).FirstOrDefaultAsync();
                prSt2.Quantity += checkJmtDto.Airtight ?? 0;

                _uof.ProductStates.Update(prod);
                _uof.ProductStates.Update(prSt2);
                await _uof.Commit();
                return new OperationDetails(true, "Проверка успешно добавлена", "");
            }
            catch (Exception)
            {
                return new OperationDetails(false, "Bad", "");
            }   
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>историю проверок новых теплообменников</returns>
        public async Task<IEnumerable<CheckJmtForListDto>> ShowCheckJmtNewAsync(string startDate, string endDate)
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

            return await _uof.CheckJmts.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate && w.State== StateFoTest.Новые)
                .Select(x=>new CheckJmtForListDto
                {
                    Count = x.Count,
                    Date = x.Date,
                    Id = x.Id,
                    ProductName = x.Product.Name,
                    Airtight = x.Airtight
                }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>историю проверок отремонтированных теплообменников</returns>
        public async Task<IEnumerable<CheckJmtForListDto>> ShowCheckJmtOldAsync(string startDate, string endDate)
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

            return await _uof.CheckJmts.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate && w.State == StateFoTest.Отремонтированные)
                .Select(x => new CheckJmtForListDto
                {
                    Count = x.Count,
                    Date = x.Date,
                    Id = x.Id,
                    ProductName = x.Product.Name,
                    Airtight = x.Airtight
                }).ToListAsync();
        }
    }
}