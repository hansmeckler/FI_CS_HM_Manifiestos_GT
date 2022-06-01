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

            Manifiesto.Data.Maritimo.MaritimoServices.parametros Params = Manifiesto.Data.Maritimo.MaritimoServices.EmpresaParametros(id_empresa, "", "MANIFEST");

            transmit.usuario = Params.manifest_user;
            transmit.password = Params.manifest_pass;
/*                        
            switch (id_empresa) { 
                case "GT":
                        transmit.usuario = "12105562";
                        transmit.password = "GTAim2017";//GTAim2022
                        break;
                case "GTLTF":
                        transmit.usuario = "41171829";
                        transmit.password = "Latin2016";
                        break;
                case "GTTLA":
                        transmit.usuario = "49475428";
                        transmit.password = "Grupotla2019";
                        break;
            }
 */ 
            transmit.nombreArchivo = nombreArchivo;
            transmit.procesamientoSincrono = false;
            transmit.respuestaXml = true;
            transmit.contenidoArchivo = System.IO.File.ReadAllText("C://inetpub//wwwroot//Manifiesto.Web//Files_A//" + nombreArchivo);
            transmit.id_empresa = id_empresa;

            return View(transmit);
        }

    }
}
