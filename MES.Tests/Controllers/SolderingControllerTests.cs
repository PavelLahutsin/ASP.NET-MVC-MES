using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.WEB.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MES.BLL.Interfaces;
using Moq;
using MES.DAL.Interfaces;

namespace MES.WEB.Controllers.Tests
{
    [TestClass()]
    public class SolderingControllerTests
    {

        private SolderingController controller;
        private ViewResult result;
       

        [TestInitialize]
        public void SetupContext()
        {
            var mockServece = new Mock<ISolderingService>();
            var mockUof = new Mock<IUnitOfWork>();
            controller = new SolderingController(mockServece.Object, mockUof.Object);
            result = controller.Soldering() as ViewResult;
        }

        [TestMethod]
        public void SolderingViewResultNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SolderingViewModelNotNull()
        {
            Assert.IsNotNull(result.Model);
        }
       
    }
}