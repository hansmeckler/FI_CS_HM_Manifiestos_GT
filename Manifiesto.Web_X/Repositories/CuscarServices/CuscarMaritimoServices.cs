using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifiesto.Data.Models;
using ventas_gt.Entities;
using ventas_gt.Entities.Entities;
using master_aimar.Entities;
using System.Data.Entity;
using System.Web;
using aimar_baw;

namespace Manifiesto.Data.Trafico
{
    public class CuscarMaritimoServices : ICuscarMaritimoServices
    {
        int pais = Convert.ToInt32(HttpContext.Current.Session["id_empresa"]);
        long id_usuario = Convert.ToInt64(HttpContext.Current.Session["id_usuario"]);

        public IEnumerable<listado_bls_VM> getListBls()
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                //int year = DateTime.Now.Year - 1;
                //int month = DateTime.Now.AddMonths(-3).Month;
                //int monthA = DateTime.Now.Month - 3;
                DateTime date = DateTime.Today.AddMonths(-24).Date;
                string adate = date.ToString("dd/MM/yyyy HH:mm:ss");
                //if (monthA == 0)
                    //monthA = 01;

                string query = "select 'LCL' as tipo, max(no_viaje) as no_viaje, " +
                                "max(viaje_contenedor.mbl) as no_bl, " +
                                "max(fecha_arribo) as fecha_arribo, " +
                                "max(vapor) as vapor, " +
                                "max(viajes.viaje_id) as id, " +
                                "count(distinct bill_of_lading.bl_id) as count1, " +
                                "count(distinct viaje_contenedor.viaje_contenedor_id) as count2, " +
                                "CASE WHEN viajes.import_export = true THEN 'IMPORT' ELSE 'EXPORT' END import_export " +
                                "from viajes " +
                                "join viaje_contenedor on (viaje_contenedor.viaje_id = viajes.viaje_id) " +
                                "join bill_of_lading on (bill_of_lading.viaje_contenedor_id=viaje_contenedor.viaje_contenedor_id) " +
                                "where viaje_contenedor.mbl<>'PENDIENTE' " +
                                "and fecha_arribo>='" + adate + "'::date " +
                                "group by viajes.viaje_id, viajes.activo having viajes.activo " +
                                "UNION " +
                                "select 'FCL' as tipo, " +
                                "max(no_viaje) as no_viaje, " +
                                "max(bl_completo.mbl) as no_bl, " +
                                "max(fecha_arribo) as fecha_arribo, " +
                                "max(vapor) as vapor, " +
                                "max(bl_completo.bl_id) as id, " +
                                "count(distinct bl_completo.bl_id) as count1, " +
                                "count(distinct contenedor_completo.contenedor_id) as count2, " +
                                "CASE WHEN bl_completo.import_export = true THEN 'IMPORT' ELSE 'EXPORT' END import_export " +
                                "from bl_completo " +
                                "join contenedor_completo on (contenedor_completo.bl_id=bl_completo.bl_id) " +
                                "where bl_completo.mbl<>'PENDIENTE' " +
                                "and fecha_arribo>='" + adate + "'::date  " +
                                "group by bl_completo.bl_id , bl_completo.activo having bl_completo.activo " +
                                "order by fecha_arribo desc";
                var list = context.Database.SqlQuery<listado_bls_VM>(query).ToList();

                return list;
            }
        }

        public IEnumerable<Comodity> get_commodity_by_service_type(int id, string tipo_servicio)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            using (var context = new ventas_gtEntities(conexion))
            {
                if (tipo_servicio == "LCL")
                {
                    string query = "select bl.comodity_id as comidity_id FROM bill_of_lading bl INNER JOIN viaje_contenedor vc  on (vc.viaje_contenedor_id = bl.viaje_contenedor_id) " +
                                   "INNER JOIN viajes v  on (vc.viaje_id = v.viaje_id) WHERE bl.activo And vc.activo And v.activo And vc.viaje_id = " + id + " group by comodity_id ";
                    var comodity = context.Database.SqlQuery<Comodity>(query).ToList();
                    return comodity;
                }
                else 
                {
                    string query = "select vc.comodity_id as comidity_id FROM bl_completo as blf INNER JOIN contenedor_completo  as vc on (vc.bl_id = blf.bl_id) where blf.bl_id = " + id + " group by vc.comodity_id; ";
                    var comodity = context.Database.SqlQuery<Comodity>(query).ToList();
                    return comodity;
                }
            }
        }
        
        public bool validate_size_of_name_commodities(int id, string tipo_servicio)
        {
            IEnumerable<Comodity> comodity = get_commodity_by_service_type(id, tipo_servicio);
            bool result = false;
            
            foreach (var c_id in comodity)
            {
                if (c_id.comidity_id == ' ' && c_id.comidity_id == null)
                    result = false;
                else
                    result = true;
            }
            return result;
        }

        // Add BL Info
        public void add_bl_info(long id, long nextId, string tipo_servicio)
        {
            string conexion = Admin.AdminService.swich_context(pais);
            var context_ventas = new ventas_gtEntities(conexion);           

            var unlocode = Admin.AdminService.getUnlocode().ToList();
            var commodities = Admin.AdminService.getCommoditiesList().ToList();
            var tipo_paquete = Admin.AdminService.getTipoPaqueteList().ToList();

            //using (var dbContextTransaction = context_ventas.Database.BeginTransaction())
            //{
                //try
                //{
                    // LCL
                    if (tipo_servicio == "LCL")
                    {
                        var viaje_contenedor = context_ventas.viaje_contenedor.Where(a => a.viaje_id == id).ToList();

                        var cuscar_container_id = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == nextId).Select(a => a.cuscar_container_id).ToList();
                        int i = -1;
                        foreach (var v in viaje_contenedor)
                        {
                            var bill_of_lading = context_ventas.bill_of_lading.Where(a => a.viaje_contenedor_id == v.viaje_contenedor_id && a.activo == true).ToList();

                            i++;
                            foreach (var bill in bill_of_lading)
                            {

                                var insert_bl = (from a in bill_of_lading
                                                    join b in unlocode on a.id_puerto_embarque equals b.unlocode_id
                                                    join c in unlocode on a.id_destino_final equals c.unlocode_id
                                                    join d in tipo_paquete on a.id_tipo_paquete equals d.tipo_id
                                                    join e in commodities on a.comodity_id equals e.commodityid
                                                    where a.bl_id == bill.bl_id
                                                    select new cuscar_bl_info
                                                    {
                                                        cuscar_viaje_id = nextId,
                                                        cuscar_container_id = cuscar_container_id[i],
                                                        no_bl = a.no_bl,
                                                        id_shipper = Convert.ToInt32(a.id_shipper),
                                                        id_cliente = Convert.ToInt32(a.id_cliente),
                                                        id_puerto_embarque = Convert.ToInt32(a.id_puerto_embarque),
                                                        id_destino_final = Convert.ToInt32(a.id_destino_final),
                                                        no_piezas = Convert.ToInt32(a.no_piezas),
                                                        id_tipo_paquete = Convert.ToInt32(a.id_tipo_paquete),
                                                        comodity_id = a.comodity_id,
                                                        peso = Convert.ToSingle(a.peso),
                                                        volumen = Convert.ToSingle(a.volumen),
                                                        ono_bl = a.no_bl,
                                                        tipo_conocimiento_id = a.tipo_conocimiento_id,
                                                        tipo_identificacion_id = a.tipo_identificacion_id,
                                                        observaciones = a.observaciones,
                                                        marks_and_numbers = a.marks_and_numbers,
                                                        tipo_merc_p = a.tipo_merc_p,
                                                        id_aduana = Convert.ToInt32(a.id_aduana),
                                                        puerto_embarque = b.pais + "" + b.locode,
                                                        destino_final = c.pais,
                                                        comodity = e.namees,
                                                        shipper = Admin.AdminService.getNombreCliente(a.id_shipper) + " " + Admin.AdminService.getDireccionCliente(a.id_shipper),
                                                        cliente = Admin.AdminService.getNombreCliente(a.id_cliente),
                                                        tipo_paquete = d.tipo,
                                                        bl_id = Convert.ToInt32(a.bl_id),
                                                        codigo_tributario = Admin.AdminService.getCodigoTributario(bill.id_cliente).Replace("-", ""),
                                                        flete = a.valor_flete_manifestar,
                                                        tipo_docto = 1,
                                                        ttm_id = 1,
                                                        direccion_cliente = Admin.AdminService.getDireccionCliente(a.id_cliente),
                                                        selected2 = false,
                                                        id_routing = Convert.ToInt32(a.id_routing),
                                                        valor_flete_manual = a.valor_flete_manual
                                                    }).FirstOrDefault();

                                context_ventas.cuscar_bl_info.Add(insert_bl);
                                context_ventas.SaveChanges();
                                //dbContextTransaction.Commit();
                            }
                        }
                    }
                    else // FCL 
                    {
                        var bl_completo = context_ventas.bl_completo.Where(a => a.bl_id == id || a.bl_id_ref == id && a.activo == true).ToList();

                        var cuscar_container_info = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == nextId).ToList();

                        foreach (var bl in bl_completo)
                        {
    
                           var insert_bl = (from a in bl_completo
                                                where a.bl_id == bl.bl_id
                                                join b in unlocode on a.id_puerto_embarque equals b.unlocode_id
                                                join c in unlocode on a.id_destino_final equals c.unlocode_id
                                                select new cuscar_bl_info
                                                {
                                                    cuscar_viaje_id = nextId,
                                                    cuscar_container_id = 0,
                                                    no_bl = a.no_bl,
                                                    mbl = a.mbl,
                                                    id_shipper = Convert.ToInt32(a.id_shipper),
                                                    id_cliente = Convert.ToInt32(a.id_cliente),
                                                    id_puerto_embarque = Convert.ToInt32(a.id_puerto_embarque),
                                                    id_destino_final = Convert.ToInt32(a.id_destino_final),
                                                    no_piezas = a.no_piezas,
                                                    id_tipo_paquete = 1,
                                                    comodity_id = a.comodity_id,
                                                    peso = Convert.ToSingle(a.peso),
                                                    volumen = Convert.ToSingle(a.volumen),
                                                    ono_bl = a.no_bl,
                                                    tipo_conocimiento_id = a.tipo_conocimiento_id,
                                                    tipo_identificacion_id = a.tipo_identificacion_id,
                                                    observaciones = a.observaciones,
                                                    marks_and_numbers = "no marks",
                                                    id_aduana = Convert.ToInt32(a.id_aduana),
                                                    puerto_embarque = b.pais + "" + b.locode,
                                                    destino_final = c.pais,
                                                    shipper = Admin.AdminService.getNombreCliente(a.id_shipper) + " " + Admin.AdminService.getDireccionCliente(a.id_shipper),
                                                    cliente = Admin.AdminService.getNombreCliente(a.id_cliente),
                                                    tipo_paquete = "CAJAS",
                                                    bl_id = Convert.ToInt32(bl.bl_id),
                                                    codigo_tributario = Admin.AdminService.getCodigoTributario(a.id_cliente).Replace("-", ""),
                                                    flete = a.valor_flete_manifestar,
                                                    tipo_docto = 1,
                                                    ttm_id = 1,
                                                    direccion_cliente = Admin.AdminService.getDireccionCliente(a.id_cliente),
                                                    selected2 = false,
                                                    id_routing = Convert.ToInt32(a.id_routing ?? 0),
                                                    valor_flete_manual = a.valor_flete_manual
                                                }).FirstOrDefault();


                            context_ventas.cuscar_bl_info.Add(insert_bl);
                            context_ventas.SaveChanges();
                            //dbContextTransaction.Commit();
                        }

                    }
                    context_ventas.Dispose();
                //}
                //catch (Exception)
                //{
                 //   dbContextTransaction.Rollback();
                //}
            //}
            
        }
      
        // Add container Info
        public void add_container_info(long id, long nextId, string tipo_servicio)
        {
            string conexion = Admin.AdminService.swich_context(pais);

            var context_ventas = new ventas_gtEntities(conexion);

                    if (tipo_servicio == "LCL")
                    {
                        var get_containers = context_ventas.viaje_contenedor.Where(a => a.viaje_id == id).ToList();

                        foreach (var a in get_containers)
                        {
                            var insert_container = (from p in get_containers
                                                    where p.viaje_id == id && p.activo
                                                    orderby p.viaje_contenedor_id
                                                    select new cuscar_container_info
                                                    {
                                                        cuscar_viaje_id = nextId,
                                                        id_tipo_paquete = 1,
                                                        comodity_id = 1,
                                                        no_contenedor = a.no_contenedor,
                                                        seal = a.seal,
                                                        id_container_type = Convert.ToInt32(a.id_container_type),
                                                        warehouse = a.warehouse,
                                                        contenedor_id = Convert.ToInt32(p.viaje_contenedor_id),
                                                        mbl = a.mbl,
                                                        bl_id = 0,
                                                        manifest = "NA",
                                                        original = "NA",
                                                        cuscar = "NA",
                                                        mtype = 785,
                                                        mfunction = 9,
                                                        operation = 23,
                                                        funcsend = "DC",
                                                        id_status = 1,
                                                        no_piezas = 1,
                                                        volumen = 0,
                                                        cuscardt = "0",
                                                        test = 6,
                                                        no_bls = 0,
                                                        viaje_id = Convert.ToInt32(a.viaje_id),
                                                        bl_id_ref = 0
                                                    }).FirstOrDefault();

                            context_ventas.cuscar_container_info.Add(insert_container);
                            context_ventas.SaveChanges();
                        }

                        var containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == nextId).ToList();

                        var container_type = Admin.AdminService.getContainerTypeList();
                        var commodities = Admin.AdminService.getCommoditiesList();
                        var tipo_paquete = Admin.AdminService.getTipoPaqueteList();

                        foreach (var c in containers)
                        {
                            if (c.no_contenedor == null || c.no_contenedor == "")
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join cc in commodities on aa.comodity_id equals Convert.ToInt32(cc.commodityid)
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = "",
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = cc.namees,
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              viaje_id = aa.viaje_id,
                                                                              container_type = "",
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();
                            }
                            else
                            {


                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join cc in commodities on aa.comodity_id equals Convert.ToInt32(cc.commodityid)
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = aa.no_contenedor.Replace("-", ""),
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = cc.namees,
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              viaje_id = aa.viaje_id,
                                                                              container_type = bb.short_name,
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();
                            }

                            var u_containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_container_id == c.cuscar_container_id).ToList();
                            update_container_manifest_number(u_containers, context_ventas, c.cuscar_container_id);
                        }
                    }
                    else // FCL
                    {
                        long? bl_id = 0;

                        var get_containers = context_ventas.contenedor_completo.Where(a => a.bl_id == id && a.activo == true).ToList();

                        var tipo_paquete = Admin.AdminService.getTipoPaqueteList();

                        foreach (var a in get_containers)
                        {
                            if (a.bl_id_ref == 0)
                                bl_id = a.bl_id;
                            else
                                bl_id = a.bl_id_ref;

                            var insert_container = (from p in get_containers
                                                    where p.contenedor_id == a.contenedor_id
                                                    join pa in tipo_paquete on p.id_tipo_paquete equals pa.tipo_id
                                                    select new cuscar_container_info
                                                    {
                                                        cuscar_viaje_id = nextId,
                                                        no_contenedor = a.no_contenedor,
                                                        seal = a.seal,
                                                        id_container_type = Convert.ToInt32(a.id_container_type),
                                                        no_piezas = Convert.ToInt32(a.no_piezas),
                                                        id_tipo_paquete = Convert.ToInt32(p.id_tipo_paquete),
                                                        peso = a.peso,
                                                        volumen = Convert.ToSingle(a.volumen),
                                                        comodity_id = Convert.ToInt32(p.comodity_id),
                                                        warehouse = a.warehouse,
                                                        bl_id = Convert.ToInt32(p.bl_id),
                                                        no_bls = 0,
                                                        manifest = "NA",
                                                        original = "NA",
                                                        cuscar = "NA",
                                                        mtype = 785,
                                                        mfunction = 9,
                                                        operation = 23,
                                                        funcsend = "DC",
                                                        id_status = 1,
                                                        test = 6,
                                                        contenedor_id = Convert.ToInt32(a.contenedor_id),
                                                        bl_id_ref = Convert.ToInt32(bl_id)
                                                    }).FirstOrDefault();

                            context_ventas.cuscar_container_info.Add(insert_container);
                            context_ventas.SaveChanges();
                        }

                        var containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == nextId).ToList();

                        var container_type = Admin.AdminService.getContainerTypeList();
                        var commodities = Admin.AdminService.getCommoditiesList();

                        foreach (var c in containers)
                        {
                            if (c.no_contenedor == null || c.no_contenedor == "")
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join cc in commodities on aa.comodity_id equals Convert.ToInt32(cc.commodityid)
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = "",
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = cc.namees,
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              container_type = "",
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              peso = aa.peso,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).Where(a => a.cuscar_container_id == c.cuscar_container_id).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();
      
                            }
                            else
                            {

                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join cc in commodities on aa.comodity_id equals Convert.ToInt32(cc.commodityid)
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = aa.no_contenedor.Replace("-", ""),
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = cc.namees,
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              container_type = bb.short_name,
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              peso = aa.peso,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();

                               
                            }

                            var u_containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_container_id == c.cuscar_container_id).ToList();
                            update_container_manifest_number(u_containers, context_ventas, c.cuscar_container_id);
                        }
                    }
                    context_ventas.Dispose();
        }

        public void update_container_manifest_number(IList<cuscar_container_info> u_containers, DbContext context_ventas, long id)
        {
            int lastcanc = -1;

            string original = "";

            foreach (var uc in u_containers)
            {
                if (uc.mfunction == 1 || uc.mfunction == 10)
                    lastcanc = u_containers.Count();
            }

            if (lastcanc == -1)
                original = u_containers.Select(a => a.manifest).FirstOrDefault();
            else if (lastcanc < u_containers.Count())
                original = u_containers.Select(a => a.manifest).FirstOrDefault();
            else if (lastcanc == u_containers.Count())
                original = "NA";

            context_ventas.Database.ExecuteSqlCommand("Update cuscar_container_info set original = '" + original + "', manifest = '" + u_containers.Select(a => a.manifest).FirstOrDefault() + "' where cuscar_container_id = " + id);
        }
        
        // Add voyage FLC info
        public long add_viaje_info_fcl(long id, string tipo_servicio)
        {
            string conexion = Admin.AdminService.swich_context(pais);
            var context_ventas = new ventas_gtEntities(conexion);

            long newIdentityValue = 0;

            var navieras = Admin.AdminService.getNavieras();
            var unlocode = Admin.AdminService.getUnlocode();


                    var bl_completo = context_ventas.bl_completo.Where(a => a.bl_id == id).ToList();

                    var viaje_info = (from a in bl_completo
                                      join b in navieras on a.id_naviera equals b.id_naviera
                                      join c in unlocode on a.id_puerto_origen equals c.unlocode_id
                                      join d in unlocode on a.id_puerto_desembarque equals d.unlocode_id
                                      where a.bl_id == id
                                      select new cuscar_viaje_info
                                      {
                                          viaje_id = Convert.ToInt64(a.bl_id),
                                          no_viaje = a.no_viaje,
                                          vapor = a.vapor,
                                          id_naviera = Convert.ToInt32(a.id_naviera),
                                          id_puerto_origen = Convert.ToInt32(a.id_puerto_origen),
                                          id_puerto_desembarque = Convert.ToInt32(a.id_puerto_desembarque),
                                          naviera = b.nombre,
                                          puerto_origen = c.pais + "" + c.locode,
                                          puerto_desembarque = d.pais + "" + d.locode,
                                          tipo = "FCL",
                                          fecha_arribo = a.fecha_arribo,
                                          eta = a.eta,
                                          etd = a.etd,
                                          import_export = a.import_export,
                                          id_usuario = id_usuario,
                                          date_created = DateTime.Now,
                                          id_deposito = Convert.ToInt32(a.id_deposito)
                                      }).FirstOrDefault();

                    context_ventas.cuscar_viaje_info.Add(viaje_info);
                    context_ventas.SaveChanges();
                    //dbContextTransaction.Commit();

                    newIdentityValue = context_ventas.cuscar_viaje_info.Select(a => a.cuscar_viaje_id).Max();

                    context_ventas.Dispose();

                    add_container_info(id, newIdentityValue, tipo_servicio);
                    add_bl_info(id, newIdentityValue, tipo_servicio);

                    return newIdentityValue;

        }

        // Add voyage LCL info
        public long add_viaje_info_lcl(long id, string tipo_servicio)
        {
            long newIdentityValue = 0;

            string conexion = Admin.AdminService.swich_context(pais);

            var context_ventas = new ventas_gtEntities(conexion);

            var navieras = Admin.AdminService.getNavieras();
            var unlocode = Admin.AdminService.getUnlocode();

            var viajes = context_ventas.viajes.Where(a => a.viaje_id == id).ToList();

            var viaje_info = (from a in viajes
                                join b in navieras on a.id_naviera equals b.id_naviera
                                join c in unlocode on a.id_puerto_origen equals c.unlocode_id
                                join d in unlocode on a.id_puerto_desembarque equals d.unlocode_id
                                where a.viaje_id == id
                                select new cuscar_viaje_info
                                {
                                    viaje_id = a.viaje_id,
                                    no_viaje = a.no_viaje,
                                    vapor = a.vapor,
                                    id_naviera = Convert.ToInt32(a.id_naviera),
                                    id_puerto_origen = Convert.ToInt32(a.id_puerto_origen),
                                    id_puerto_desembarque = Convert.ToInt32(a.id_puerto_desembarque),
                                    naviera = b.nombre,
                                    puerto_origen = c.pais + "" + c.locode,
                                    puerto_desembarque = d.pais + "" + d.locode,
                                    tipo = "LCL",
                                    fecha_arribo = a.fecha_arribo,
                                    eta = a.eta,
                                    etd = a.etd,
                                    import_export = a.import_export,
                                    id_usuario = id_usuario,
                                    date_created = DateTime.Now,
                                    id_deposito = Convert.ToInt32(a.id_deposito)
                                }).FirstOrDefault();

            context_ventas.cuscar_viaje_info.Add(viaje_info);
            context_ventas.SaveChanges();

            newIdentityValue = context_ventas.cuscar_viaje_info.Select(a => a.cuscar_viaje_id).Max();

            context_ventas.Dispose();

            add_container_info(id, newIdentityValue, tipo_servicio);
            add_bl_info(id, newIdentityValue, tipo_servicio);

            return newIdentityValue;
                
           
        }

        // Add voyage Info
        public long add_viaje_info(long id, string tipo_servicio)
        {
            long newIdentityValue = 0;

            if (tipo_servicio == "LCL")
                newIdentityValue = add_viaje_info_lcl(id, tipo_servicio);
            else
                newIdentityValue = add_viaje_info_fcl(id, tipo_servicio);

            return newIdentityValue;

        }

        // Crea Cuscar Maritimo
        public long add_cuscar(int id, string tipo_servicio)
        {
            long nextId = add_viaje_info(id, tipo_servicio);
            return nextId;
        }

        // Generacion informacion de manifiesto
        public long add_cuscar2(int id, string tipo_servicio)
        {
            long newIdentityValue = 0;

            string conexion = Admin.AdminService.swich_context(pais);
            var context_ventas = new ventas_gtEntities(conexion);

            var navieras = Admin.AdminService.getNavieras();

            using (var dbContextTransaction = context_ventas.Database.BeginTransaction())
            {
                try
                {
                    if (tipo_servicio == "LCL")
                    {
                        //Obtiene Viaje
                        var viajes = context_ventas.viajes.Where(a => a.viaje_id == id).ToList();

                        //Inserta Viaje
                        var viaje_info = (from a in viajes
                                          join b in navieras on a.id_naviera equals b.id_naviera
                                          where a.viaje_id == id
                                          select new cuscar_viaje_info
                                          {
                                              viaje_id = a.viaje_id,
                                              no_viaje = a.no_viaje,
                                              vapor = a.vapor,
                                              id_naviera = Convert.ToInt32(a.id_naviera),
                                              id_puerto_origen = Convert.ToInt32(a.id_puerto_origen),
                                              id_puerto_desembarque = Convert.ToInt32(a.id_puerto_desembarque),
                                              naviera = b.nombre,
                                              puerto_origen = Admin.AdminService.getPais(a.id_puerto_origen) + "" + Admin.AdminService.getlocode(a.id_puerto_origen),
                                              puerto_desembarque = Admin.AdminService.getPais(a.id_puerto_desembarque) + "" + Admin.AdminService.getlocode(a.id_puerto_desembarque),
                                              tipo = "LCL",
                                              fecha_arribo = a.fecha_arribo,
                                              eta = a.eta,
                                              etd = a.etd,
                                              import_export = a.import_export,
                                              id_usuario = id_usuario,
                                              date_created = DateTime.Now,
                                              id_deposito = Convert.ToInt32(a.id_deposito)
                                          }).FirstOrDefault();

                        context_ventas.cuscar_viaje_info.Add(viaje_info);
                        context_ventas.SaveChanges();

                        //Nuevo Id Viaje para relacionar contenedores
                        newIdentityValue = context_ventas.cuscar_viaje_info.Select(a => a.cuscar_viaje_id).Max();

                        //Obtiene Contenedores
                        var get_containers = context_ventas.viaje_contenedor.Where(a => a.viaje_id == id).ToList();

                        //Recorre e inserta contendores
                        foreach (var a in get_containers)
                        {
                            var insert_container = (from p in get_containers
                                                    where p.viaje_id == id && p.activo
                                                    orderby p.viaje_contenedor_id
                                                    select new cuscar_container_info
                                                    {
                                                        cuscar_viaje_id = newIdentityValue,
                                                        id_tipo_paquete = 1,
                                                        comodity_id = 1,
                                                        no_contenedor = a.no_contenedor,
                                                        seal = a.seal,
                                                        id_container_type = Convert.ToInt32(a.id_container_type),
                                                        warehouse = a.warehouse,
                                                        contenedor_id = Convert.ToInt32(p.viaje_contenedor_id),
                                                        mbl = a.mbl,
                                                        bl_id = 0,
                                                        manifest = "NA",
                                                        original = "NA",
                                                        cuscar = "NA",
                                                        mtype = 785,
                                                        mfunction = 9,
                                                        operation = 23,
                                                        funcsend = "DC",
                                                        id_status = 1,
                                                        no_piezas = 1,
                                                        volumen = 0,
                                                        cuscardt = "0",
                                                        test = 6,
                                                        no_bls = 0,
                                                        viaje_id = Convert.ToInt32(a.viaje_id),
                                                        bl_id_ref = 0
                                                    }).FirstOrDefault();

                            context_ventas.cuscar_container_info.Add(insert_container);
                            context_ventas.SaveChanges();
                        }

                        //Actualiza y completa informacion de contenedores que ya se insertaron

                        //Obtiene tipo de contenedores
                        var container_type = Admin.AdminService.getContainerTypeList();
                        //Obtiene tipos de paquqte
                        var tipo_paquete = Admin.AdminService.getTipoPaqueteList();

                        //Obtiene nuevos contenedores ya ingresados para hacer join con tipo de contenedores y tipos de paquete y actualizar esta informacion
                        var containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == newIdentityValue).ToList();

                        foreach (var c in containers)
                        {
                            if (c.no_contenedor == null || c.no_contenedor == "")
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = "",
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = Admin.AdminService.getCommodityNamees(aa.comodity_id),
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              viaje_id = aa.viaje_id,
                                                                              container_type = "",
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();
                            }
                            else
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = aa.no_contenedor.Replace("-", ""),
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = Admin.AdminService.getCommodityNamees(aa.comodity_id),
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              viaje_id = aa.viaje_id,
                                                                              container_type = bb.short_name,
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();
                            }
                        } // fin foreach

                        // Informacion para el BL
                        var viaje_contenedor = context_ventas.viaje_contenedor.Where(a => a.viaje_id == id).ToList();

                        var cuscar_container_id = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == newIdentityValue).Select(a => a.cuscar_container_id).ToList();
                        int i = -1;
                        foreach (var v in viaje_contenedor)
                        {
                            var bill_of_lading = context_ventas.bill_of_lading.Where(a => a.viaje_contenedor_id == v.viaje_contenedor_id && a.activo == true).ToList();
                            i++;
                            foreach (var bill in bill_of_lading)
                            {
                                List<decimal?> facturados = new List<decimal?>();
                                int manifestar = 1;
                                if (bill.id_routing > 0 && bill.valor_flete_manual == true) //solo busque que esté facturado
                                {
                                    facturados = contabilizacion_sca(bill.bl_id, 1, 2);
                                }
                                else if (bill.id_routing > 0 && bill.valor_flete_manual == false) //busque que esté facturado y también coincida el valor de flete a manifestar con la suma de rubros en dólares int a cliente
                                {
                                    facturados = contabilizacion_sca(bill.bl_id, 2, 2);
                                }
                                else if (bill.id_routing == 0 || bill.id_routing == null) //solo busque que esté facturado
                                {
                                    facturados = contabilizacion_sca(bill.bl_id, 3, 2);
                                }

                                decimal? suma_montos = null;

                                if (facturados.Count == 0)
                                {
                                    suma_montos = null;
                                }
                                else
                                {
                                    suma_montos = new decimal();
                                    suma_montos = facturados.Sum();
                                    if (bill.id_routing > 0 && bill.valor_flete_manual == true) //solo busque que esté facturado
                                    {
                                        manifestar = 0;
                                    }
                                    else if (bill.id_routing > 0 && bill.valor_flete_manual == false) //busque que esté facturado y también coincida el valor de flete a manifestar con la suma de rubros en dólares int a cliente
                                    {
                                        if (suma_montos == bill.valor_flete_manifestar)
                                        {
                                            manifestar = 0;
                                        }
                                    }
                                    else if (bill.id_routing == 0 || bill.id_routing == null) //solo busque que esté facturado
                                    {
                                        manifestar = 0;
                                    }

                                }

                                if (pais != 1 || viaje_info.import_export != true || bill.fecha_ingreso_sistema <= new DateTime(2018, 7, 2))
                                {
                                    manifestar = 0;
                                }

                                var insert_bl = (from a in bill_of_lading
                                                    join d in tipo_paquete on a.id_tipo_paquete equals d.tipo_id
                                                    where a.bl_id == bill.bl_id
                                                    select new cuscar_bl_info
                                                    {
                                                        cuscar_viaje_id = newIdentityValue,
                                                        cuscar_container_id = cuscar_container_id[i],
                                                        no_bl = a.no_bl,
                                                        id_shipper = Convert.ToInt32(a.id_shipper),
                                                        id_cliente = Convert.ToInt32(a.id_cliente),
                                                        id_puerto_embarque = Convert.ToInt32(a.id_puerto_embarque),
                                                        id_destino_final = Convert.ToInt32(a.id_destino_final),
                                                        no_piezas = Convert.ToInt32(a.no_piezas),
                                                        id_tipo_paquete = Convert.ToInt32(a.id_tipo_paquete),
                                                        comodity_id = a.comodity_id,
                                                        peso = Convert.ToSingle(a.peso),
                                                        volumen = Convert.ToSingle(a.volumen),
                                                        ono_bl = a.no_bl,
                                                        tipo_conocimiento_id = a.tipo_conocimiento_id,
                                                        tipo_identificacion_id = a.tipo_identificacion_id,
                                                        observaciones = a.observaciones,
                                                        marks_and_numbers = a.marks_and_numbers,
                                                        tipo_merc_p = a.tipo_merc_p,
                                                        id_aduana = Convert.ToInt32(a.id_aduana),
                                                        puerto_embarque = Admin.AdminService.getPais(a.id_puerto_embarque) + "" + Admin.AdminService.getlocode(a.id_puerto_embarque),
                                                        destino_final = Admin.AdminService.getPais(a.id_destino_final),
                                                        comodity = Admin.AdminService.getCommodityNamees(Convert.ToInt32(a.comodity_id)),
                                                        shipper = Admin.AdminService.getNombreCliente(a.id_shipper) + " " + Admin.AdminService.getDireccionCliente(a.id_shipper),
                                                        cliente = Admin.AdminService.getNombreCliente(a.id_cliente),
                                                        tipo_paquete = d.tipo,
                                                        bl_id = Convert.ToInt32(a.bl_id),
                                                        codigo_tributario = Admin.AdminService.getCodigoTributario(bill.id_cliente).Replace("-", ""),
                                                        flete = a.valor_flete_manifestar,
                                                        tipo_docto = 1,
                                                        ttm_id = 1,
                                                        direccion_cliente = Admin.AdminService.getDireccionCliente(a.id_cliente),
                                                        selected2 = false,
                                                        id_routing = Convert.ToInt32(a.id_routing),
                                                        valor_flete_manual = a.valor_flete_manual,
                                                        valor_cont_sca = suma_montos,
                                                        flag_manifestar = manifestar
                                                    }).FirstOrDefault();

                                context_ventas.cuscar_bl_info.Add(insert_bl);
                                context_ventas.SaveChanges();       
                            }
                        }
                    } // Fin if LCL
                    else 
                    {
                        //Obtiene BL 
                        var bl_completo = context_ventas.bl_completo.Where(a => a.bl_id == id).ToList();

                        //Inserta Informacion de viaje
                        var viaje_info = (from a in bl_completo
                                          join b in navieras on a.id_naviera equals b.id_naviera
                                          where a.bl_id == id
                                          select new cuscar_viaje_info
                                          {
                                              viaje_id = Convert.ToInt64(a.bl_id),
                                              no_viaje = a.no_viaje,
                                              vapor = a.vapor,
                                              id_naviera = Convert.ToInt32(a.id_naviera),
                                              id_puerto_origen = Convert.ToInt32(a.id_puerto_origen),
                                              id_puerto_desembarque = Convert.ToInt32(a.id_puerto_desembarque),
                                              naviera = b.nombre,
                                              puerto_origen = Admin.AdminService.getPais(a.id_puerto_origen) + "" + Admin.AdminService.getlocode(a.id_puerto_origen), 
                                              puerto_desembarque = Admin.AdminService.getPais(a.id_puerto_desembarque) + "" + Admin.AdminService.getlocode(a.id_puerto_desembarque),
                                              tipo = "FCL",
                                              fecha_arribo = a.fecha_arribo,
                                              eta = a.eta,
                                              etd = a.etd,
                                              import_export = a.import_export,
                                              id_usuario = id_usuario,
                                              date_created = DateTime.Now,
                                              id_deposito = Convert.ToInt32(a.id_deposito)
                                          }).FirstOrDefault();

                        context_ventas.cuscar_viaje_info.Add(viaje_info);
                        context_ventas.SaveChanges();

                        //Obtiene ultimo Id guardado
                        newIdentityValue = context_ventas.cuscar_viaje_info.Select(a => a.cuscar_viaje_id).Max();

                        long? bl_id = 0;

                        //Obtiene contenedores
                        var get_containers = context_ventas.contenedor_completo.Where(a => a.bl_id == id && a.activo == true).ToList();

                        var tipo_paquete = Admin.AdminService.getTipoPaqueteList();

                        //Recorre contenedores para insertarlos
                        foreach (var a in get_containers)
                        {
                            if (a.bl_id_ref == 0)
                                bl_id = a.bl_id;
                            else
                                bl_id = a.bl_id_ref;

                            var insert_container = (from p in get_containers
                                                    where p.contenedor_id == a.contenedor_id
                                                    join pa in tipo_paquete on p.id_tipo_paquete equals pa.tipo_id
                                                    select new cuscar_container_info
                                                    {
                                                        cuscar_viaje_id = newIdentityValue,
                                                        no_contenedor = a.no_contenedor,
                                                        seal = a.seal,
                                                        id_container_type = Convert.ToInt32(a.id_container_type),
                                                        no_piezas = Convert.ToInt32(a.no_piezas),
                                                        id_tipo_paquete = Convert.ToInt32(p.id_tipo_paquete),
                                                        peso = a.peso,
                                                        volumen = Convert.ToSingle(a.volumen),
                                                        comodity_id = Convert.ToInt32(p.comodity_id),
                                                        warehouse = a.warehouse,
                                                        bl_id = Convert.ToInt32(p.bl_id),
                                                        no_bls = 0,
                                                        manifest = "NA",
                                                        original = "NA",
                                                        cuscar = "NA",
                                                        mtype = 785,
                                                        mfunction = 9,
                                                        operation = 23,
                                                        funcsend = "DC",
                                                        id_status = 1,
                                                        test = 6,
                                                        contenedor_id = Convert.ToInt32(a.contenedor_id),
                                                        bl_id_ref = Convert.ToInt32(bl_id)
                                                    }).FirstOrDefault();

                            context_ventas.cuscar_container_info.Add(insert_container);
                            context_ventas.SaveChanges();
                        }

                        //Obtiene contenedores nuevos contenedores insertados
                        var containers = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == newIdentityValue).ToList();

                        var container_type = Admin.AdminService.getContainerTypeList();

                        foreach (var c in containers)
                        {
                            if (c.no_contenedor == null || c.no_contenedor == "")
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = "",
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = Admin.AdminService.getCommodityNamees(aa.comodity_id),
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              container_type = "",
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              peso = aa.peso,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).Where(a => a.cuscar_container_id == c.cuscar_container_id).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();

                            }
                            else
                            {
                                cuscar_container_info update_container = (from aa in containers
                                                                          join bb in container_type on aa.id_container_type equals bb.id_container_type
                                                                          join dd in tipo_paquete on aa.id_tipo_paquete equals Convert.ToInt32(dd.tipo_id)
                                                                          where aa.cuscar_container_id == c.cuscar_container_id
                                                                          select new cuscar_container_info
                                                                          {
                                                                              cuscar_viaje_id = aa.cuscar_viaje_id,
                                                                              cuscar_container_id = aa.cuscar_container_id,
                                                                              no_contenedor = aa.no_contenedor.Replace("-", ""),
                                                                              mbl = aa.mbl,
                                                                              seal = aa.seal,
                                                                              id_container_type = aa.id_container_type,
                                                                              no_piezas = aa.no_piezas,
                                                                              id_tipo_paquete = aa.id_tipo_paquete,
                                                                              tipo_paquete = dd.tipo,
                                                                              volumen = aa.volumen,
                                                                              comodity_id = aa.comodity_id,
                                                                              warehouse = aa.warehouse,
                                                                              comodity = Admin.AdminService.getCommodityNamees(aa.comodity_id),
                                                                              no_bls = aa.no_bls,
                                                                              bl_id = aa.bl_id,
                                                                              contenedor_id = aa.contenedor_id,
                                                                              container_type = bb.short_name,
                                                                              cuscar = aa.cuscar,
                                                                              cuscardt = aa.cuscardt,
                                                                              manifest = aa.manifest,
                                                                              original = aa.original,
                                                                              mtype = aa.mtype,
                                                                              mfunction = aa.mfunction,
                                                                              operation = aa.operation,
                                                                              funcsend = aa.funcsend,
                                                                              test = aa.test,
                                                                              peso = aa.peso,
                                                                              id_status = aa.id_status,
                                                                              bl_id_ref = aa.bl_id_ref
                                                                          }).FirstOrDefault();

                                context_ventas.Entry(c).CurrentValues.SetValues(update_container);
                                context_ventas.SaveChanges();


                            }
                        } //Fin foreach

                        //Informacion BL
                        var bl_completo_det = context_ventas.bl_completo.Where(a => a.bl_id == id || a.bl_id_ref == id && a.activo == true).ToList();

                        //var cuscar_container_info = context_ventas.cuscar_container_info.Where(a => a.cuscar_viaje_id == newIdentityValue).ToList();

                        foreach (var bl in bl_completo_det)
                        {
                            using (aimar_bawEntities aimar_baw = new aimar_bawEntities())
                            {
                                int manifestar = 1;
                                List<decimal?> facturados = new List<decimal?>();
                                if (bl.id_routing > 0 && bl.valor_flete_manual == true) //solo busque que esté facturado
                                {
                                    facturados = contabilizacion_sca(bl.bl_id, 1, 1);
                                }
                                else if (bl.id_routing > 0 && bl.valor_flete_manual == false) //busque que esté facturado y también coincida el valor de flete a manifestar con la suma de rubros en dólares int a cliente
                                {
                                    facturados = contabilizacion_sca(bl.bl_id, 2, 1);
                                }
                                else if (bl.id_routing == 0 || bl.id_routing == null) //solo busque que esté facturado
                                {
                                    facturados = contabilizacion_sca(bl.bl_id, 3, 1);
                                }

                                decimal? suma_montos = null;

                                if (facturados.Count == 0)
                                {
                                    suma_montos = null;
                                }
                                else
                                {
                                    suma_montos = new decimal();
                                    suma_montos = facturados.Sum();
                                    if (bl.id_routing > 0 && bl.valor_flete_manual == true) //solo busque que esté facturado
                                    {
                                        manifestar = 0;
                                    }
                                    else if (bl.id_routing > 0 && bl.valor_flete_manual == false) //busque que esté facturado y también coincida el valor de flete a manifestar con la suma de rubros en dólares int a cliente
                                    {
                                        if (suma_montos == bl.valor_flete_manifestar)
                                        {
                                            manifestar = 0;
                                        }
                                    }
                                    else if (bl.id_routing == 0 || bl.id_routing == null) //solo busque que esté facturado
                                    {
                                        manifestar = 0;
                                    }
                                }

                                if (pais != 1 || viaje_info.import_export != true || bl.fecha_ingreso_sistema <= new DateTime(2018, 7, 2))
                                {
                                    manifestar = 0;
                                }

                                var insert_bl = (from a in bl_completo_det
                                                 where a.bl_id == bl.bl_id
                                                 select new cuscar_bl_info
                                                 {
                                                     cuscar_viaje_id = newIdentityValue,
                                                     cuscar_container_id = 0,
                                                     no_bl = a.no_bl,
                                                     mbl = a.mbl,
                                                     id_shipper = Convert.ToInt32(a.id_shipper),
                                                     id_cliente = Convert.ToInt32(a.id_cliente),
                                                     id_puerto_embarque = Convert.ToInt32(a.id_puerto_embarque),
                                                     id_destino_final = Convert.ToInt32(a.id_destino_final),
                                                     no_piezas = a.no_piezas,
                                                     id_tipo_paquete = 1,
                                                     comodity_id = a.comodity_id,
                                                     peso = Convert.ToSingle(a.peso),
                                                     volumen = Convert.ToSingle(a.volumen),
                                                     ono_bl = a.no_bl,
                                                     tipo_conocimiento_id = a.tipo_conocimiento_id,
                                                     tipo_identificacion_id = a.tipo_identificacion_id,
                                                     observaciones = a.observaciones,
                                                     marks_and_numbers = "no marks",
                                                     id_aduana = Convert.ToInt32(a.id_aduana),
                                                     puerto_embarque = Admin.AdminService.getPais(a.id_puerto_embarque) + "" + Admin.AdminService.getlocode(a.id_puerto_embarque),
                                                     destino_final = Admin.AdminService.getPais(a.id_destino_final),
                                                     shipper = Admin.AdminService.getNombreCliente(a.id_shipper) + " " + Admin.AdminService.getDireccionCliente(a.id_shipper),
                                                     cliente = Admin.AdminService.getNombreCliente(a.id_cliente),
                                                     tipo_paquete = "CAJAS",
                                                     bl_id = Convert.ToInt32(bl.bl_id),
                                                     codigo_tributario = Admin.AdminService.getCodigoTributario(a.id_cliente).Replace("-", ""),
                                                     flete = a.valor_flete_manifestar,
                                                     tipo_docto = 1,
                                                     ttm_id = 1,
                                                     direccion_cliente = Admin.AdminService.getDireccionCliente(a.id_cliente),
                                                     selected2 = false,
                                                     id_routing = Convert.ToInt32(a.id_routing ?? 0),
                                                     valor_flete_manual = a.valor_flete_manual,
                                                     valor_cont_sca = suma_montos,
                                                     flag_manifestar = manifestar
                                                 }).FirstOrDefault();


                                context_ventas.cuscar_bl_info.Add(insert_bl);
                            }
                            context_ventas.SaveChanges();
                        }
                    }
                    dbContextTransaction.Commit();                  
                }
                catch (Exception e)
                {
                    var a = e.Message;
                    newIdentityValue = 0;
                    dbContextTransaction.Rollback();
                }
                return newIdentityValue;
            }          
        }

        private dynamic contabilizacion_sca(long bl_id, int tipo, int carga)
        {
            using (aimar_bawEntities aimar_baw = new aimar_bawEntities())
            {
                var query = (dynamic)null;
                List<decimal?> facturados = new List<decimal?>();
                switch (tipo)
                {
                    case 1:
                    case 3:
                        query = (from a in aimar_baw.tbl_reconciliacion_carga_sesiones
                                      join b in aimar_baw.tbl_reconciliacion_carga_bls
                                      on a.trs_id equals b.trb_trs_id
                                      join c in aimar_baw.tbl_reconciliacion_carga_cuestionario
                                      on b.trb_id equals c.trc_trb_id
                                      join d in aimar_baw.tbl_reconciliacion_carga_transacciones
                                      on c.trc_id equals d.trt_trc_id
                                      where a.trs_estado == 1 &&
                                      b.trb_estado == 1 &&
                                      c.trc_estado == 1 &&
                                      d.trt_estado == 1 &&
                                      //d.trt_ttm_id == 8 &&
                                      a.trs_estado_sesion == 4 &&
                                      a.trs_sis_id == 1 &&
                                      a.trs_tto_id == carga &&
                                      d.trt_ttr_id == 1 &&
                                      b.trb_bl_id == bl_id
                                      select new
                                      {
                                          d.trt_monto
                                      }).ToList();

                        if (query.Count > 0)
                        {
                            foreach (var a in query)
                            {
                                facturados.Add(a.trt_monto);
                            }
                        }
                        break;
                    case 2:
                        query = (from a in aimar_baw.tbl_reconciliacion_carga_sesiones
                                      join b in aimar_baw.tbl_reconciliacion_carga_bls
                                      on a.trs_id equals b.trb_trs_id
                                      join c in aimar_baw.tbl_reconciliacion_carga_cuestionario
                                      on b.trb_id equals c.trc_trb_id
                                      join d in aimar_baw.tbl_reconciliacion_carga_transacciones
                                      on c.trc_id equals d.trt_trc_id
                                      where a.trs_estado == 1 &&
                                      b.trb_estado == 1 &&
                                      c.trc_estado == 1 &&
                                      d.trt_estado == 1 &&
                                      d.trt_local_internacional_id == 2 &&
                                      d.trt_ttm_id == 8 &&
                                      a.trs_estado_sesion == 4 &&
                                      a.trs_sis_id == 1 &&
                                      a.trs_tto_id == carga &&
                                      d.trt_ttr_id == 1 &&
                                      d.trt_tpi_id == 3 &&
                                      b.trb_bl_id == bl_id
                                      select new
                                      {
                                          d.trt_monto
                                      }).ToList();
                        if (query.Count > 0)
                        {
                            foreach (var a in query)
                            {
                                facturados.Add(a.trt_monto);
                            }
                        }
                        break;
                }
                return facturados;
            }
        }
    }

    public class Comodity
    {
        public Nullable<long> comidity_id { get; set; }
    }

    public class NamesComodity 
    {
        public int names { get; set; }
    }
}
