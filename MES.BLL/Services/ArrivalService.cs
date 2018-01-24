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
    public class ArrivalService : IArrivalService
    {
        private readonly IUnitOfWork _uof;

        public ArrivalService(IUnitOfWork uof)
        {
            _uof = uof;
        }


        /// <summary>
        /// Записывает данные о приходе и добавляет количество деталей на склад
        /// </summary>
        /// <param name="arrival">данные о приходе на склад</param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival)
        {
            try
            {
                var detail = await _uof.Details.GetAsync(arrival.DetailId);
                if (detail == null) throw new Exception();
                var arrivalOfDetail = Mapper.Map<ArrivalOfDetail>(arrival);
                _uof.ArrivalOfDetails.Create(arrivalOfDetail);

                detail.Quantityq += arrival.Count;
                _uof.Details.Update(detail);
                await _uof.Commit();
                return new OperationDetails(true, $"{detail.Name} успешно добавлена", "/Arrival/HistArrivalPartial");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Данные не удалось добавить", "");
            }

        }


        /// <summary>
        /// Возвращает список приходов на склад
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedailsAsync(string startDate, string endDate)
        {
            DateTime myEndDate;
            DateTime myStartDate;
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                myEndDate = DateTime.Now;
                myStartDate = new DateTime(myEndDate.Year, myEndDate.Month, 1);
            }
            else
            {
                myEndDate = DateTime.Parse(endDate);
                myStartDate = DateTime.Parse(startDate);
            }
            return await _uof.ArrivalOfDetails.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x => new DisplayArrivalOfDetailDto
            {
                Date = x.Date,
                Count = x.Count,
                NameDetail = _uof.Details.Entities.Where(w => w.Id == x.DetailId).Select(s => s.Name).FirstOrDefault(),
                Id = x.Id
            }).ToListAsync();
        }

        /// <summary>
        /// Удаление данных о приходе со склада
        /// </summary>
        /// <param name="id">ArrivalOfDetail.Id</param>
        /// <returns>успешна ли операция</returns>
        public async Task<OperationDetails> DeleteArrivalOfDetailAsync(int id)
        {
            try
            {
                var arrival = await _uof.ArrivalOfDetails.GetAsync(id);
                var detail = await _uof.Details.GetAsync(arrival.DetailId);
                if (detail == null) throw new Exception();

                if ((detail.Quantityq -= arrival.Count) < 0) throw new Exception();

                _uof.ArrivalOfDetails.Delete(id);
                _uof.Details.Update(detail);
                await _uof.Commit();

                return new OperationDetails(true, "Данные успешно удалены", "");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Данные не удалены", "");
            }

        }

        /// <summary>
        /// Редактирует данные о приходе и обновляет количество деталей на складе
        /// </summary>
        /// <param name="arrival">Обнавленные данные о приходе</param>
        /// <returns>успешно ли обнавление бд</returns>
        public async Task<OperationDetails> EditArrivalOfDetailAsync(ArrivalOfDetail arrival)
        {
            try
            {
                var oldArrival = await _uof.ArrivalOfDetails.GetAsync(arrival.Id);
                var oldDetail = await _uof.Details.GetAsync(oldArrival.DetailId);

                if (oldDetail == null) throw new Exception();

                if ((oldDetail.Quantityq -= oldArrival.Count) < 0) throw new Exception();

                var newDetail = await _uof.Details.GetAsync(arrival.DetailId);

                if (newDetail == null) throw new Exception();

                if ((newDetail.Quantityq += arrival.Count) < 0) throw new Exception();


                oldArrival.Count = arrival.Count;
                oldArrival.Detail = newDetail;
                oldArrival.DetailId = newDetail.Id;
                oldArrival.Date = arrival.Date;

                _uof.ArrivalOfDetails.Update(oldArrival);
                _uof.Details.Update(oldDetail);
                _uof.Details.Update(newDetail);
                await _uof.Commit();
                return new OperationDetails(true, "Данные успешно изменены", "/Arrival/HistArrivalPartial");
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
    }
}