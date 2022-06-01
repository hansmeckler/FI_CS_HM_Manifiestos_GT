using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Manifiesto.Web.Areas.Aereo.Models
{
    public class guiasListViewModel
    {
        [Display(Name = "AWB")]
        public string awbnumber { get; set; }
        [Display(Name = "HAWB")]
        public string hawbnumber { get; set; }
        [Display(Name = "Created")]
        public DateTime createddate { get; set; }

        public int id { get; set; }

        [Display(Name = "BLs")]
        public Nullable<int> bls { get; set; }
    }
}