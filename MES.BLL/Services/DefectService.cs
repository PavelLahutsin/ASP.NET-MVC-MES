using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class DefectService : IDefectService
    {
        private readonly IUnitOfWork _uof;

        public DefectService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        /// <summary>
        /// Сохраняет данные о браке деталей
        /// </summary>
        /// <param name="defect">данные о браке</param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> AddDefectDetailAsync(DefectDetailDto defect)
        {
            try
            {
                var detail = await _uof.Details.GetAsync(defect.DetailId);
                if (detail == null) throw new Exception();
                var defectDetail = Mapper.Map<DefectDetail>(defect);
                _uof.DefectDetails.Create(defectDetail);

                if ((detail.Quantityq -= defect.Count) < 0) throw new Exception("Нельзя добавить в брак больше, чем есть!");

                _uof.Details.Update(detail);
                await _uof.Commit();
                return new OperationDetails(true, $"{detail.Name} добавлена в брак", "/Defect/HistDefectPartial");
            }
            catch (Exception e)
            {
                _uof.Rollback();
                return new OperationDetails(false, $"{e.Message}", "");
            }

        }

        /// <summary>
        /// Возвращает список брака деталей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DefectDetailDisplayDto>> ShowDefectDetailAsync()
        {
            return await _uof.DefectDetails.Entities.Select(x => new DefectDetailDisplayDto
            {
                Date = x.Date,
                Count = x.Count,
                NameDetail = _uof.Details.Entities.Where(w => w.Id == x.DetailId).Select(s => s.Name).FirstOrDefault(),
                Id = x.Id,
                UserName = x.User.UserName
            }).OrderByDescending(x => x.Date).ToListAsync();
        }


        /// <summary>
        /// Удаление данных о браке деталей
        /// </summary>
        /// <param name="id">Записи для удаления</param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> DeleteDefectDetailAsync(int id)
        {
            try
            {
                var defect = await _uof.DefectDetails.GetAsync(id);
                var detail = await _uof.Details.GetAsync(defect.DetailId);
                if (detail == null) throw new Exception();

                detail.Quantityq += defect.Count;

                _uof.DefectDetails.Delete(id);
                _uof.Details.Update(detail);
                await _uof.Commit();

                return new OperationDetails(true, "Данные удалены", "");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Данные удалены", "");
            }

        }

        /// <summary>
        /// успешна ли операция
        /// </summary>
        /// <param name="defect">измененные данные</param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> EditDefectDetailAsync(DefectDetail defect)
        {
            try
            {
                var oldDefectDetail = await _uof.DefectDetails.GetAsync(defect.Id);
                var oldDetail = await _uof.Details.GetAsync(oldDefectDetail.DetailId);

                if (oldDetail == null) throw new Exception();

                oldDetail.Quantityq += oldDefectDetail.Count;

                var newDetail = await _uof.Details.GetAsync(defect.DetailId);

                if (newDetail == null) throw new Exception();

                if ((newDetail.Quantityq -= defect.Count) < 0) throw new Exception();


                oldDefectDetail.Count = defect.Count;
                oldDefectDetail.Detail = newDetail;
                oldDefectDetail.DetailId = newDetail.Id;
                oldDefectDetail.Date = defect.Date;

                _uof.DefectDetails.Update(oldDefectDetail);
                _uof.Details.Update(oldDetail);
                _uof.Details.Update(newDetail);
                await _uof.Commit();
                return new OperationDetails(true, "Данные изменены", "/Defect/HistDefectPartial");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Данные не изменены", "");
            }

        }
        /// <summary>
        /// Возвращает список деталей группы ЖМТ
        /// </summary>
        /// <returns>список деталей</returns>
        public List<DetailDTO> GetDetailsJmt() => Mapper.Map<IEnumerable<Detail>, List<DetailDTO>>(_uof.Details.Entities.Where(w => w.GroupProduct.Name == "JMT").ToList());

        public async Task<DefectDetailDto> GetDefect(int id) =>
            Mapper.Map<DefectDetailDto>(await _uof.DefectDetails.GetAsync(id));
    }
}