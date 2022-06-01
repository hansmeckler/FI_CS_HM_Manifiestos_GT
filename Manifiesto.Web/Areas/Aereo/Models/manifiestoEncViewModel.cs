using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Manifiesto.Web.Areas.Aereo.Models
{
    public class manifiestoEncViewModel
    {
        public int cuscar_voyage_id { get; set; }
        [Display(Name = "AWB Number")]
        public string no_viaje { get; set; }
        [Display(Name = "Carrier")]
        public string naviera { get; set; }
        [Display(Name = "Flight")]
        public string vapor { get; set; }
        [Display(Name = "Entry Dt")]
        public string fecha_arribo { get; set; }
        [Display(Name = "Cuscar No")]
        public string cuscar { get; set; }
        [Display(Name = "Manifest")]
        public string manifest { get; set; }
        [Display(Name = "Original")]
        public string original { get; set; }
        public Nullable<double> viaje_id { get; set; }
        public Nullable<double> id_naviera { get; set; }
        public Nullable<double> id_puerto_origen { get; set; }
        public Nullable<double> id_puerto_desembarque { get; set; }
        [Display(Name = "Load")]
        public string puerto_origen { get; set; }
        [Display(Name = "Fin")]
        public string puerto_desembarque { get; set; }
        public string tipo { get; set; }
        public string fecha_salida { get; set; }
        [Display(Name = "Manifest")]
        public Nullable<int> mtype { get; set; }
        [Display(Name = "Func Cuscar")]
        public Nullable<int> mfunction { get; set; }
        public Nullable<int> operation { get; set; }
        public string funcsend { get; set; }
        public Nullable<int> test { get; set; }
        public Nullable<System.DateTime> cuscardt { get; set; }
        public string respuesta_sat { get; set; }
        public string countries { get; set; }
        public int id_status { get; set; }
    }
}