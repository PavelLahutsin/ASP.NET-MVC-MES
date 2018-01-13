using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.DAL.Entities;

namespace MES.BLL.Interfaces
{
    public interface IArrivalService
    {
        Task<bool> AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival);
        Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedailsAsync(string startDate, string endDate);
        Task<bool> DeleteArrivalOfDetailAsync(int id);
        Task<bool> EditArrivalOfDetailAsync(ArrivalOfDetail arrival);
        List<DetailDTO> GetDetailsJmt();
    }
}