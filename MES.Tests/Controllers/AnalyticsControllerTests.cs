using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.WEB.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MES.DAL.Interfaces;
using MES.BLL.Interfaces;
using System.Web.Mvc;

namespace MES.WEB.Controllers.Tests
{
    [TestClass()]
    public class AnalyticsControllerTests
    {

        private AnalyticsController controller;
        private ViewResult result;


        [TestInitialize]
        public void SetupContext()
        {
            var mockServece = new Mock<IAnalyticsService>();
            var mockUof = new Mock<IUnitOfWork>();
            controller = new AnalyticsController(mockServece.Object, mockUof.Object);
        }

        [TestMethod()]
        public void IndexTest()
        {           
            // Act
            result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

     

        [TestMethod()]
        public async Task RemainingDetailsTest()
        {
            PartialViewResult result = await controller.RemainingDetails() as PartialViewResult;
            Assert.AreEqual("_RemainingDetails", result.ViewName);
        }

        [TestMethod()]
        public async Task RemainingDetailsIndexTest()
        {
            PartialViewResult result = await controller.RemainingDetailsIndex() as PartialViewResult;
            Assert.AreEqual("_RemainingDetailsIndex", result.ViewName);
        }

        [TestMethod()]
        public async Task HowManySolderedTest()
        {
            PartialViewResult result = await controller.HowManySoldered(null, null) as PartialViewResult;
            Assert.AreEqual("_HowManySoldered", result.ViewName);
        }

        [TestMethod()]
        public async Task HowManySolderedModalTest()
        {
            PartialViewResult result = await controller.HowManySolderedModal(null, null) as PartialViewResult;
            Assert.AreEqual("_HowManySolderedModal", result.ViewName);
        }

        [TestMethod()]
        public async Task CheckInfoTest()
        {
            PartialViewResult result = await controller.CheckInfo(null, null) as PartialViewResult;
            Assert.AreEqual("_CheckInfo", result.ViewName);
        }

        [TestMethod()]
        public async Task CheckInfoModalTest()
        {
            PartialViewResult result = await controller.CheckInfoModal(null, null) as PartialViewResult;
            Assert.AreEqual("_CheckInfoModal", result.ViewName);
        }

        [TestMethod()]
        public async Task ShipmentChartTest()
        {
            PartialViewResult result = await controller.ShipmentChart(null, null) as PartialViewResult;
            Assert.AreEqual("_ShipmentChart", result.ViewName);
        }

        [TestMethod()]
        public async Task ShipmentChartModalTest()
        {
            PartialViewResult result = await controller.ShipmentChartModal(null, null) as PartialViewResult;
            Assert.AreEqual("_ShipmentChartModal", result.ViewName);
        }
    }
}