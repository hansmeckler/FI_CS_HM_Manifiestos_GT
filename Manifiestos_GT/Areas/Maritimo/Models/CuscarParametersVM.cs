using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class CuscarParametersVM
    {
        public int no_viaje { get; set; }

        public int no_contenedor { get; set; }

        public int type { get; set; }

        public int function_t { get; set; }

        public int operation { get; set; }

        public string funcsend { get; set; }

        public int fix { get; set; }

    }
}