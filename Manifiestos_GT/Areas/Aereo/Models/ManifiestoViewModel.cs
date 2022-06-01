using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Manifiesto.Web.Areas.Aereo.Models
{
    public class ManifiestoViewModel
    {
        public int mno { get; set; }
        [Display(Name = "Pieces")]
        public int? no_piezas { get; set; }
        [Display(Name = "Weight")]
        public double? peso { get; set; }
        [Display(Name = "Freight")]
        public Int32 volumen { get; set; }

        public manifiestoEncViewModel manifiesto_enc { get; set; }

        public IEnumerable<manifiestoDetViewModel> manifiesto_det { get; set; }
    }
}