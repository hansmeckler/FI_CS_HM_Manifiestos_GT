using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Web.Areas.Maritimo.Models;

namespace Manifiesto.Web.Areas.Maritimo.Controllers
{
    public class TransmitController : Controller
    {
        //
        // GET: /Maritimo/Transmit/

        public ActionResult Index(string nombreArchivo)
        {
            int pais = Convert.ToInt32(HttpContext.Session["id_empresa"]);
            Transmit transmit = new Transmit();
            if (pais == 1)
            {
                transmit.usuario = "12105562";
                transmit.password = "GTAim2022";
            }
            else if (pais == 15)
            {
                transmit.usuario = "41171829";
                transmit.password = "Latin2016";
            }
            transmit.nombreArchivo = nombreArchivo;
            transmit.procesamientoSincrono = false;
            transmit.respuestaXml = true;
            transmit.contenidoArchivo = System.IO.File.ReadAllText("C://inetpub//wwwroot//Manifiestos_GT//Files//" + nombreArchivo);
            transmit.id_empresa = pais;

            return View(transmit);
        }

    }
}
