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

            Manifiesto.Data.Maritimo.MaritimoServices.parametros Params = Manifiesto.Data.Maritimo.MaritimoServices.EmpresaParametros(pais.ToString(), "", "MANIFEST");

            transmit.usuario = Params.manifest_user;
            transmit.password = Params.manifest_pass;
/*            
            switch (pais)
            {
                case 1: //GT
                    transmit.usuario = "12105562";
                    transmit.password = "GTAim2017";//GTAim2022
                    break;
                case 15: //GTLTF
                    transmit.usuario = "41171829";
                    transmit.password = "Latin2016";
                    break;
                case 32: //GTTLA
                    transmit.usuario = "49475428";
                    transmit.password = "Grupotla2019";
                    break;
            }
 */
            transmit.nombreArchivo = nombreArchivo;
            transmit.procesamientoSincrono = false;
            transmit.respuestaXml = true;
            transmit.contenidoArchivo = System.IO.File.ReadAllText("C://inetpub//wwwroot//Manifiesto.Web//Files//" + nombreArchivo);
            transmit.id_empresa = pais;

            return View(transmit);
        }

    }
}
