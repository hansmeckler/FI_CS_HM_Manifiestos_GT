using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Models
{
    public class Usuarios_EmpresasViewModel
    {
        public long id_usuario { get; set; }
        public string pw_name { get; set; }
        public string pw_passwd { get; set; }
        public Nullable<int> pw_uid { get; set; }
        public Nullable<int> pw_gid { get; set; }
        public string pw_gecos { get; set; }
        public string pw_dir { get; set; }
        public string pw_shell { get; set; }
        public short tipo_usuario { get; set; }
        public string pais { get; set; }
        public string dominio { get; set; }
        public Nullable<short> level { get; set; }
        public int pw_activo { get; set; }
        public string pw_codigo_tributario { get; set; }
        public Nullable<int> pw_correo { get; set; }
        public Nullable<System.DateTime> modificado { get; set; }
        public string locode { get; set; }
        
        public int pw_passwd_dias { get; set; }
        public DateTime pw_passwd_fecha { get; set; }

    }
}