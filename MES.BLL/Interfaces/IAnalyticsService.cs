using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;

namespace MES.BLL.Interfaces
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<DetailDTO>> GetDetailOnProduct(string name);
        Task<IEnumerable<SolderingCountDto>> ShowSolderingsCountAsync(string startDate, string endDate);
        Task<ChekDetailsDto> ShowCheckInfo(string startDate, string endDate);
        Task<IEnumerable<ShipmentChartDto>> ShowShipmentAsync(string startDate, string endDate);
        Task<IEnumerable<QualityIndicators>> QualityIndicators520001(string startDate, string endDate);
    }
}