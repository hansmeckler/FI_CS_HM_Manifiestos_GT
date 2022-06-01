using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Models
{
    public class reporteVM
    {
        public int anio { get; set; }

        public string origen { get; set; }

        public string pais { get; set; }

        public Decimal peso { get; set; }
    }

    public class Totales
    {
        public int anio { get; set; }

        public bool import_export { get; set; }

        public bool cif { get; set; }

        public string region { get; set; }

        public string pais { get; set; }

        public Nullable<decimal> peso { get; set; }
    }
}