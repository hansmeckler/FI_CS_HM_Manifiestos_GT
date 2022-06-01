using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Models
{
    public class cuscar_rols_per_users_VM
    {
        public long id_usuario { get; set; }

        public int aereo { get; set; }

        public int maritimo { get; set; }

        public int admin { get; set; }
    }
}