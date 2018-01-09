using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;

namespace MES.BLL.Interfaces
{
    public interface ISolderingService
    {
        Task<OperationDetails> AddSolderingAsync(SolderingDto soldering);
        Task<IEnumerable<SolderingDto>> ShowSolderingsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SolderingCountDto>> ShowSolderingsCountAsync(DateTime startDate, DateTime endDate);
        Task<bool> DeleteSoldering(int id);
    }
}