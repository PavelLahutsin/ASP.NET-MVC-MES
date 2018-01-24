using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.DAL.Entities;

namespace MES.BLL.Interfaces
{
    public interface IArrivalService
    {
        Task<OperationDetails> AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival);
        Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedailsAsync(string startDate, string endDate);
        Task<OperationDetails> EditArrivalOfDetailAsync(ArrivalOfDetail arrival);
        List<DetailDTO> GetDetailsJmt();
        Task<OperationDetails> DeleteArrivalOfDetailAsync(int id);
    }
}