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

        public async Task<OperationDetails> AddCheckJmtAsync(CheckJmtDto checkJmtDto){
            
            _uof.CheckJmts.Create(Mapper.Map<CheckJmt>(checkJmtDto));
            await _uof.Commit();
            return new OperationDetails(true, "Проверка успешно добавлена", "");
        }

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