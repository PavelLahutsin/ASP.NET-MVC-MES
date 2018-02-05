using MES.DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;

namespace MES.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uof;

        public AdminService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> CreateDetail(DetailDTO detailDto)
        {
            try
            {
                if (await _uof.Details.Entities.AnyAsync(a =>
                    a.Name == detailDto.Name || a.VendorCode == detailDto.VendorCode))
                    throw new Exception("Деталь с таким именем или кодом уже существует");

                var newDetail = new Detail
                {
                    Name = detailDto.Name,
                    VendorCode = detailDto.VendorCode,
                    GroupProductId = 1,
                    Quantityq = 0
                };

                _uof.Details.Create(newDetail);
                await _uof.Commit();
                return new OperationDetails(true, "Деталь успешно добавлена", "");
            }
            catch (Exception e)
            {
                return new OperationDetails(true, e.Message, "");
            }

        }

        public async Task<OperationDetails> CreateProduct(ProductDto productDto)
        {
            try
            {
                if (await _uof.Products.Entities.AnyAsync(a => a.Name == productDto.Name))
                    throw new Exception("Продукт с таким именем уже существует");

                var newProduct = new Product()
                {
                    Name = productDto.Name,

                };

                _uof.Products.Create(newProduct);
                await _uof.Commit();
                return new OperationDetails(true, "Продукт успешно добавлен", "");
            }
            catch (Exception e)
            {
                return new OperationDetails(true, e.Message, "");
            }

        }

        public async Task<OperationDetails> CreateStructProduct(StructureOfTheProductDto dto)
        {
            try
            {
                if (await _uof.StructureOfTheProducts.Entities.AnyAsync(a =>
                    a.DetailId == dto.DetailId && a.ProductId == dto.ProductId))
                    throw new Exception("Такая деталь уже существует");

                var structureOfTheProduct =
                    new StructureOfTheProduct {DetailId = dto.DetailId, ProductId = dto.ProductId};

                _uof.StructureOfTheProducts.Create(structureOfTheProduct);
                await _uof.Commit();
                return new OperationDetails(true, "Деталь успешно добавлена", "");
            }
            catch (Exception e)
            {
                return new OperationDetails(true, e.Message, "");
            }

        }


        public async Task<IEnumerable<ProductDto>> ListProduct()
        {
            return Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(await _uof.Products.GetAllAsync());
        }

        public async Task<IEnumerable<DetailInProductDto>> ListStructOfTheProduct(int id)
        {
            var structProduct = await _uof.StructureOfTheProducts.Entities.Where(w => w.ProductId == id).ToListAsync();

            return structProduct.Select(x => new DetailInProductDto
            {
                Id = x.DetailId,
                Name = x.Detail.Name,
                Quantity = x.Quantity
            });
        }
    }
}
