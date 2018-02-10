using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IAdminService
    {
        Task<OperationDetails> CreateDetail(DetailDTO detailDto);
        Task<OperationDetails> CreateProduct(ProductDto productDto);
        Task<OperationDetails> CreateStructProduct(DetailInProductDto dto);
        Task<IEnumerable<ProductDto>> ListProduct();
        Task<IEnumerable<DetailInProductDto>> ListStructOfTheProduct(int id);
        Task<OperationDetails> DeleteDetailOnStructProduct(int detailid, int productId);
        Task<OperationDetails> DeleteProduct(int id);
    }
}