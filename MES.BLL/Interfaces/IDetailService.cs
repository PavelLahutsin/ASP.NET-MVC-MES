using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;

namespace MES.BLL.Interfaces
{
    public interface IDetailService
    {
        IEnumerable<DetailDTO> GetDetail(string name);
        List<DetailDTO> GetDetailsJmt();
        Task AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival);
    }
}