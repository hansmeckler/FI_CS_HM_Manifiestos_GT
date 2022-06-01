using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ventas_gt.Entities.Entities;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class CuscarPreviewVM
    {

        public string no_cuscar { get; set; }

        public string cuscar_ref { get; set; }

        public string type { get; set; }

        public string function { get; set; }

        public string operation { get; set; }

        public string sender { get; set; }

        public List<string> myobj { get; set; }

        public string total_bls { get; set; }

        public string total_peso { get; set; }

        public string total_volumen { get; set; }

        public string total_piezas { get; set; }

        public string no_contenedores { get; set; }

        public string flete { get; set; }

        public CuscarParametersVM parameters { get; set; }

        public cuscar_viaje_infoVM cuscar_viaje_info { get; set; }

        public IList<cuscar_bl_infoVM> cuscar_bl_info { get; set; }

        public IList<cuscar_container_infoVM> cuscar_container_info { get; set; }
    }
}