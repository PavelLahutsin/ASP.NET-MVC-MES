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
    public class RepairService : IRepairService
    {
        private IUnitOfWork _uof;

        public RepairService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> AddRepairAsync(RepairDto repairDto)
        {
            try
            {
                var repair = new Repair()
                {
                    Date = repairDto.Date,
                    ProductId = repairDto.ProductId,
                    Quantity = repairDto.Quantity,
                    RepairsVariant = repairDto.RepairsVariant,
                    UserId = repairDto.UserId
                };

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == repairDto.ProductId && w.StateProduct == VariantStateProduct.Спаяно).FirstOrDefaultAsync();

                if ((prSt1.Quantity += repairDto.Quantity) < 0) throw new Exception("Столько не нет");

                ProductState prSt2 = new ProductState();
                switch (repairDto.RepairsVariant)
                {
                    case RepairsVariant.Медь:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repairDto.ProductId && w.StateProduct == VariantStateProduct.МЕДЬ)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity -= repairDto.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }

                    case RepairsVariant.Никель:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repairDto.ProductId && w.StateProduct == VariantStateProduct.НИКЕЛЬ)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity -= repairDto.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }

                    case RepairsVariant.Центр:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repairDto.ProductId && w.StateProduct == VariantStateProduct.ЦЕНТР)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity -= repairDto.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }
                }
                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Repairs.Create(repair);
                await _uof.Commit();
                return new OperationDetails(true, "Упаковка успешно добавлена", "/Repair/ListPartial");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(false, e.Message, "/Repair/ListPartial");
            }
        }

        public async Task<IEnumerable<RepairDto>> ListRepairAsync(string startDate, string endDate)
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

            return await _uof.Repairs.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new RepairDto()
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    RepairsVariant = x.RepairsVariant,
                    UserName = x.User.UserName
                }).OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<OperationDetails> DeleteRepair(int id)
        {
            try
            {
                var repair = await _uof.Repairs.Entities.FirstOrDefaultAsync(f => f.Id == id);


                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == repair.ProductId && w.StateProduct == VariantStateProduct.Спаяно).FirstOrDefaultAsync();

                if ((prSt1.Quantity -= repair.Quantity) < 0) throw new Exception("Столько нет");

                ProductState prSt2 = new ProductState();
                switch (repair.RepairsVariant)
                {
                    case RepairsVariant.Медь:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repair.ProductId && w.StateProduct == VariantStateProduct.МЕДЬ)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity += repair.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }

                    case RepairsVariant.Никель:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repair.ProductId && w.StateProduct == VariantStateProduct.НИКЕЛЬ)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity += repair.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }

                    case RepairsVariant.Центр:
                        {
                            prSt2 = await _uof.ProductStates.Entities.Where(w =>
                                    w.ProductId == repair.ProductId && w.StateProduct == VariantStateProduct.ЦЕНТР)
                                .FirstOrDefaultAsync();
                            if ((prSt2.Quantity += repair.Quantity) < 0)
                                throw new Exception("Нельзя удалить больше, чем есть");
                            break;
                        }
                }

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Repairs.Delete(id);
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