using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.DAL.Entities;

namespace MES.BLL.Interfaces
{
    public interface IDefectService
    {
        Task<OperationDetails> AddDefectDetailAsync(DefectDetailDto defect);
        Task<IEnumerable<DefectDetailDisplayDto>> ShowDefectDetailAsync();
        Task<OperationDetails> DeleteDefectDetailAsync(int id);
        Task<OperationDetails> EditDefectDetailAsync(DefectDetail defect);
        List<DetailDTO> GetDetailsJmt();
        Task<DefectDetailDto> GetDefect(int id);
    }
}