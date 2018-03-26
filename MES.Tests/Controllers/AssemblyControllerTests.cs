using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.WEB.Models;
using MES.BLL.Interfaces;
using MES.DAL.Interfaces;
using Moq;
using System.Web.Mvc;

namespace MES.WEB.Controllers.Tests
{
    [TestClass()]
    public class AssemblyControllerTests
    {
        [TestMethod()]
        public void IndexViewModelNotNull()
        {
            // arrange
            
            var mockServece = new Mock<IAssemblyService>();
            var mockUof = new Mock<IUnitOfWork>();
            AssemblyVm assembly = new AssemblyVm();
            AssemblyController controller = new AssemblyController(mockServece.Object, mockUof.Object);
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }
    }
}