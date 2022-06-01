using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Repositories.Models
{
    public class empresas_model
    {
        public int id_empresa { get; set; }

        public int id_usuario { get; set; }

        public string empresa { get; set; }

        public string nombre_pais { get; set; }
    }
}