using System.Collections.Generic;
using MES.BLL.DTO;

namespace MES.BLL.Interfaces
{
    public interface IDetailService
    {
        IEnumerable<DetailDTO> GetDetail(string name);
    }
}