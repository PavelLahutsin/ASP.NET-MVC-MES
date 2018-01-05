﻿using System.Collections.Generic;
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
        Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedailsAsync();
        Task<bool> DeleteArrivalOfDetailAsync(int id);
        Task<bool> EditArrivalOfDetailAsync(ArrivalOfDetail arrival);
        Task<bool> AddDefectDetailAsync(DefectDetailDto defect);
        Task<IEnumerable<DefectDetailDisplayDto>> ShowDefectDetailAsync();
        Task<bool> DeleteDefectDetailAsync(int id);
        Task<bool> EditDefectDetailAsync(DefectDetail defect);
    }
}