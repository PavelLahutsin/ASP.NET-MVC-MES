using System.Threading.Tasks;
using MES.BLL.DTO;

namespace MES.BLL.Interfaces
{
    public interface ISolderingService
    {
        Task<bool> AddSolderingAsync(SolderingDto soldering);
    }
}