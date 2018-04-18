using System;
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



            var check = await _uof.CheckJmts.Entities.Where(w =>
                    w.Date >= myStartDate && w.Date <= myEndDate && w.State == StateFoTest.Новые &&
                    w.Product.Name == "5200-01")
                .GroupBy(dt => dt.ProductId)
                .Select(x => new ChekDetailsDto()
                {
                    Airtight = x.Sum(s => s.Airtight),
                    Defect = x.Sum(s => s.Defect),
                    Housing = x.Sum(s => s.Housing),
                    Other = x.Sum(s => s.Other),
                    Tube = x.Sum(s => s.Tube),
                    CapN = x.Sum(s => s.CapN),
                    CapM = x.Sum(s => s.CapM),
                    Center = x.Sum(s => s.Center)
                }).FirstOrDefaultAsync();

            if (check != null) return check;
            return new ChekDetailsDto()
            {
                Airtight = 0,
                Defect = 0,
                Housing = 0,
                Other = 0,
                Tube = 0,
                CapN = 0,
                CapM = 0,
                Center = 0
            };
        }

        public async Task<IEnumerable<ShipmentChartDto>> ShowShipmentAsync(string startDate, string endDate)
        {
            DateTime myEndDate;
            DateTime myStartDate;
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                myEndDate = DateTime.Now;
                myStartDate = new DateTime(myEndDate.Year, myEndDate.Month, 1).AddYears(-1);
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

            

            var list2 = list.GroupBy(x => new {x.ProducName, x.Date}).Select(x=>new ShipmentChartDto
            {
                ProducName = x.First().ProducName,
                Quantity = x.Sum(q=>q.Quantity),
                Date = x.First().Date
            }).ToList();

            var date = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            var date2 = new DateTime(myEndDate.Year, myEndDate.Month, 1);
            var prod = _uof.Products.Entities.Select(x => x.Name);
            var shpList = new List<ShipmentChartDto>();
            for (DateTime i = date; i <= date2; i=i.AddMonths(1))
            {
                foreach (string p in prod)
                {
                    if (list.Any(a => a.ProducName == p && a.Date == i))
                    {
                        shpList.Add(list2.First(a => a.ProducName == p && a.Date == i));
                    }
                    else
                    {
                        shpList.Add(new ShipmentChartDto
                        {
                            ProducName = p,
                            Date = i,
                            Quantity = 0
                        });
                    }
                }


            }
            
            return shpList;
        }
    }
}