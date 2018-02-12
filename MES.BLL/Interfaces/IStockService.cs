using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.DAL.Entities;

namespace MES.BLL.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<DetailDTO>> GetDetailProduct(string name);
        List<DetailDTO> GetDetailsJmt();
        Task<OperationDetails> CreateDetail(DetailDTO detailDto);
        Task<DetailDTO> GetDetail(int id);
        Task<OperationDetails> EditDetail(DetailDTO detailDto);
        Task<OperationDetails> DeleteDetailAsync(int id);
    }
}