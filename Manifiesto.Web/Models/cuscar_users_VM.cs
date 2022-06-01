using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Manifiesto.Web.Models
{
    public class cuscar_users_VM
    {
        [Required]
        public int id_usuario { get; set; }

        [Required]
        [Display(Name="Nombre Usuario")]
        public string nombre_usuario { get; set; }

        public string domino { get; set; }

        [Display(Name = "Admin")]
        public bool admin { get; set; }

        [Display(Name = "Aereo")]
        public bool aereo { get; set; }

        [Display(Name = "Maritimo")]
        public bool marino { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Display(Name = "Password")]
        public string password { get; set; }
    }
}