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
        public async Task<OperationDetails> AddCheckJmtAsync(CheckJmtDto checkJmtDto)
        {
            try
            {
                if ((checkJmtDto.RepairCu + checkJmtDto.RepairNi + checkJmtDto.RepairCentre) <
                    (checkJmtDto.Count - checkJmtDto.Airtight - checkJmtDto.Defect))
                    return new OperationDetails(false, "В ремонт + целые != поступившие", "");
                if ((checkJmtDto.CapM +
                     checkJmtDto.CapN +
                     checkJmtDto.Defect +
                     checkJmtDto.Center +
                     checkJmtDto.Housing +
                     checkJmtDto.Tube + checkJmtDto.Other) <
                    (checkJmtDto.Count - checkJmtDto.Airtight))
                    return new OperationDetails(false, "В ремонт + целые != поступившие", "");
                if (checkJmtDto.Count != (checkJmtDto.Airtight + checkJmtDto.CapM + checkJmtDto.CapN +
                                          checkJmtDto.Center + checkJmtDto.Defect + checkJmtDto.Housing +
                                          checkJmtDto.Other + checkJmtDto.Tube))
                    return new OperationDetails(false, "В ремонт + целые != поступившие", "");

                _uof.CheckJmts.Create(Mapper.Map<CheckJmt>(checkJmtDto));
                var prod = await _uof.ProductStates.Entities
                    .Where(w => w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.Спаяно)
                    .FirstOrDefaultAsync();
                if ((prod.Quantity -= checkJmtDto.Count) < 0) throw new Exception("Столько не спаяли");

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.Проверено)
                    .FirstOrDefaultAsync();
                prSt2.Quantity += checkJmtDto.Airtight;

                var prSt3 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.МЕДЬ)
                    .FirstOrDefaultAsync();
                prSt3.Quantity += checkJmtDto.RepairCu;

                var prSt4 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.НИКЕЛЬ)
                    .FirstOrDefaultAsync();
                prSt4.Quantity += checkJmtDto.RepairNi;

                var prSt5 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == checkJmtDto.ProductId && w.StateProduct == VariantStateProduct.ЦЕНТР)
                    .FirstOrDefaultAsync();
                prSt5.Quantity += checkJmtDto.RepairCentre;



                _uof.ProductStates.Update(prod);
                _uof.ProductStates.Update(prSt2);
                _uof.ProductStates.Update(prSt3);
                _uof.ProductStates.Update(prSt4);
                _uof.ProductStates.Update(prSt5);
                await _uof.Commit();

                if (checkJmtDto.State == StateFoTest.Новые)
                    return new OperationDetails(true, "Проверка успешно добавлена", "/CheckJmt/HistCheckJmtNewPartial");
                else
                {
                    return new OperationDetails(true, "Проверка успешно добавлена", "/CheckJmt/HistCheckJmtOldPartial");
                }
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
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

            return await _uof.CheckJmts.Entities.Where(w =>
                    w.Date >= myStartDate && w.Date <= myEndDate && w.State == StateFoTest.Новые)
                .Select(x => new CheckJmtForListDto
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

            return await _uof.CheckJmts.Entities.Where(w =>
                    w.Date >= myStartDate && w.Date <= myEndDate && w.State == StateFoTest.Отремонтированные)
                .Select(x => new CheckJmtForListDto
                {
                    Count = x.Count,
                    Date = x.Date,
                    Id = x.Id,
                    ProductName = x.Product.Name,
                    Airtight = x.Airtight
                }).ToListAsync();
        }

        public async Task<OperationDetails> DeleteCheck(int id)
        {
            try
            {
                var check = await _uof.CheckJmts.GetAsync(id);

                var prod = await _uof.ProductStates.Entities
                    .Where(w => w.ProductId == check.ProductId && w.StateProduct == VariantStateProduct.Спаяно)
                    .FirstOrDefaultAsync();
                prod.Quantity += check.Count;

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == check.ProductId && w.StateProduct == VariantStateProduct.Проверено)
                    .FirstOrDefaultAsync();
                if ((prSt2.Quantity -= check.Airtight ?? 0) < 0)
                    return new OperationDetails(false, "Не может быть удалено больше, чем добавлено!", "");

                var prSt3 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == check.ProductId && w.StateProduct == VariantStateProduct.МЕДЬ)
                    .FirstOrDefaultAsync();
                if ((prSt3.Quantity -= check.RepairCu ?? 0) < 0)
                    return new OperationDetails(false, "Не может быть удалено больше, чем добавлено!", "");

                var prSt4 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == check.ProductId && w.StateProduct == VariantStateProduct.НИКЕЛЬ)
                    .FirstOrDefaultAsync();
                if ((prSt4.Quantity -= check.RepairNi ?? 0) < 0)
                    return new OperationDetails(false, "Не может быть удалено больше, чем добавлено!", "");

                var prSt5 = await _uof.ProductStates.Entities.Where(w =>
                        w.ProductId == check.ProductId && w.StateProduct == VariantStateProduct.ЦЕНТР)
                    .FirstOrDefaultAsync();
                if ((prSt5.Quantity -= check.RepairCentre ?? 0) < 0)
                    return new OperationDetails(false, "Не может быть удалено больше, чем добавлено!", "");

                _uof.ProductStates.Update(prod);
                _uof.ProductStates.Update(prSt2);
                _uof.ProductStates.Update(prSt3);
                _uof.ProductStates.Update(prSt4);
                _uof.ProductStates.Update(prSt5);



                _uof.CheckJmts.Delete(id);
                await _uof.Commit();
                return new OperationDetails(true, "Успешно удалено", "");

            }
            catch (Exception)
            {

                return new OperationDetails(false, "Не прошло", "");
            }
        }

        public async Task<ChekDetailsDto> DetailsCheck(int id)
        {
            return await _uof.CheckJmts.Entities.Where(w => w.Id == id).Select(x => new ChekDetailsDto
            {
                Date = x.Date,
                Count = x.Count,
                Airtight = x.Airtight,
                State = x.State,
                UserName = x.User.UserName,
                Defect = x.Defect,
                ProductName = x.Product.Name,
                Housing = x.Housing,
                Other = x.Other,
                Tube = x.Tube,
                CapN = x.CapN,
                CapM = x.CapM,
                Center = x.Center,
                RepairNi = x.RepairNi,
                RepairCu = x.RepairCu,
                RepairCentre = x.RepairCentre
            }).FirstOrDefaultAsync();
        }
    }
}