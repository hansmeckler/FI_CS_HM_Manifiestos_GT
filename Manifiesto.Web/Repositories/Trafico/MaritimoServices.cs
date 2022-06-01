using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ventas_gt.Entities.Entities;
using ventas_gt.Entities;
using Manifiesto.Data.Models;
using Npgsql;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using Manifiesto.Web.Repositories.Models;
using AutoMapper;
using Manifiesto.Web.Areas.Maritimo.Models;

namespace Manifiesto.Data.Maritimo
{

    public class MaritimoServices : IMaritimoServices
    {
        int pais = Convert.ToInt32(HttpContext.Current.Session["id_empresa"]);


        public IEnumerable<cargos_bl> get_cargos_bl()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                return context.cargos_bl.ToList();
            }
        }

        public IEnumerable<cuscar_viaje_info> get_viaje_info()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                return context.cuscar_viaje_info.ToList();
            }
        }

        public IEnumerable<cuscar_container_info> get_container_info()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                return context.cuscar_container_info.ToList();
            }
        }

        public IEnumerable<cuscar_container_info> get_container_info_id(int id_viaje)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                var list = (from a in context.cuscar_container_info
                            join b in context.cuscar_bl_info on a.bl_id_ref equals b.bl_id
                            where a.cuscar_viaje_id == id_viaje && b.cuscar_viaje_id == id_viaje && b.selected2 == true
                            select new cuscar_container_infoVM_2
                            {
                                cuscar_container_id = a.cuscar_container_id,
                                cuscar_viaje_id = a.cuscar_viaje_id,
                                no_contenedor = a.no_contenedor,
                                mbl = a.mbl,
                                seal = a.seal,
                                id_container_type = a.id_container_type,
                                no_piezas = a.no_piezas,
                                id_tipo_paquete = a.id_tipo_paquete,
                                tipo_paquete = a.tipo_paquete,
                                volumen = a.volumen,
                                comodity_id = a.comodity_id,
                                warehouse = a.warehouse,
                                comodity = a.comodity,
                                no_bls = a.no_bls,
                                bl_id = a.bl_id,
                                contenedor_id = a.contenedor_id,
                                viaje_id = a.viaje_id,
                                container_type = a.container_type,
                                cuscar = a.cuscar,
                                cuscardt = a.cuscardt,
                                manifest = a.manifest,
                                original = a.original,
                                mtype = a.mtype,
                                mfunction = a.mfunction,
                                operation = a.operation,
                                funcsend = a.funcsend,
                                test = a.test,
                                peso = a.peso,
                                id_status = a.id_status,
                                bl_id_ref = a.bl_id_ref,
                                respuesta_sat = a.respuesta_sat
                            }).ToList();


                Mapper.CreateMap<cuscar_container_infoVM_2, cuscar_container_info>();
                IList<cuscar_container_info> vm = Mapper.Map<IList<cuscar_container_infoVM_2>, IList<cuscar_container_info>>(list);

                return vm;
            }
        }

        public IEnumerable<cuscar_bl_info> get_bl_info()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                return context.cuscar_bl_info.ToList();
            }
        }

        public IEnumerable<totalesDet2> getTotales(string tipo_servicio, int cno, int cond, long? bl_id_ref)
        {
            IList<totalesDet2> totales = new List<totalesDet2>();

            string connStr = "";

            switch (pais)
            {
                case 1: connStr = ConfigurationManager.ConnectionStrings["ventas_gt"].ConnectionString; break;
                case 15: connStr = ConfigurationManager.ConnectionStrings["ventas_gtltf"].ConnectionString; break;
                case 32: connStr = ConfigurationManager.ConnectionStrings["ventas_gttla"].ConnectionString; break;
            }

            var m_conn = new NpgsqlConnection(connStr);

            using (var connec = new NpgsqlConnection(connStr))
            {

                if (tipo_servicio == "LCL")
                {
                    if (cond == 1)
                    {
                        using (var coman = new NpgsqlCommand())
                        {
                            connec.Open();
                            coman.Connection = connec;
                            coman.CommandText = "select count(1) as count, sum(peso) as peso, sum(volumen) as volumen, sum(no_piezas) as no_piezas FROM cuscar_bl_info where selected2=true and cuscar_container_id = " + cno;

                            coman.CommandTimeout = 0;
                            NpgsqlDataReader reader = coman.ExecuteReader();

                            while ((reader.Read()))
                            {
                                totalesDet2 ob = new totalesDet2();

                                ob.count = Convert.ToInt32(reader["count"]);
                                ob.peso = Convert.ToDecimal(reader["peso"]);
                                ob.volumen = Convert.ToDecimal(reader["volumen"]);
                                ob.no_piezas = Convert.ToInt64(reader["no_piezas"]);
                                totales.Add(ob);
                            }
                            connec.Close();
                        }
                        return totales;
                    }
                    else
                    {
                        using (var coman = new NpgsqlCommand())
                        {
                            connec.Open();
                            coman.Connection = connec;
                            coman.CommandText = "select count(1) as count, sum(peso) as peso, sum(volumen) as volumen, sum(no_piezas) as no_piezas FROM cuscar_bl_info where cuscar_container_id = " + cno;

                            coman.CommandTimeout = 0;
                            NpgsqlDataReader reader = coman.ExecuteReader();

                            while ((reader.Read()))
                            {
                                totalesDet2 ob = new totalesDet2();

                                ob.count = Convert.ToInt32(reader["count"]);
                                ob.peso = Convert.ToDecimal(reader["peso"]);
                                ob.volumen = Convert.ToDecimal(reader["volumen"]);
                                ob.no_piezas = Convert.ToInt64(reader["no_piezas"]);
                                totales.Add(ob);
                            }
                            connec.Close();
                        }
                        return totales;
                    }
                }
                else //FCL
                {
                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
                        if (cond == 3)
                            coman.CommandText = "select 1 as count, sum(cu.peso) as peso, sum(cu.volumen) as volumen, sum(cu.no_piezas) as no_piezas FROM cuscar_container_info cu where cu.cuscar_viaje_id =" + cno;
                        else if (cond == 4)
                            coman.CommandText = "select 1 as count, coalesce(sum(cu.peso),0) as peso, coalesce(sum(volumen),0) as volumen, coalesce(sum(no_piezas),0) as no_piezas FROM cuscar_container_info cu where cu.cuscar_viaje_id =" + cno + " and cu.bl_id_ref =" + bl_id_ref;
                        else if (cond == 5)
                            coman.CommandText = "select 1 as count, coalesce(sum(peso),0) as peso, coalesce(sum(volumen),0) as volumen, coalesce(sum(no_piezas),0) as no_piezas FROM cuscar_bl_info where cuscar_viaje_id = " + cno + " and bl_id = " + bl_id_ref;
                        else if (cond == 6)
                            coman.CommandText = "select 1 as count, coalesce(sum(cu.peso),0) as peso, coalesce(sum(cu.volumen),0) as volumen, coalesce(sum(cu.no_piezas),0) as no_piezas FROM cuscar_container_info cu inner join cuscar_bl_info a on cu.bl_id_ref = a.bl_id where cu.cuscar_viaje_id = " + cno + " and a.cuscar_viaje_id = " + cno + " and a.selected2 = true";
                        else if (cond == 7)
                            coman.CommandText = "select 1 as count, coalesce(sum(cu.peso),0) as peso, coalesce(sum(cu.volumen),0) as volumen, coalesce(sum(cu.no_piezas),0) as no_piezas FROM cuscar_container_info cu inner join cuscar_bl_info a on cu.bl_id_ref = a.bl_id where cu.cuscar_viaje_id = " + cno + " and a.cuscar_viaje_id = " + cno + "";
                        else
                            coman.CommandText = "select 1 as count, sum(cu.peso) as peso, sum(volumen) as volumen, sum(no_piezas) as no_piezas FROM cuscar_container_info cu where cu.cuscar_viaje_id =" + cno + " and cu.bl_id_ref =" + bl_id_ref;
                        coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        while ((reader.Read()))
                        {
                            totalesDet2 ob = new totalesDet2();

                            ob.count = Convert.ToInt32(reader["count"]);
                            ob.peso = Convert.ToDecimal(reader["peso"]);
                            ob.volumen = Convert.ToDecimal(reader["volumen"]);
                            ob.no_piezas = Convert.ToInt64(reader["no_piezas"]);
                            totales.Add(ob);
                        }
                        connec.Close();
                    }
                    return totales;
                }
            }
        }


        public decimal getSumPeso(long? vc_id, int function, long container_id)
        {
            string connStr = "";
            
            switch (pais)
            {
                case 1: connStr = ConfigurationManager.ConnectionStrings["ventas_gt"].ConnectionString; break;
                case 15: connStr = ConfigurationManager.ConnectionStrings["ventas_gtltf"].ConnectionString; break;
                case 32: connStr = ConfigurationManager.ConnectionStrings["ventas_gttla"].ConnectionString; break;
            }

            var m_conn = new NpgsqlConnection(connStr);
            decimal peso = 0;

            using (var connec = new NpgsqlConnection(connStr))
            {

                if (function == 2 || function == 3 || function == 13 || function == 40)
                {

                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
                        coman.CommandText = "select sum(peso) as peso from cuscar_bl_info where selected2 = true and cuscar_viaje_id  = " + vc_id + " and cuscar_container_id = " + container_id;

                        coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        while ((reader.Read()))
                        {
                            peso = Convert.ToDecimal(reader["peso"]);
                        }
                        connec.Close();
                    }
                    return peso;
                }
                else
                {
                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
                        coman.CommandText = "select sum(peso) as peso from cuscar_bl_info where cuscar_viaje_id  = " + vc_id + " and cuscar_container_id = " + container_id;

                        coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        while ((reader.Read()))
                        {
                            peso = Convert.ToDecimal(reader["peso"]);
                        }
                        connec.Close();
                    }
                    return peso;
                }
            }
        }

        public void update_cuscar(cuscar_container_info cuscar_actual, cuscar_container_info cuscar_to_update)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                context.Entry(cuscar_actual).CurrentValues.SetValues(cuscar_to_update);
                context.SaveChanges();
            }
        }

        public cuscar_container_info close_cuscar(long id, string firma)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                var currentContainer = context.cuscar_container_info.Where(a => a.cuscar_container_id == id);

                string query_update = "update cuscar_container_info set " +
                        "id_status = 0, " +
                        "respuesta_sat = '" + firma + "' " +
                        "where cuscar_container_id =" + id;

                context.Database.ExecuteSqlCommand(query_update);
                context.SaveChanges();

                return currentContainer.FirstOrDefault();
            }
        }

        public cuscar_container_info close_cuscar_fcl(long id, string firma)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {

                var cuscar_containers = context.cuscar_container_info.Where(a => a.cuscar_viaje_id == id).ToList();

                foreach (var c in cuscar_containers)
                {
                    context.Database.ExecuteSqlCommand("Update cuscar_container_info set respuesta_sat = '" + firma + "', id_status = 0 where cuscar_viaje_id = " + id);
                }

                return cuscar_containers.FirstOrDefault();
            }
        }

        public long update_bl_info(cuscar_bl_info cuscar_bl_info)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "update cuscar_bl_info set no_bl = '" + cuscar_bl_info.no_bl + "', cliente = '" + cuscar_bl_info.cliente + "', shipper = '" + cuscar_bl_info.shipper + "', tipo_docto = " + cuscar_bl_info.tipo_docto + ", direccion_cliente = '" + cuscar_bl_info.direccion_cliente + "', selected2 = " + cuscar_bl_info.selected2 + " where cuscar_bl_id = " + cuscar_bl_info.cuscar_bl_id;
                context.Database.ExecuteSqlCommand(query);

                return cuscar_bl_info.cuscar_viaje_id;
            }
        }

        public long update_container_info(cuscar_container_info cuscar_container_info)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "update cuscar_container_info set comodity = '" + cuscar_container_info.comodity + "' where cuscar_container_id = " + cuscar_container_info.cuscar_container_id;
                context.Database.ExecuteSqlCommand(query);

                return cuscar_container_info.cuscar_viaje_id;
            }
        }


        public long update_bl_info_lcl(cuscar_bl_info cuscar_bl_info)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "update cuscar_bl_info set no_bl = '" + cuscar_bl_info.no_bl + "', comodity = '" + cuscar_bl_info.comodity + "', cliente = '" + cuscar_bl_info.cliente + "', shipper = '" + cuscar_bl_info.shipper + "', tipo_docto = " + cuscar_bl_info.tipo_docto + ", direccion_cliente = '" + cuscar_bl_info.direccion_cliente + "', selected2 = " + cuscar_bl_info.selected2 + " where cuscar_bl_id = " + cuscar_bl_info.cuscar_bl_id;
                context.Database.ExecuteSqlCommand(query);

                return cuscar_bl_info.cuscar_viaje_id;
            }
        }

        public void update_container_original(cuscar_container_info_update cuscar_update_info, int id)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "";
                if (id == 1)
                    query = "UPDATE cuscar_container_info SET cuscar = '" + cuscar_update_info.mfile + "', cuscardt = '" + cuscar_update_info.s + "', manifest = '" + cuscar_update_info.manifest + "', original = '" + cuscar_update_info.manifest + "' where cuscar_container_id = " + cuscar_update_info.cuscar_container_id;
                else
                    query = "UPDATE cuscar_container_info SET cuscar = '" + cuscar_update_info.mfile + "', cuscardt = '" + cuscar_update_info.s + "', manifest = '" + cuscar_update_info.manifest + "' where cuscar_container_id = " + cuscar_update_info.cuscar_container_id;
                context.Database.ExecuteSqlCommand(query);
            }
        }


        public void update_container_original_fcl(cuscar_container_info_update cuscar_update_info, int id)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "";
                if (id == 1)
                    query = "UPDATE cuscar_container_info SET cuscar = '" + cuscar_update_info.mfile + "', cuscardt = '" + cuscar_update_info.s + "', manifest = '" + cuscar_update_info.manifest + "', original = '" + cuscar_update_info.manifest + "' where cuscar_viaje_id = " + cuscar_update_info.cuscar_viaje_id;
                else
                    query = "UPDATE cuscar_container_info SET cuscar = '" + cuscar_update_info.mfile + "', cuscardt = '" + cuscar_update_info.s + "', manifest = '" + cuscar_update_info.manifest + "' where cuscar_viaje_id = " + cuscar_update_info.cuscar_viaje_id;
                context.Database.ExecuteSqlCommand(query);
            }
        }

        public void insert_history(string mchgfield, string mcf, string mct, string mmn_id, string mnm_table, string mid_field, int? mid_value, bool mvalid)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "insert into aahist (chgfield,chgfrom,chgto,mn_id,nm_table,id_field,id_value,myuser) " +
                               "values ('" + mchgfield + "','" + mcf + "','" + mct + "'," + mmn_id + ",'" + mnm_table + "','" + mid_field + "'," + mid_value + ",'" + 1344 + "')";
                context.Database.ExecuteSqlCommand(query);
                context.SaveChanges();
            }
        }

        public void update_cuscar(cuscar_container_info_vm cuscar_container_update)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                string query = "Update cuscar_container_info set" +
                               " mtype = " + cuscar_container_update.mtype + ", " +
                               " mfunction = " + cuscar_container_update.mfunction + ", " +
                               " operation = " + cuscar_container_update.operation + ", " +
                               " funcsend = '" + cuscar_container_update.funcsend + "'" +
                               " WHERE cuscar_container_id =  " + cuscar_container_update.cuscar_container_id;
                context.Database.ExecuteSqlCommand(query);
                context.SaveChanges();
            }
        }





        public static Manifiesto.Web.Models.Usuarios_EmpresasViewModel login(string user, string pass, string connstring)
        {
            var temp = new Manifiesto.Web.Models.Usuarios_EmpresasViewModel();

            temp.pw_activo = -1;

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            
                using (var connec = new NpgsqlConnection(connStr))
                {
                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
                        coman.CommandText = "select usuarios_empresas.id_usuario, pw_name, pw_passwd, pw_passwd_dias, pw_passwd_fecha from usuarios_empresas inner join aausers on aausers.id_usuario = usuarios_empresas.id_usuario where pw_name = '" + user + "' and (pw_passwd = MD5('" + pass + "') OR  pw_passwd = '" + pass + "')";

                        //coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        if (reader.Read())
                        {
                            temp.id_usuario = Convert.ToInt32(reader["id_usuario"]); 
                            temp.pw_name = Convert.ToString(reader["pw_name"]);
                            temp.pw_passwd = Convert.ToString(reader["pw_passwd"]);
                            temp.pw_passwd_dias = Convert.ToInt32(reader["pw_passwd_dias"]);
                            temp.pw_passwd_fecha = Convert.ToDateTime(reader["pw_passwd_fecha"]);
                            temp.pw_activo = 1;
                        }
                        connec.Close();
                    }                    
                
                }

            }
            catch (Exception e)
            {
                temp.pw_activo = -2;
                temp.locode = e.Message;
            }

            return temp;
        }



        

        public static IEnumerable<Manifiesto.Web.Repositories.Models.empresas_model> EmpresasUsuarios(string username, string connstring)
        {
            List<Manifiesto.Web.Repositories.Models.empresas_model> ListEmp = new List<Manifiesto.Web.Repositories.Models.empresas_model>();

            

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;

                using (var connec = new NpgsqlConnection(connStr))
                {
                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
        
                        coman.CommandText = "select c.id_usuario, a.id_empresa, a.nombre_empresa, a.nombre_pais from empresas a " +
                        "inner join usuarios_x_empresa b on a.id_empresa = b.id_empresa " +
                        "inner join usuarios_empresas c on b.id_usuario = c.id_usuario and c.pw_name = '" + username  + "' " + 
                        "limit 50";

                        //coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        while (reader.Read())
                        {
                            Manifiesto.Web.Repositories.Models.empresas_model Temp = new Manifiesto.Web.Repositories.Models.empresas_model();
                            
                            Temp.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                            Temp.nombre_empresa = Convert.ToString(reader["nombre_empresa"]);
                            Temp.nombre_pais = Convert.ToString(reader["nombre_pais"]);
                            Temp.id_empresa = Convert.ToInt32(reader["id_empresa"]);

                            Manifiesto.Data.Maritimo.MaritimoServices.parametros Params = Manifiesto.Data.Maritimo.MaritimoServices.EmpresaParametros(Temp.id_empresa.ToString(), "", "MANIFEST");
                            Temp.empresa = System.Convert.ToBase64String(Params.logo);

                            ListEmp.Add(Temp);        
                        }
                        connec.Close();
                    }

                }

            }
            catch (Exception e)
            {
                //temp.pw_activo = -2;
                //temp.locode = e.Message;
            }

            return ListEmp;
        }




        public static IEnumerable<master_aimar.Entities.Entities.empresas> GetEmpresas(int id_empresa, string connstring)
        {
            List<master_aimar.Entities.Entities.empresas> ListEmp = new List<master_aimar.Entities.Entities.empresas>();

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;

                using (var connec = new NpgsqlConnection(connStr))
                {
                    using (var coman = new NpgsqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;

                        if (id_empresa == 0)
                            coman.CommandText = "select * from empresas where activo = 't' order by id_empresa";
                        else
                            coman.CommandText = "select * from empresas where id_empresa = " + id_empresa + " and activo = 't' order by id_empresa";
                        //coman.CommandTimeout = 0;
                        NpgsqlDataReader reader = coman.ExecuteReader();

                        while (reader.Read())
                        {
                            master_aimar.Entities.Entities.empresas Temp = new master_aimar.Entities.Entities.empresas();

                            try
                            {
                                Temp.activo = Convert.ToBoolean(reader["activo"]);
                                Temp.id_empresa = Convert.ToInt32(reader["id_empresa"]);
                                Temp.nombre_empresa = Convert.ToString(reader["nombre_empresa"]);
                                Temp.nombre_pais = Convert.ToString(reader["nombre_pais"]);
                                Temp.pais_iso = Convert.ToString(reader["pais_iso"]);
                                ListEmp.Add(Temp);
                            }
                            catch (Exception e)
                            {
                                string msg = e.Message;
                            }
                        }
                        connec.Close();
                    }

                }

            }
            catch (Exception e)
            {
                string msg = e.Message;                
            }

            return ListEmp;
        }





        public struct parametros
        {
            public string country;
            public string plantilla;
            public string edicion;
            public string titulo;
            public string nombre_empresa;
            public string nit;
            public string direccion;
            public string telefonos;            
            //trackactivo trackpuerto, a.trackmailserver, a.trackauth, a.trackfromaddress, a.trackpassword, a.home_page, a.firma, a.                            
            public string home_page;
            public string firma;
            public string manifest_user;
            public string manifest_pass;
            public string manifest_code;
            public string manifest_emisor;
            /*public string ftp_server;
            public string ftp_user;
            public string ftp_pass;*/
            public byte[] logo;
        }





        public static parametros EmpresaParametros(string pais, string docid, string sistema)
        {
            parametros Params = new parametros();

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["master-aimar"].ConnectionString;

                using (var connec = new NpgsqlConnection(connStr))
                {
                    using (var comm = new NpgsqlCommand())
                    {
                        connec.Open();
                        comm.Connection = connec;

                        NpgsqlDataReader reader; // = comm.ExecuteReader();

                        try
                        {
                            int result = int.Parse(pais);
                            comm.CommandText = "SELECT pais_iso FROM empresas WHERE id_empresa = " + result + " LIMIT 1";
                            reader = comm.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("pais_iso")))
                                    pais = reader["pais_iso"].ToString();
                            }
                        }
                        catch (Exception e)
                        {

                        }

                        string default_image = "iVBORw0KGgoAAAANSUhEUgAAAHMAAAB8CAYAAABaFY8zAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAABMSSURBVHhe7Z13rBRFHMcHsKNREXtvIFbEgoolMSKW2LAQDWCLLZKIYCwgSFSsKLZEwBK7EZVEo+gfioANjCUq9gYIighYURThuZ8f+z2H9e7d3Xs39+727TfZ3O3s7OzM/OZXZ3a2TUMElyEVaBv/ZkgBMmKmCBkxU4SMmClCRswUISNmihDENaHINm3axGcrzlsr/H4IjSCcWc0GZPgPQYMGf/31l5s0aVJ8lqFnz55ulVVWic8qj6BiFmJ26tTJrbnmmvGV1oF//vnnf0SbM2eOW7hwoVt99dXjlMojOGfusccebt1113V///23a9eunaUvW7bMftMItTGJuXPnuu+++y6oCgpKTEborrvumiNmx44d4yutBwsWLHDt27d3X331VTqIiZg99NBD3TXXXBNfaR2AS2+55RY3YcIE48xvv/22fnXmH3/84bp16+bWXntt16tXLzdy5Mg4R+vB0KFD3cSJE92iRYvczJkzg3JmUNekkP7IEAZBI0AZMauLIMSU5K43q5V6B9A6VUMWAUqAur/zzjtu8ODB7sADD3Tdu3c3vffll1/a9VomdiZmPUDIJ5980vXu3ds9//zzbsmSJSZdnnjiCXfEEUe4N954o6YHalBi1hs++ugjd8kll7gNN9zQLHANxg022MCOs846y/3www+WVouoG50pfRZSzN1///32CxE5VH/9/vjjj27q1Kn2vxZRNzqTMlUuwYgQRCVK06FDByNeciByvtVWW7kvvvgiTqk91KXOJGCNOCQoUQkkB0ahev/555/xv9pEUGImR3e5yMd9dOh5553nbr31VjdlypQ4tXkQx++7777ut99+s/8+QWnHGmusYTMfe+65Z5xaHohNh0bN60yVJdF68cUXu+nTp7sePXq4YcOGuZ9++smuC/kGQKno16+f/dLx6nyIutpqq1nAfP/997cYc1OeQRmhEVRnNrcB0pN0HgHq66+/3r388sumuxYvXux+//13d/fdd8e5V0DPbgq23HJLc0Oo97x586z8X375xXzMnXbayT3yyCNVIUpTEVTMVgJwJAS655573J133mkuAhwPx+A+MCsh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";
                        Params.logo = System.Convert.FromBase64String(default_image);

                        comm.CommandText = "SELECT a.*, b.edicion, b.titulo, b.observaciones, c.descripcion " +
                        "FROM empresas_parametros a LEFT JOIN empresas_plantillas b ON a.country = b.country AND CAST(b.activo AS text) = 'true' AND b.doc_id = '" + docid + "' AND b.sistema = '" + sistema + "' LEFT JOIN empresas_plantillas_docs c ON b.doc_id = c.doc_id " +
                        "WHERE CAST(a.activo AS text) = 'true' AND a.country = '" + pais + "'";
                        reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            int col = reader.GetOrdinal("logo");
                            if (!reader.IsDBNull(col))
                                if (reader["logo"].ToString() != "")                                
                                    Params.logo = System.Convert.FromBase64String(reader["logo"].ToString());                                                                   

                            if (!reader.IsDBNull(reader.GetOrdinal("descripcion")))
                                Params.plantilla = reader["descripcion"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("edicion")))
                                Params.edicion = reader["edicion"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("titulo")))
                                Params.titulo = reader["titulo"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_empresa")))
                                Params.nombre_empresa = reader["nombre_empresa"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("nit")))
                                Params.nit = reader["nit"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("direccion")))
                                Params.direccion = reader["direccion"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("telefonos")))
                                Params.telefonos = reader["telefonos"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("home_page")))
                                Params.home_page = reader["home_page"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("firma")))
                                Params.firma = reader["firma"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("manifest_codigo")))
                                Params.manifest_user = reader["manifest_codigo"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("manifest_pass")))
                                Params.manifest_pass = reader["manifest_pass"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("manifest_tipo")))
                                Params.manifest_code = reader["manifest_tipo"].ToString();

                            if (!reader.IsDBNull(reader.GetOrdinal("manifest_emisor")))
                                Params.manifest_emisor = reader["manifest_emisor"].ToString();
                        }
                    }


                }

            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return Params;
        }



    }

}
