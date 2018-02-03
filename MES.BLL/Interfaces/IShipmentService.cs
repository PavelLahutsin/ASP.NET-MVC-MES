using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IShipmentService
    {
        Task<OperationDetails> AddShipmentAsync(ShipmentDto shipmentDto);
        Task<IEnumerable<ShipmentDto>> ShowShipmentAsync(string startDate, string endDate);
        Task<IEnumerable<ProductStateDto>> PackagedShow();
        Task<OperationDetails> DeleteShipment(int id);
    }
}