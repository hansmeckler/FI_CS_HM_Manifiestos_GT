using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class manifestListViewModel
    {
        public long id { get; set; }

        [DisplayName("Voyage No")]
        public string no_viaje { get; set; }

        [DisplayName("Manifest No")]
        public string manifest { get; set; }

        [DisplayName("Original No")]
        public string original { get; set; }

        [DisplayName("Archive No")]
        public string cuscar { get; set; }

        [DisplayName("Date")]
        public string cuscardt { get; set; }

        [DisplayName("Type")]
        public string type { get; set; }

        public string function { get; set; }

        public string operation { get; set; }

        public string funcsend { get; set; }

        public Nullable<int> test { get; set; }

        public string tipo { get; set; }

        public Nullable<bool> import_export { get; set; }

        public string import_export_desc { get; set; }

    }
}