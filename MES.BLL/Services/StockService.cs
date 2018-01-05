﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class StockService : IStockService
    {

        private readonly IUnitOfWork _uof;

        public StockService(IUnitOfWork uof)
        {
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
        }
        
       
        /// <summary>
        /// Возвращает сколько деталей расходуется на 1 продукт
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <returns>список деталей</returns>
        public IEnumerable<DetailDTO> GetDetail(string name)
        {
            return _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == name).Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantityq = x.Detail.Quantityq / x.Quantity
            });
            
        }


        /// <summary>
        /// Возвращает список деталей группы ЖМТ
        /// </summary>
        /// <returns>список деталей</returns>
        public List<DetailDTO> GetDetailsJmt() => Mapper.Map<IEnumerable<Detail>, List<DetailDTO>>(_uof.Details.Entities.Where(w => w.GroupProduct.Name == "JMT").ToList());


        /// <summary>
        /// Записывает данные о приходе и добавляет количество деталей на склад
        /// </summary>
        /// <param name="arrival">данные о приходе на склад</param>
        /// <returns>успешна ли операция</returns>
        public async Task<bool> AddArrivalOfDetailAsync(ArrivalOfDetailDto arrival)
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
                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }
            
        }


        /// <summary>
        /// Возвращает список приходов на склад
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DisplayArrivalOfDetailDto>> ShowArryvalOfDedailsAsync()
        {
            return await _uof.ArrivalOfDetails.Entities.Select(x => new DisplayArrivalOfDetailDto
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
        public async Task<bool> DeleteArrivalOfDetailAsync(int id)
        {
            try
            {
                var arrival = await _uof.ArrivalOfDetails.GetAsync(id);
                var detail = await _uof.Details.GetAsync(arrival.DetailId);
                if (detail == null) throw new Exception();

                if ((detail.Quantityq -= arrival.Count) < 0) return false;

                _uof.ArrivalOfDetails.Delete(id);
                _uof.Details.Update(detail);
                await _uof.Commit();

                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }
           
        }

        /// <summary>
        /// Редактирует данные о приходе и обновляет количество деталей на складе
        /// </summary>
        /// <param name="arrival">Обнавленные данные о приходе</param>
        /// <returns>успешно ли обнавление бд</returns>
        public async Task<bool> EditArrivalOfDetailAsync(ArrivalOfDetail arrival)
        {
            try
            {
                var oldArrival = await _uof.ArrivalOfDetails.GetAsync(arrival.Id);
                var oldDetail = await _uof.Details.GetAsync(oldArrival.DetailId);

                if (oldDetail == null) throw new Exception();
                
                if ((oldDetail.Quantityq -= oldArrival.Count) < 0) return false;

                var newDetail = await _uof.Details.GetAsync(arrival.DetailId);

                if (newDetail == null) throw new Exception();
                
                if ((newDetail.Quantityq += arrival.Count) < 0) return false;


                oldArrival.Count = arrival.Count;
                oldArrival.Detail = newDetail;
                oldArrival.DetailId = newDetail.Id;
                oldArrival.Date = arrival.Date;
                
                _uof.ArrivalOfDetails.Update(oldArrival);
                _uof.Details.Update(oldDetail);
                _uof.Details.Update(newDetail);
                await _uof.Commit();
                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }

        }

        /// <summary>
        /// Сохраняет данные о браке деталей
        /// </summary>
        /// <param name="defect">данные о браке</param>
        /// <returns>успешна ли операция</returns>
        public async Task<bool> AddDefectDetailAsync(DefectDetailDto defect)
        {
            try
            {
                var detail = await _uof.Details.GetAsync(defect.DetailId);
                if (detail == null) throw new Exception();
                var defectDetail = Mapper.Map<DefectDetail>(defect);
                _uof.DefectDetails.Create(defectDetail);

                if ((detail.Quantityq -= defect.Count) < 0) return false;

                _uof.Details.Update(detail);
                await _uof.Commit();
                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
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
                Id = x.Id
            }).ToListAsync();
        }


        /// <summary>
        /// Удаление данных о браке деталей
        /// </summary>
        /// <param name="id">Записи для удаления</param>
        /// <returns>успешна ли операция</returns>
        public async Task<bool> DeleteDefectDetailAsync(int id)
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

                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }

        }

        /// <summary>
        /// успешна ли операция
        /// </summary>
        /// <param name="defect">измененные данные</param>
        /// <returns>успешна ли операция</returns>
        public async Task<bool> EditDefectDetailAsync(DefectDetail defect)
        {
            try
            {
                var oldDefectDetail = await _uof.DefectDetails.GetAsync(defect.Id);
                var oldDetail = await _uof.Details.GetAsync(oldDefectDetail.DetailId);

                if (oldDetail == null) throw new Exception();

                oldDetail.Quantityq += oldDefectDetail.Count;

                var newDetail = await _uof.Details.GetAsync(defect.DetailId);

                if (newDetail == null) throw new Exception();

                if ((newDetail.Quantityq -= defect.Count) < 0) return false;


                oldDefectDetail.Count = defect.Count;
                oldDefectDetail.Detail = newDetail;
                oldDefectDetail.DetailId = newDetail.Id;
                oldDefectDetail.Date = defect.Date;

                _uof.DefectDetails.Update(oldDefectDetail);
                _uof.Details.Update(oldDetail);
                _uof.Details.Update(newDetail);
                await _uof.Commit();
                return true;
            }
            catch (Exception)
            {
                _uof.Rollback();
                return false;
            }

        }
    }

}