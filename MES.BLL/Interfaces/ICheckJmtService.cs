using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface ICheckJmtService
    {
        Task<OperationDetails> AddCheckJmtAsync(CheckJmtDto checkJmtDto);
        Task<IEnumerable<CheckJmtForListDto>> ShowCheckJmtNewAsync(string startDate, string endDate);
        Task<IEnumerable<CheckJmtForListDto>> ShowCheckJmtOldAsync(string startDate, string endDate);
        Task<OperationDetails> DeleteCheck(int id);
    }
}