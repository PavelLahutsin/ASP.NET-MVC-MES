using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.DAL.Entities;

namespace MES.BLL.Interfaces
{
    public interface IStockService
    {
        IEnumerable<DetailDTO> GetDetail(string name);
        List<DetailDTO> GetDetailsJmt();
        Task<bool> AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival);
        Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedails();
        Task<bool> DeleteArrivalOfDetail(int id);
        Task<bool> EditArrivalOfDetail(ArrivalOfDetail arrival);
        Task<bool> AddDefectDetailAsync(DefectDetailDto defect);
        Task<IEnumerable<DefectDetailDisplayDto>> ShowDefectDetailAsync();
        Task<bool> DeleteDefectDetail(int id);
        Task<bool> EditDefectDetail(DefectDetail defect);
    }
}