using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
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


        public async Task<bool> AddSolderingAsync(SolderingDto soldering)
        {
            try
            {
                var structureOfTheProducts = _uof.StructureOfTheProducts.Entities.Where(w => w.ProductId == soldering.ProductId).ToList();

                foreach (var structureOfTheProduct in structureOfTheProducts)
                {
                    var detail = await _uof.Details.GetAsync(structureOfTheProduct.DetailId);
                    detail.Quantityq -= (structureOfTheProduct.Quantity * soldering.Quantity);
                    _uof.Details.Update(detail);
                }

                _uof.Solderings.Create(Mapper.Map<Soldering>(soldering));
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