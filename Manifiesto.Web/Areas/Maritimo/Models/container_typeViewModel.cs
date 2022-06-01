using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class container_typeViewModel
    {
        public Nullable<int> id_container_type { get; set; }

        public string short_name { get; set; }

        public string description { get; set; }

        public string iso { get; set; }

        public int mt { get; set; }
  
        public int id_container_class { get; set; }
    }
}