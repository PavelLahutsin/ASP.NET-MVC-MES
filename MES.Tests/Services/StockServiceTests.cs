using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MES.DAL.Interfaces;
using MES.BLL.DTO;
using MES.DAL.Entities;
using MES.BLL.Interfaces;
using AutoMapper;

namespace MES.BLL.Services.Tests
{
    [TestClass()]
    public class StockServiceTests
    {
        [TestMethod()]
        public async Task GetDetailTest()
        {
            //Arrange
            Mapper.Initialize(cfg => cfg.CreateMap<Detail, DetailDTO>().ReverseMap());
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Details.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<Detail>(new Detail()));
            var service = new StockService(mock.Object);
            //Act
            var model = await service.GetDetail(2);            

            //Assert

            Assert.IsNotNull(model);
        }
    }
}