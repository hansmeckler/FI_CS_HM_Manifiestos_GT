using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Manifiesto.Data.Aereo;
using Manifiesto.Web.Areas.Aereo.Models;
using Manifiesto.Web.Areas.Aereo.Controllers;
using System.Web.Mvc;

namespace Manifiesto.Web.Tests.Aereo
{
    /// <summary>
    /// Summary description for ImportControllerTest
    /// </summary>
    [TestClass]
    public class ImportControllerTest
    {
        
        [TestMethod]
        public void Index_Return_NotNull()
        {
            var mock = new Mock<IAereoService>();

            mock.Setup(f => f.getGuiasImportList()).Returns(new List<guiasListViewModel>
            {
                new guiasListViewModel() { id = 1, awbnumber = "R0038916", hawbnumber = "R0038916", bls = 1 },
                new guiasListViewModel() { id = 2, awbnumber = "CC23061300245", hawbnumber = "CC23061300245", bls = 1  }
            });

            var controller = new ImportController(mock.Object);

            var result = controller.Index();
            var rresult = (ViewResult)result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(rresult, typeof(ViewResult));
            Assert.IsInstanceOfType(rresult.Model, typeof(IEnumerable<guiasListViewModel>));

            mock.VerifyAll();
        }
    }
}
