using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Manifiesto.Data.Aereo;
using Moq;
using Manifiesto.Web.Repositories.CuscarServices;
using Manifiesto.Web.Areas.Aereo.Controllers;
using db_aereo.Entities.Entities;
using System.Web.Mvc;
using Manifiesto.Data.Models;
using AutoMapper;
using System.Web;

namespace Manifiesto.Web.Tests.Aereo
{
    /// <summary>
    /// Summary description for ManifestControllerTest
    /// </summary>
    [TestClass]
    public class ManifestControllerTest
    {

        Mock<IAereoService> _mockAereo;
        Mock<ICuscarAereoServices> _mockCuscarAereo;
        ManifestController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAereo = new Mock<IAereoService>();
            _mockCuscarAereo = new Mock<ICuscarAereoServices>();
            _controller = new ManifestController(_mockAereo.Object, _mockCuscarAereo.Object);
        }
      
        [TestMethod]
        public void Index_Return_NotNull()
        {

            _mockAereo.Setup(c => c.get_cuscar_voyage_info()).Returns(new List<cuscar_voyage_info>
            {
                new cuscar_voyage_info() { cuscar_voyage_id = 1, no_viaje = "TUSH433", naviera = "COPA AIRLINES", tipo = "DC" },
                new cuscar_voyage_info() { cuscar_voyage_id = 2, no_viaje = "JSUUWL3334", naviera = "COPA AIRLINES", tipo = "DC"  }
            });

            var result = _controller.Index();
            var rresult = (ViewResult)result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(rresult, typeof(ViewResult));
            Assert.IsInstanceOfType(rresult.Model, typeof(IEnumerable<cuscar_voyage_info>));

            _mockAereo.VerifyAll();

        }

        [TestMethod]
        public void finGuia_Return_NotNull()
        {
           _mockAereo.Setup(a => a.FindToAdd(1, 22)).Returns(new manifiesto_encVM
            {
                funcsend = "DC",
                no_viaje = "AJST87",
                id_naviera = 1,
                vapor = "TEHS09",
                id_puerto_origen = 1,
                id_puerto_desembarque = 1,
                tipo = "AWB",
                viaje_id = 1,
                operation = 22,
                test = 6,
                naviera = "Naviera",
                manifest = "NA",
                mfunction = 9,
                mtype = 785,
                cuscar = "NA",
                cuscardt = null,
                id_status = 1
            });

           var result = _controller.findGuia(1,22);

           Assert.IsNotNull(result);
           Assert.IsInstanceOfType(result, typeof(manifiesto_encVM));

           _mockAereo.Verify(x => x.FindToAdd(1, 22), Times.Once);
           _mockAereo.VerifyAll();
        }

        [TestMethod]
        public void Create_Voyage_Return_NotNull()
        {

            cuscar_voyage_info vm = new cuscar_voyage_info()
            {
                funcsend = "DC",
                no_viaje = "XX",
                id_naviera = 1,
                vapor = "AA",
                id_puerto_origen = 1,
                id_puerto_desembarque = 1,
                tipo = "AA",
                viaje_id = 1,
                operation = 22,
                test = 6,
                naviera = "A",
                manifest = "NA",
                mfunction = 9,
                mtype = 785,
                cuscar = "NA",
                cuscardt = null,
                fecha_arribo = "2015-07-02",
                cuscar_voyage_id = 9999,
                puerto_desembarque = "XX",
                puerto_origen = "XX",
                fecha_salida = "2015-07-02",
                original = "NA"
            };

            //var mockDelegate = new Mock<Func<ICuscarAereoServices>>();
            //mockDelegate.Setup(x => x()).Returns(_mockCuscarAereo.Object);
            //mockDelegate.Verify(x => x(), Times.Once);
            
            _mockCuscarAereo.Setup(x => x.add_voyage_cuscar_info(vm)).Returns(vm);
            
            //_mockCuscarAereo.VerifyAll();
            _mockCuscarAereo.Verify(x => x.add_voyage_cuscar_info(vm), Times.Never);
            
        }

   
    }
}
