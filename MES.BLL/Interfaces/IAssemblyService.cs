using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IAssemblyService
    {
        Task<OperationDetails> AddAssemblyAsync(AssemblyDto assembly);
        Task<IEnumerable<AssemblyDto>> ShowAssemblysAsync(string startDate, string endDate);
        Task<OperationDetails> DeleteAssembly(int id);
    }
}