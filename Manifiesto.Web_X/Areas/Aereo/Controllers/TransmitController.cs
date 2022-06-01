using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Web.Areas.Aereo.Models;

namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    public class TransmitController : Controller
    {
        //
        // GET: /Aereo/Transmit/

        public ActionResult Index(string nombreArchivo, string id_empresa)
        {
            Transmit transmit = new Transmit();
            if (id_empresa == "GT")
            {
                transmit.usuario = "12105562";
                transmit.password = "GTAim2022";
            }
            else if (id_empresa == "GTLTF")
            {
                transmit.usuario = "41171829";
                transmit.password = "Latin2016";
            }
            transmit.nombreArchivo = nombreArchivo;
            transmit.procesamientoSincrono = false;
            transmit.respuestaXml = true;
            transmit.contenidoArchivo = System.IO.File.ReadAllText("C://inetpub//wwwroot//Manifiestos_GT//Files_A//" + nombreArchivo);
            transmit.id_empresa = id_empresa;

            return View(transmit);
        }

    }
}
