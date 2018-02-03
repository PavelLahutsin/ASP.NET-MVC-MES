using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface IRepairService
    {
        Task<OperationDetails> AddRepairAsync(RepairDto repairDto);
        Task<IEnumerable<RepairDto>> ListRepairAsync(string startDate, string endDate);
        Task<OperationDetails> DeleteRepair(int id);
    }
}