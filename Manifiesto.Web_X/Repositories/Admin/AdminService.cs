using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifiesto.Data.Entities;
using master_aimar.Entities;
using Manifiesto.Data.Models;
using master_aimar.Entities.Entities;
using ventas_gt.Entities.Entities;
using ventas_gt.Entities;
using Manifiesto.Web.Models;
using System.Data.Entity;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Manifiesto.Data.Admin
{
    public class AdminService : IAdminService
    {

        public void DoAuth(string userName, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
        }

        public void DoLogOff()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public string Encrypt_MD5(string password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            string pass = sb.ToString();
            return pass;
        }


        public IEnumerable<Usuarios_EmpresasViewModel> get_users_login()
        {
            using (var context = new master_aimarEntities())
            {
                var listUsers = (from a in context.usuarios_empresas
                                 join b in context.cuscar_users on a.id_usuario equals b.id_usuario
                                 select new Usuarios_EmpresasViewModel
                                 {
                                     id_usuario = a.id_usuario,
                                     pw_name = a.pw_name,
                                     pw_passwd = a.pw_passwd,
                                     
                                     pw_passwd_dias = a.pw_passwd_dias,
                                     pw_passwd_fecha = a.pw_passwd_fecha
                                 }).ToList();

                return listUsers;
            }
        }

        int TEST2 = Convert.ToInt32(HttpContext.Current.Session["id_empresa"]);
        int pais = Convert.ToInt32(HttpContext.Current.Session["id_empresa"]);

        public static string swich_context(int country)
        {
            string conexion;
            switch (country)
            {
                case 1:
                    conexion = "ventas_gt";
                    break;
                case 15:
                    conexion = "ventas_gtltf";
                    break;
                default:
                    conexion = "ventas_gt";
                    break;
            }
            return conexion;
        }

        public string iif(bool condition, string t, string f)
        {
            string functionReturnValue = null;
            if (condition)
                functionReturnValue = t;
            else
                functionReturnValue = f;
            return functionReturnValue;
        }

        public string pad0(string mstring, int l)
        {
            int i = 0;
            for (i = 1; i <= l; i++)
            {
                if (mstring.Length < l)
                    mstring = "0" + mstring;
            }
            return mstring;
        }

        public char Chr(int n)
        {
            return (char)n;
        }

        /*
        public string Break35(string text)
        {
            while (text.IndexOf("  ") != -1)
            {
                text = text.Replace("  ", " ");
            }
            char char10 = Chr(10);
            char char13 = Chr(13);
            string schar10 = char10.ToString();
            string schar13 = char13.ToString();

            text = text.Replace(schar10, "");
            text = text.Replace(schar13, " ");
            string wuz2 = text;
            string wuz = RemoveSpecialCharacters(wuz2);

            const int separateOnLength = 35;

            string separated = new string(
                wuz.Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { ':', x } : new[] { x })
                    .SelectMany(x => x)
                    .ToArray()
                );

            int count = 0;

            int j = -1;
            foreach (var c in separated)
            {
                if (c == ':')
                {
                    j++;
                    count = j + 1;
                }
            }

            if (count == 1)
                separated = separated + ":" + ":" + ":";
            else if (count == 2)
                separated = separated + ":" + ":";
            else if (count == 3)
                separated = separated + ":";
            else
                separated = separated.ToString();


            return separated;

            //return wuz.Substring(1,35) + ":" + wuz.Substring(36,35) + ":" + wuz.Substring(71,35) + ":" + wuz.Substring(106,35) + ":" + wuz.Substring(141,35); 
        }
        */
         
        public string Break35(string text)
        {
            while (text.IndexOf("  ") != -1)
            {
                text = text.Replace("  ", " ");
            }
            char char10 = Chr(10);
            char char13 = Chr(13);
            string schar10 = char10.ToString();
            string schar13 = char13.ToString();

            text = text.Replace(schar10, "");
            text = text.Replace(schar13, " ");
            string wuz2 = text;
            string wuz = RemoveSpecialCharactersClient(wuz2);

            const int separateOnLength = 35;

            string separated = new string(
                wuz.Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { ':', x } : new[] { x })
                    .SelectMany(x => x)
                    .ToArray()
                );

            int count = 0;

            int j = -1;
            foreach (var c in separated)
            {
                if (c == ':')
                {
                    j++;
                    count = j + 1;
                }
            }

            if (count == 1)
                separated = separated + ":";
            else if (count < 1)
                separated = separated.ToString() + "::";


            return separated;
        }



        public string Break100_cliente(string text)
        {
            while (text.IndexOf("  ") != -1)
            {
                text = text.Replace("  ", " ");
            }
            char char10 = Chr(10);
            char char13 = Chr(13);
            string schar10 = char10.ToString();
            string schar13 = char13.ToString();

            text = text.Replace(schar10, "");
            text = text.Replace(schar13, " ");
            string wuz2 = text;
            string wuz = RemoveSpecialCharacters(wuz2);

            const int separateOnLength = 100;

            string separated = new string(
                wuz.Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { ':', x } : new[] { x })
                    .SelectMany(x => x)
                    .ToArray()
                );

            int count = 0;

            int j = -1;
            foreach (var c in separated)
            {
                if (c == ':')
                {
                    j++;
                    count = j + 1;
                }
            }

            if (count == 1)
                separated = separated + ":";
            else
                separated = separated.ToString() + "::";


            return separated;

            //return wuz.Substring(1,35) + ":" + wuz.Substring(36,35) + ":" + wuz.Substring(71,35) + ":" + wuz.Substring(106,35) + ":" + wuz.Substring(141,35); 
        }


        public string Break100_direccion(string text)
        {
            while (text.IndexOf("  ") != -1)
            {
                text = text.Replace("  ", " ");
            }
            char char10 = Chr(10);
            char char13 = Chr(13);
            string schar10 = char10.ToString();
            string schar13 = char13.ToString();

            text = text.Replace(schar10, "");
            text = text.Replace(schar13, " ");
            string wuz2 = text;
            string wuz = RemoveSpecialCharacters(wuz2);

            const int separateOnLength = 100;

            string separated = new string(
                wuz.Select((x, i) => i > 0 && i % separateOnLength == 0 ? new[] { ':', x } : new[] { x })
                    .SelectMany(x => x)
                    .ToArray()
                );

            int count = 0;

            int j = -1;
            foreach (var c in separated)
            {
                if (c == ':')
                {
                    j++;
                    count = j + 1;
                }
            }

            if (count == 1)
                separated = separated + ":" + ":";
            else if (count == 2)
                separated = separated + ":";
            else
                separated = separated.ToString();


            return separated;

            //return wuz.Substring(1,35) + ":" + wuz.Substring(36,35) + ":" + wuz.Substring(71,35) + ":" + wuz.Substring(106,35) + ":" + wuz.Substring(141,35); 
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
        }

        public static string RemoveSpecialCharactersClient(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.&()-]+", " ", RegexOptions.Compiled);
        }

        public IEnumerable<cuscar_users_VM> get_cuscar_users()
        {
            using (var context = new master_aimarEntities())
            {
                IEnumerable<cuscar_users_VM> get_users = (from a in context.cuscar_users
                                                          join b in context.usuarios_empresas on a.id_usuario equals b.id_usuario
                                                          select new cuscar_users_VM
                                                          {
                                                              id_usuario = b.id_usuario,
                                                              nombre_usuario = b.pw_name,
                                                              domino = b.dominio,
                                                              admin = a.admin,
                                                              aereo = a.aereo,
                                                              marino = a.marino,
                                                              nombre = b.pw_gecos,
                                                              apellido = "",
                                                              password = b.pw_passwd
                                                          }).ToList();
                return get_users;
            }
        }

        public IEnumerable<usuarios_empresas> get_usuarios_empresas()
        {
            using (var context = new master_aimarEntities())
            {
                return context.usuarios_empresas.ToList();
            }
        }

        public cuscar_users_VM addUser(cuscar_users_VM vm)
        {
            try
            {
                using (var context = new master_aimarEntities())
                {
                    cuscar_users cuscar_users = new cuscar_users();
                    cuscar_users.id_usuario = vm.id_usuario;
                    cuscar_users.username = vm.nombre_usuario;
                    cuscar_users.admin = vm.admin;
                    cuscar_users.aereo = vm.aereo;
                    cuscar_users.marino = vm.marino;
                    cuscar_users.firstname = vm.nombre;
                    cuscar_users.lastname = vm.apellido;
                    cuscar_users.password = vm.password;

                    context.cuscar_users.Add(cuscar_users);

                    if (vm.aereo == true)
                    {
                        int[] emp_perm = new int[] { 1, 6, 7, 8 };
                        foreach (int item in emp_perm)
                        {
                            usuarios_permisos_manifiestos usuarios_permisos_manifiestos = new usuarios_permisos_manifiestos();
                            usuarios_permisos_manifiestos.id_usuario = vm.id_usuario;
                            usuarios_permisos_manifiestos.id_empresa = 0;
                            usuarios_permisos_manifiestos.id_menu = item;
                            usuarios_permisos_manifiestos.fecha_creado = DateTime.Today;
                            context.usuarios_permisos_manifiestos.Add(usuarios_permisos_manifiestos);
                        }
                    }

                    if (vm.marino == true)
                    {
                        int[] emp = new int[] { 1, 15 };
                        int[] emp_perm = new int[] { 2, 3, 4 };
                        foreach (int item in emp)
                        {
                            foreach(int item2 in emp_perm)
                            {
                                usuarios_permisos_manifiestos usuarios_permisos_manifiestos = new usuarios_permisos_manifiestos();
                                usuarios_permisos_manifiestos.id_usuario = vm.id_usuario;
                                usuarios_permisos_manifiestos.id_empresa = item;
                                usuarios_permisos_manifiestos.id_menu = item2;
                                usuarios_permisos_manifiestos.fecha_creado = DateTime.Today;
                                context.usuarios_permisos_manifiestos.Add(usuarios_permisos_manifiestos);
                            }
                            usuarios_x_empresa usuarios_x_empresa = new usuarios_x_empresa();
                            usuarios_x_empresa.id_usuario = vm.id_usuario;
                            usuarios_x_empresa.id_empresa = item;
                            usuarios_x_empresa.fecha_creado = DateTime.Today;
                            usuarios_x_empresa.id_menu = 0;
                            context.usuarios_x_empresa.Add(usuarios_x_empresa);
                        }
                    }

                    context.SaveChanges();
                    return vm;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public cuscar_users_VM editUser(cuscar_users_VM vm)
        {
            using (var context = new master_aimarEntities())
            {
                cuscar_users cuscar_users = new cuscar_users();
                cuscar_users.id_usuario = vm.id_usuario;
                cuscar_users.admin = vm.admin;
                cuscar_users.aereo = vm.aereo;
                cuscar_users.marino = vm.marino;
                context.Entry(cuscar_users).State = EntityState.Modified;
                context.SaveChanges();

                return vm;
            }
        }

        public aaseqEntity getAaseq()
        {
            using (var context = new master_aimarEntities())
            {

                string query = "select nexttransmission,nextmessage,nextmanifest,nextarchive,nextatransmission,nextamessage,nextamanifest,nextaarchive,firma,keyid,nextctransmission,nextcmessage,nextcmanifest,nextcarchive from aaseq";
                var data = context.Database.SqlQuery<aaseqEntity>(query).ToList();

                return data.FirstOrDefault();
            }
        }

        public IEnumerable<TypeEntity> getType()
        {
            IList<TypeEntity> getList = new List<TypeEntity>();
            getList.Add(new TypeEntity() { Id = 785, type = "Manifest" });
            getList.Add(new TypeEntity() { Id = 632, type = "Anwser" });

            return getList.ToList();
        }

        public IEnumerable<functionEntity> getFunction()
        {
            IList<functionEntity> getList = new List<functionEntity>();
            getList.Add(new functionEntity() { Id = 9, function = "Original" });
            getList.Add(new functionEntity() { Id = 20, function = "Replace" });
            getList.Add(new functionEntity() { Id = 2, function = "+ BL" });
            getList.Add(new functionEntity() { Id = 3, function = "- BL" });
            getList.Add(new functionEntity() { Id = 1, function = "Cancel" });
            getList.Add(new functionEntity() { Id = 36, function = "Repl CI/CS" });
            getList.Add(new functionEntity() { Id = 13, function = "+ BL CI/CS" });
            getList.Add(new functionEntity() { Id = 40, function = "- BL CI/CS" });
            getList.Add(new functionEntity() { Id = 10, function = "Cancel CI/CS" });

            return getList.ToList();
        }

        public IEnumerable<operationEntity> getOperation()
        {
            IList<operationEntity> getList = new List<operationEntity>();
            getList.Add(new operationEntity() { Id = 22, operation = "Export" });
            getList.Add(new operationEntity() { Id = 23, operation = "Import" });
            getList.Add(new operationEntity() { Id = 24, operation = "Transit" });

            return getList.ToList();
        }

        public IEnumerable<container_type> getContainerType()
        {
            using (var context = new master_aimarEntities())
            {
                var result = context.container_type.ToList();
                return result;
            }
        }

        public static IEnumerable<container_type> getContainerTypeList()
        {
            using (var context = new master_aimarEntities())
            {
                var result = context.container_type.ToList();
                return result;
            }
        }

        public static IEnumerable<commodities> getCommoditiesList()
        {
            using (var context = new master_aimarEntities())
            {
                return context.commodities.ToList();
            }
        }

        public static IEnumerable<tipo_paquete> getTipoPaqueteList()
        {
            using (var context = new master_aimarEntities())
            {
                return context.tipo_paquete.ToList();
            }
        }

        public IEnumerable<clientes> getClientes()
        {
            using (var context = new master_aimarEntities())
            {
                return context.clientes.ToList();
            }
        }

        public IEnumerable<tipo_paquete> getTipoPaquete()
        {
            using (var context = new master_aimarEntities())
            {
                return context.tipo_paquete.ToList();
            }
        }

        public IEnumerable<aduanas> getAduanas()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                var result = context.aduanas.ToList();
                return result;
            }
        }

        public static IEnumerable<navieras> getNavieras()
        {
            using (var context = new master_aimarEntities())
            {
                return context.navieras.ToList();
            }
        }

        public static IEnumerable<unlocode> getUnlocode()
        {
            using (var context = new master_aimarEntities())
            {
                return context.unlocode.ToList();
            }
        }


        public string getManifiestoMaster(string tipo, long? viaje_id)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                if (tipo == "LCL")
                {
                    var sequenceQueryResult = context.viaje_contenedor.Where(a => a.activo == true && a.viaje_id == viaje_id).Select(a => a.manifiesto_master).FirstOrDefault();
                    return sequenceQueryResult;
                }
                else
                {
                    var sequenceQueryResult = context.bl_completo.Where(a => a.activo == true && a.bl_id == viaje_id).Select(a => a.manifiesto_master).FirstOrDefault();
                    return sequenceQueryResult;
                }
            }
        }

        public IEnumerable<empresas> get_empresas()
        {
            using (var context = new master_aimarEntities())
            {
                return context.empresas.ToList();
            }
        }

        public IEnumerable<usuarios_x_empresa> get_usuarios_x_empresa()
        {
            using (var context = new master_aimarEntities())
            {
                return context.usuarios_x_empresa.ToList();
            }
        }

        public IEnumerable<usuarios_permisos_manifiestos> get_usuarios_permisos_manifiestos()
        {
            using (var context = new master_aimarEntities())
            {
                return context.usuarios_permisos_manifiestos.ToList();
            }
        }

        public static string getNombreCliente(long? idCliente)
        {
            using (var context = new master_aimarEntities())
            {
                var cliente = context.clientes.Where(p => p.id_cliente == idCliente).Select(p => p.nombre_cliente);
                if (cliente.Count() != 0)
                    return cliente.Single();
                else
                    return "NF";
            }
        }

        public static string getDireccionCliente(long? idCliente)
        {
            using (var context = new master_aimarEntities())
            {
                var direccion = context.direcciones.Where(p => p.id_cliente == idCliente).Select(p => p.direccion_completa).ToList();
                if (direccion.Count() != 0)
                    return direccion.FirstOrDefault();
                else
                    return "NF";
            }
        }


        public static string getPais(long? unlocode_id)
        {
            using (var context = new master_aimarEntities())
            {
                var pais = context.unlocode.Where(a => a.unlocode_id == unlocode_id).Select(a => a.pais).FirstOrDefault();
                return pais;
            }
        }


        public static string getlocode(long? unlocode_id)
        {
            using (var context = new master_aimarEntities())
            {
                var locode = context.unlocode.Where(a => a.unlocode_id == unlocode_id).Select(a => a.locode).FirstOrDefault();
                return locode;
            }
        }


        public static string getCommodityNamees(int? comodity_id)
        {
            using (var context = new master_aimarEntities())
            {
                var comodity = context.commodities.Where(a => a.commodityid == comodity_id).Select(a => a.namees).FirstOrDefault();
                return comodity;
            }
        }

        public static string getCodigoTributario(long? idCliente)
        {
            using (var context = new master_aimarEntities())
            {
                var codigo = context.clientes.Where(p => p.id_cliente == idCliente).Select(a => a.codigo_tributario).FirstOrDefault();
                if (codigo == null)
                    return codigo = "";
                else
                    return codigo;
            }
        }

    }
}
