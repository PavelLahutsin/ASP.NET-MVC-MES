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
    public class AssemblyService : IAssemblyService
    {
        private readonly IUnitOfWork _uof;

        public AssemblyService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> AddAssemblyAsync(AssemblyDto assembly)
        {
            try
            {
                var structureOfTheProducts = _uof.StructureOfTheProducts.Entities
                    .Where(w => w.ProductId == assembly.ProductId).ToList();

                foreach (var structureOfTheProduct in structureOfTheProducts)
                {
                    var detail = await _uof.Details.GetAsync(structureOfTheProduct.DetailId);
                    if ((detail.Quantityq -= (structureOfTheProduct.Quantity * assembly.Quantity)) < 0) throw new Exception($"Число деталей ({detail.Name}) на складе не может быть отрицательным");
                    _uof.Details.Update(detail);
                }
                var ass = new Assembly()
                {
                    ProductId = assembly.ProductId,
                    Quantity = assembly.Quantity,
                    Date = assembly.Date
                };

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == assembly.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                prSt1.Quantity += assembly.Quantity;

                

                _uof.ProductStates.Update(prSt1);
                

                _uof.Assemblys.Create(ass);
                await _uof.Commit();
                return new OperationDetails(true, "Сборка успешно добавлена", "");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(true, e.Message, ""); ;
            }
        }

        public async Task<IEnumerable<AssemblyDto>> ShowAssemblysAsync(string startDate, string endDate)
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

            var s = await _uof.Assemblys.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new AssemblyDto()
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name
                }).OrderByDescending(x => x.Date).ToListAsync();
            return s;
        }

        public async Task<bool> DeleteAssembly(int id)
        {
            try
            {
                var assembly = await _uof.Assemblys.GetAsync(id);
                var structureOfTheProducts = _uof.StructureOfTheProducts.Entities
                    .Where(w => w.ProductId == assembly.ProductId).ToList();

                foreach (var structureOfTheProduct in structureOfTheProducts)
                {
                    var detail = await _uof.Details.GetAsync(structureOfTheProduct.DetailId);
                    detail.Quantityq += (structureOfTheProduct.Quantity * assembly.Quantity);
                    _uof.Details.Update(detail);
                }

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == assembly.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                if ((prSt1.Quantity -= assembly.Quantity)<0) throw new Exception();

               

                _uof.ProductStates.Update(prSt1);

                _uof.Assemblys.Delete(id);
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