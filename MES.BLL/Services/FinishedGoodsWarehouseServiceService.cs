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
    public class FinishedGoodsWarehouseServiceService : IFinishedGoodsWarehouseService
    {
        private readonly IUnitOfWork _uof;

        public FinishedGoodsWarehouseServiceService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<IEnumerable<ProductStateDto>> PackagedList() => await _uof.ProductStates.Entities
            .Where(w => w.StateProduct == VariantStateProduct.Упаковано)
            .Select(s => new ProductStateDto
            {
                StateProduct = s.StateProduct,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                Id = s.Id,
                ProductName = s.Product.Name
            }).ToListAsync();

        public Task<OperationDetails> AddShipping(ProductStateDto productState)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationDetails> DeleteShipping(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}