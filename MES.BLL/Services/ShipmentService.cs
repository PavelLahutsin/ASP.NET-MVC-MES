using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.EF;
using MES.DAL.Entities;
using MES.DAL.Enums;

namespace MES.BLL.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly UnitOfWork _uof;

        public ShipmentService(UnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> AddShipmentAsync(ShipmentDto shipmentDto)
        {
            try
            {
                var shipment = new Shipment
                {
                    BoxingVariant = shipmentDto.BoxingVariant,
                    ProductId = shipmentDto.ProductId,
                    Quantity = shipmentDto.Quantity,
                    Date = shipmentDto.Date,
                    UserId = shipmentDto.UserId
                };

                ProductState prSt1;
                switch (shipmentDto.BoxingVariant)
                {
                    case BoxingVariant.Годная:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipmentDto.ProductId && w.StateProduct == VariantStateProduct.Упаковано).FirstOrDefaultAsync();
                        if ((prSt1.Quantity -= shipmentDto.Quantity) < 0) throw new Exception("Столько не упаковано");
                        break;
                        }

                    case BoxingVariant.Вторичка:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipmentDto.ProductId && w.StateProduct == VariantStateProduct.Вторичка).FirstOrDefaultAsync();
                        if ((prSt1.Quantity -= shipmentDto.Quantity) < 0) throw new Exception("Столько не упаковано");
                        break;
                        }

                    case BoxingVariant.Запаски:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipmentDto.ProductId && w.StateProduct == VariantStateProduct.Запаски).FirstOrDefaultAsync();
                        if ((prSt1.Quantity -= shipmentDto.Quantity) < 0) throw new Exception("Столько не упаковано");
                            break;
                    }
                    default:
                        throw new Exception("Столько не упаковано");
                }
                

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == shipmentDto.ProductId && w.StateProduct == VariantStateProduct.Отгружено).FirstOrDefaultAsync();

                prSt2.Quantity += shipmentDto.Quantity;

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Shipments.Create(shipment);
                await _uof.Commit();
                return new OperationDetails(true, "Пайка успешно добавлена", "/FinishedGoodsWarehouse/ListPartial");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(false, e.Message, ""); ;
            }
        }

        public async Task<IEnumerable<ShipmentDto>> ShowShipmentAsync(string startDate, string endDate)
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

            var s = await _uof.Shipments.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new ShipmentDto()
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    UserName = x.User.UserName,
                    BoxingVariant = x.BoxingVariant
                }).OrderByDescending(x => x.Date).ToListAsync();
            return s;
        }

        public async Task<IEnumerable<ProductStateDto>> PackagedShow() => await _uof.ProductStates.Entities
            .Where(w => w.StateProduct == VariantStateProduct.Упаковано)
            .Select(s => new ProductStateDto
            {
                StateProduct = s.StateProduct,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                Id = s.Id,
                ProductName = s.Product.Name
            }).ToListAsync();

        public async Task<OperationDetails> DeleteShipment(int id)
        {
            try
            {
                var shipment = await _uof.Shipments.GetAsync(id);


                ProductState prSt1;
                switch (shipment.BoxingVariant)
                {
                    case BoxingVariant.Годная:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipment.ProductId && w.StateProduct == VariantStateProduct.Упаковано).FirstOrDefaultAsync();
                        if ((prSt1.Quantity += shipment.Quantity) < 0) throw new Exception("Столько не упаковано");
                        break;
                    }

                    case BoxingVariant.Вторичка:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipment.ProductId && w.StateProduct == VariantStateProduct.Вторичка).FirstOrDefaultAsync();
                        if ((prSt1.Quantity += shipment.Quantity) < 0) throw new Exception("Столько нет");
                        break;
                    }

                    case BoxingVariant.Запаски:
                    {
                        prSt1 = await _uof.ProductStates.Entities.Where(w =>
                            w.ProductId == shipment.ProductId && w.StateProduct == VariantStateProduct.Запаски).FirstOrDefaultAsync();
                        if ((prSt1.Quantity += shipment.Quantity) < 0) throw new Exception("Столько нет");
                        break;
                    }
                    default:
                        throw new Exception("Столько нет");
                }

                var prSt2 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == shipment.ProductId && w.StateProduct == VariantStateProduct.Отгружено).FirstOrDefaultAsync();

                if ((prSt2.Quantity -= shipment.Quantity) < 0) throw new Exception();

                _uof.ProductStates.Update(prSt1);
                _uof.ProductStates.Update(prSt2);

                _uof.Shipments.Delete(id);
                await _uof.Commit();

                return new OperationDetails(true, "Отгрузка успешно удалена", "");
            }
            catch (Exception)
            {
                //_uof.Rollback();
                return new OperationDetails(false, "Отгрузка не удалена", "");
            }
        }
    }
}