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

            if (pais == 1)
                connStr = ConfigurationManager.ConnectionStrings["ventas_gt"].ConnectionString;
            else
                connStr = ConfigurationManager.ConnectionStrings["ventas_gtltf"].ConnectionString;

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

            if (pais == 1)
                connStr = ConfigurationManager.ConnectionStrings["ventas_gt"].ConnectionString;
            else
                connStr = ConfigurationManager.ConnectionStrings["ventas_gtltf"].ConnectionString;

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
    }
}
