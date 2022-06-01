using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Repositories.Models
{
    public class cuscar_container_info_vm
    {
        public long cuscar_container_id { get; set; }

        public long cuscar_viaje_id { get; set; }

        public Nullable<int> mtype { get; set; }

        public Nullable<int> mfunction { get; set; }

        public Nullable<int> operation { get; set; }

        public string funcsend { get; set; }
    }
}