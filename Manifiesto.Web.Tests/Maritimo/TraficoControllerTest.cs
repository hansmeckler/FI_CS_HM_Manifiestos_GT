using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Manifiesto.Data.Trafico;
using Manifiesto.Data.Models;
using Manifiesto.Web.Areas.Maritimo.Controllers;
using System.Web.Mvc;
using ventas_gt.Entities.Entities;

namespace Manifiesto.Web.Tests.Maritimo
{
    /// <summary>
    /// Summary description for TraficoControllerTest
    /// </summary>
    [TestClass]
    public class TraficoControllerTest
    {

        Mock<ICuscarMaritimoServices> _mock;
        TraficoController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mock = new Mock<ICuscarMaritimoServices>();
            _controller = new TraficoController(_mock.Object);
        }


        [TestMethod]
        public void Index_Return_NotNull()
        {

            var result = _controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var vresult = (ViewResult)result;

            Assert.IsNotNull(vresult.Model);
            Assert.IsInstanceOfType(vresult.Model, typeof(IEnumerable<listado_bls_VM>));

        }
     
        [TestMethod]
        public void Index_Return_WithData()
        {
            _mock.Setup(f => f.getListBls()).Returns(new List<listado_bls_VM>
            {
                new listado_bls_VM() { id = 1, fecha_arribo = DateTime.Now, no_bl = "1", tipo = "LCL", no_viaje = "JAXS5M138935", vapor = "xxx" },
                new listado_bls_VM() { id = 2, fecha_arribo = DateTime.Now, no_bl = "2", tipo = "FCL", no_viaje = "JAXS5M138935", vapor = "xxx" }
            });

            var result = _controller.Index();
            var vresult = (ViewResult)result;
            var list = (IEnumerable<listado_bls_VM>)vresult.Model;

            Assert.AreEqual(list.Count(), 2);
            Assert.IsNotNull(vresult.Model);
            Assert.IsInstanceOfType(vresult.Model, typeof(IEnumerable<listado_bls_VM>));
            Assert.AreNotEqual(0, list.Count());
        }
        
        [TestMethod]
        public void Create_Return_View()
        {
            int id = 1;
            string tipo_servicio = "FCL";

            var result = _controller.Create(id, tipo_servicio);
            var vresult = (ViewResult)result;

        }
        
        
        [TestMethod]
        public void Create_Return_WithData()
        {

            int id = 35;
            string tipo_servicio = "FCL";
            bool r = true;

            _mock.Setup(f => f.validate_size_of_name_commodities(id, tipo_servicio)).Returns(r);

            //var result = _controller.Create(id, tipo_servicio);
            //var vresult = (ViewResult)result;

            //Assert.IsNotNull(vresult.Model);
            //Assert.IsInstanceOfType(vresult.Model, typeof(bool));

            _mock.VerifyAll();
        }
        
    }
}
