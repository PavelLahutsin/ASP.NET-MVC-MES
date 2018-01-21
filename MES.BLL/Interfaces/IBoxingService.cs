using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IBoxingService
    {
        Task<OperationDetails> AddBoxingAsync(BoxingDto boxing);
        Task<IEnumerable<BoxingDto>> ShowBoxingsAsync(string startDate, string endDate);
        Task<OperationDetails> DeleteBoxing(int id);
    }
}