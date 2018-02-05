﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _uof;

        public AnalyticsService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<IEnumerable<DetailDTO>> GetDetailOnProduct(string name)
        {
            return await _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == name).Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantityq = x.Detail.Quantityq / x.Quantity
            }).ToListAsync();

        }

        public async Task<IEnumerable<SolderingCountDto>> ShowSolderingsCountAsync(string startDate, string endDate)
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
            return await _uof.Solderings.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).GroupBy(x => x.ProductId).Select(s => new SolderingCountDto()
            {
                Quantity = s.Sum(x => x.Quantity),
                ProductName = s.FirstOrDefault().Product.Name
            }).OrderBy(x => x.ProductName).ToListAsync();

        }

        public async Task<ChekDetailsDto> ShowCheckInfo(string startDate, string endDate)
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

            return await _uof.CheckJmts.Entities.Where(w =>
                    w.Date >= myStartDate && w.Date <= myEndDate && w.State == StateFoTest.Новые && w.Product.Name == "5200-01")
                .GroupBy(dt => dt.ProductId)
                .Select(x => new ChekDetailsDto()
                {
                    Airtight = x.Sum(s => s.Count),
                    Defect = x.Sum(s => s.Defect),
                    Housing = x.Sum(s => s.Housing),
                    Other = x.Sum(s => s.Other),
                    Tube = x.Sum(s => s.Tube),
                    CapN = x.Sum(s => s.CapN),
                    CapM = x.Sum(s => s.CapM),
                    Center = x.Sum(s => s.Center)
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ShipmentChartDto>> ShowShipmentAsync(string startDate, string endDate)
        {
            DateTime myEndDate;
            DateTime myStartDate;
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                myEndDate = DateTime.Now;
                myStartDate = new DateTime(myEndDate.Year-1);
            }
            else
            {
                myEndDate = DateTime.Parse(endDate);
                myStartDate = DateTime.Parse(startDate);
            }
            
                var s =  await _uof.Shipments.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                    new ShipmentChartDto()
                    {
                        ProducName = x.Product.Name,
                        Quantity = x.Quantity,
                        Date = x.Date
                    }).OrderBy(x => x.Date).ToListAsync();

            var list = new List<ShipmentChartDto>();
            foreach (var dto in s)
            {
                list.Add(new ShipmentChartDto
                {
                    ProducName = dto.ProducName,
                    Quantity = dto.Quantity,
                    Date = new DateTime(dto.Date.Year, dto.Date.Month, 1)
                });
            }

            var list2 = list.GroupBy(x => x.Date).Select(x=>new ShipmentChartDto
            {
                ProducName = x.First().ProducName,
                Quantity = x.Sum(q=>q.Quantity),
                Date = x.First().Date
            });

           
            return list2;
        }
    }
}