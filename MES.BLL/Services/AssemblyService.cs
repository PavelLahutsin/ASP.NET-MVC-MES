﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Entities;
using MES.DAL.Enums;
using MES.DAL.Interfaces;

namespace MES.BLL.Services
{
    public class AssemblyService : IAssemblyService
    {
        private readonly IUnitOfWork _uof;

        public AssemblyService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OperationDetails> AddAssemblyAsync(AssemblyDto assembly)
        {
            try
            {
                var structureOfTheProducts = _uof.StructureOfTheProducts.Entities
                    .Where(w => w.ProductId == assembly.ProductId).ToList();

                foreach (var structureOfTheProduct in structureOfTheProducts)
                {
                    var detail = await _uof.Details.GetAsync(structureOfTheProduct.DetailId);
                    if ((detail.Quantityq -= (structureOfTheProduct.Quantity * assembly.Quantity)) < 0) throw new Exception($"Число деталей ({detail.Name}) на складе не может быть отрицательным");
                    _uof.Details.Update(detail);
                }
                var ass = new Assembly()
                {
                    ProductId = assembly.ProductId,
                    Quantity = assembly.Quantity,
                    Date = assembly.Date,
                    UserId = assembly.UserId
                };

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == assembly.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                prSt1.Quantity += assembly.Quantity;

                

                _uof.ProductStates.Update(prSt1);
                

                _uof.Assemblys.Create(ass);
                
                await _uof.Commit();
                await CheckStock();
                return new OperationDetails(true, "Сборка успешно добавлена", "/Assembly/ListPartial");
            }
            catch (Exception e)
            {
                //_uof.Rollback();
                return new OperationDetails(false, e.Message, ""); ;
            }
        }

        public async Task<IEnumerable<AssemblyDto>> ShowAssemblysAsync(string startDate, string endDate)
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

            var s = await _uof.Assemblys.Entities.Where(w => w.Date >= myStartDate && w.Date <= myEndDate).Select(x =>
                new AssemblyDto()
                {
                    Quantity = x.Quantity,
                    Date = x.Date,
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    UserId = x.UserId,
                    UserName = x.User.UserName
                }).OrderByDescending(x => x.Date).ToListAsync();
            return s;
        }

        public async Task<OperationDetails> DeleteAssembly(int id)
        {
            try
            {
                var assembly = await _uof.Assemblys.GetAsync(id);
                var structureOfTheProducts = _uof.StructureOfTheProducts.Entities
                    .Where(w => w.ProductId == assembly.ProductId).ToList();

                foreach (var structureOfTheProduct in structureOfTheProducts)
                {
                    var detail = await _uof.Details.GetAsync(structureOfTheProduct.DetailId);
                    detail.Quantityq += (structureOfTheProduct.Quantity * assembly.Quantity);
                    _uof.Details.Update(detail);
                }

                var prSt1 = await _uof.ProductStates.Entities.Where(w =>
                    w.ProductId == assembly.ProductId && w.StateProduct == VariantStateProduct.Собрано).FirstOrDefaultAsync();

                if ((prSt1.Quantity -= assembly.Quantity)<0) throw new Exception();

               

                _uof.ProductStates.Update(prSt1);

                _uof.Assemblys.Delete(id);
                await _uof.Commit();

                return new OperationDetails(true, "Сборка успешно удалена", "");
            }
            catch (Exception)
            {
                _uof.Rollback();
                return new OperationDetails(false, "Сборка не удалена", "");
            }
        }

        private async Task CheckStock()
        {
            var details = await _uof.StructureOfTheProducts.Entities.Where(w => w.Product.Name == "5200-01").Select(x => new DetailDTO
            {
                Name = x.Detail.Name,
                GroupProductId = x.Detail.GroupProductId,
                Quantityq = x.Detail.Quantityq / x.Quantity,
                VendorCode = x.Detail.VendorCode
            }).ToListAsync();

            
            var str = new StringBuilder();
            foreach (var dto in details)
            {
                if (dto.Quantityq > 500) continue;
                str.AppendLine($"{dto.Name} ({dto.VendorCode}) осталось на {dto.Quantityq} теплообменника\n");
               
            }

            if (str.Length>0)
            {
                //await SendEmailAsync(str.ToString());
            }
                        
        }

        private async Task SendEmailAsync(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var from = new MailAddress("glade@zavet.ga", "Склад");
                var to = new MailAddress("pavelvasilevich@gmail.com");
                var m = new MailMessage(@from, to)
                {
                    Subject = "Детали заканчивающиеся на складе",
                    Body = str
                };
                m.To.Add("ooozavet@bk.ru");
                var smtp = new SmtpClient("mail.zavet.ga", 8889)
                {
                    Credentials = new NetworkCredential("glade@zavet.ga", "_7553311df"),

                };
                await smtp.SendMailAsync(m);
            }
        }
    }
}
