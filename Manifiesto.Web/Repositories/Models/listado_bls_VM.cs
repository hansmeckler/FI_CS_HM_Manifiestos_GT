using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Manifiesto.Data.Models
{
    public class listado_bls_VM
    {
        [Display(Name = "Type")]
        public string tipo { get; set; }

        [Display(Name = "Voyage")]
        public string no_viaje { get; set; }

        public string no_bl { get; set; }

        [Display(Name = "Arrival")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fecha_arribo { get; set; }

        [Display(Name = "Ship")]
        public string vapor { get; set; }

        public long id { get; set; }

        [Display(Name = "BLs")]
        public long count1 { get; set; }

        [Display(Name = "CNs")]
        public long count2 { get; set; }

        [Display(Name = "Import_Export")]
        public string import_export { get; set; }
    }
}
