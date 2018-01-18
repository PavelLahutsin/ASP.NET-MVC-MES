using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IFinishedGoodsWarehouseService
    {
        Task<IEnumerable<ProductStateDto>> PackagedList();
        Task<OperationDetails> AddShipping(ProductStateDto productState);
        Task<OperationDetails> DeleteShipping(int id);

    }
}