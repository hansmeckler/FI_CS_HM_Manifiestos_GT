using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ventas_gt.Entities.Entities;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class manifestViewModelLCL
    {

        public int fix { get; set; }

        public string import_export { get; set; }

        public IEnumerable<cuscar_bl_info> cuscar_bl_info { get; set; }

        public cuscar_viaje_info cuscar_viaje_info { get; set; }

        public IEnumerable<cuscar_container_info> cuscar_container_info { get; set; }
    }
}