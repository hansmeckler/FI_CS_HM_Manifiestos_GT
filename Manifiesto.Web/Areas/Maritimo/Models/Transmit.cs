using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class Transmit
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string nombreArchivo { get; set; }
        public bool procesamientoSincrono { get; set; }
        public bool respuestaXml { get; set; }
        public string contenidoArchivo { get; set; }
        public int id_empresa { get; set; }
    }
}