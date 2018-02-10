using MES.DAL.Interfaces;
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
                return new OperationDetails(false, e.Message, "");
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

                foreach (VariantStateProduct vaStPr in Enum.GetValues(typeof(VariantStateProduct)))
                {
                    _uof.ProductStates.Create(new ProductState { Product = newProduct, StateProduct = vaStPr, Quantity = 0 });
                   
                }

                await _uof.Commit();
                return new OperationDetails(true, "Продукт успешно добавлен", "/Admin/ListProduct");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }

        }

        public async Task<OperationDetails> CreateStructProduct(DetailInProductDto dto)
        {
            try
            {
                if (await _uof.StructureOfTheProducts.Entities.AnyAsync(a =>
                    a.DetailId == dto.Id && a.ProductId == dto.ProductId))
                    throw new Exception("Такая деталь уже существует");

                var structureOfTheProduct =
                    new StructureOfTheProduct {DetailId = dto.Id, ProductId = dto.ProductId, Quantity = dto.Quantity};

                _uof.StructureOfTheProducts.Create(structureOfTheProduct);
                await _uof.Commit();
                return new OperationDetails(true, "Деталь успешно добавлена", $"/Admin/ListStructProduct/{dto.ProductId}");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, $"/Admin/ListStructProduct/{dto.ProductId}");
            }

        }


        public async Task<IEnumerable<ProductDto>> ListProduct()
        {
            return Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(await _uof.Products.GetAllAsync());
        }

        public async Task<IEnumerable<DetailInProductDto>> ListStructOfTheProduct(int id)
        {
            var structProduct = await _uof.StructureOfTheProducts.Entities.Where(w => w.ProductId == id).ToListAsync();
            if (structProduct.Count == 0)
            {
                return new List<DetailInProductDto>
                {
                    new DetailInProductDto
                    {
                        Id = -1,
                        ProductId = id
                    }
                };
            }
            return structProduct.Select(x => new DetailInProductDto
            {
                ProductId = x.ProductId,
                Id = x.DetailId,
                Name = x.Detail.Name,
                Quantity = x.Quantity
            });
        }

        public async Task<OperationDetails> DeleteDetailOnStructProduct(int detailid, int productId)
        {
            try
            {
                var structureOfTheProduct = await _uof.StructureOfTheProducts.Entities.FirstAsync(a =>
                    a.DetailId == detailid && a.ProductId == productId);
                
                _uof.StructureOfTheProducts.Entities.Remove(structureOfTheProduct);
                await _uof.Commit();
                return new OperationDetails(true, "Успешно удалена!", "");
            }
            catch (InvalidOperationException)
            {
                return new OperationDetails(false, "Нет в составе", "");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }
        }

        public async Task<OperationDetails> DeleteProduct(int id)
        {
            try
            {
                if(!await _uof.Products.Entities.AnyAsync(a=>a.Id==id)) throw new Exception("Этого продукта нет в базе");
                _uof.Products.Delete(id);
                await _uof.Commit();
                return new OperationDetails(true, "Успешно удалена!", "");
            }
            catch (Exception e)
            {
                return new OperationDetails(false, e.Message, "");
            }
        }
    }
}
