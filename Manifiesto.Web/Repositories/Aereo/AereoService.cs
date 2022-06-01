using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifiesto.Data.Models;
using System.Linq.Expressions;
using db_aereo.Entities.Entities;
using db_aereo.Entities;
using Manifiesto.Web.Areas.Aereo.Models;

namespace Manifiesto.Data.Aereo
{
    public class AereoService : IAereoService
    {

        public IEnumerable<awbi> getGuiasImport()
        {
            using (var context = new db_aereoContext())
            {
                return context.guia_import.ToList();
            }
        }

        public IEnumerable<guiasListViewModel> getGuiasImportList()
        {
            using (var context = new db_aereoContext())
            {
                DateTime date = DateTime.Today.AddMonths(-3).Date;
                string adate = date.ToString("yyyy-MM-dd");
                string query = "select replace(max(awbnumber),'-','') as awbnumber, replace(max(hawbnumber),'-','') as hawbnumber, max(createddate) as createddate, max(awbid) as id, sum(1) as bls from Awbi where createddate >= '" + adate + "' and hawbnumber <> 'Master' and hawbnumber<>'' group by awbnumber order by AwbID desc";
                IEnumerable<guiasListViewModel> list = context.Database.SqlQuery<guiasListViewModel>(query).ToList();
                return list;
            }
        }

        public IEnumerable<guiasListViewModel> getGuiasExportList()
        {
            using (var context = new db_aereoContext())
            {
                DateTime date = DateTime.Today.AddMonths(-3).Date;
                string adate = date.ToString("yyyy-MM-dd");
                string query = "select replace(max(awbnumber),'-','') as awbnumber, replace(max(hawbnumber),'-','') as hawbnumber, max(createddate) as createddate, max(awbid) as id, sum(1) as bls from Awb where createddate >= '" + adate + "' and hawbnumber <> 'Master' and hawbnumber<>'' group by awbnumber order by AwbID desc";
                IEnumerable<guiasListViewModel> list = context.Database.SqlQuery<guiasListViewModel>(query).ToList();
                return list;
            }
        }

        public IEnumerable<cuscar_voyage_info> get_cuscar_voyage_info()
        {
            using (var context = new db_aereoContext())
            {
                return context.cuscar_voyage_info.ToList();
            }
        }

        public IEnumerable<cuscar_bl_info> get_cuscar_bl_info()
        {
            using (var context = new db_aereoContext())
            {
                return context.cuscar_bl_info.ToList();
            }
        }

        public IEnumerable<totalesDet2> get_totales(int cuscar_voyage_id)
        {
            using (var context = new db_aereoContext())
            {
                string query = "select count(1) as count, sum(peso) as peso, sum(volumen) as volumen, sum(no_piezas) as no_piezas FROM cuscar_bl_info where cuscar_voyage_id = " + cuscar_voyage_id;
                IEnumerable<totalesDet2> list = context.Database.SqlQuery<totalesDet2>(query).ToList();
                return list;
            }
        }

        public manifiesto_encVM FindToAdd(int id, int optipo)
        {
            try
            {
                using (var context = new db_aereoContext())
                {
                    if (optipo == 23)
                    {
                        var toInsertaamn = (from a in context.guia_import
                                            where a.AwbID == id
                                            join b in context.Carriers on a.CarrierID equals b.CarrierID
                                            select new manifiesto_encVM
                                            {
                                                funcsend = "DC",
                                                no_viaje = a.AwbNumber,
                                                id_naviera = a.CarrierID,
                                                vapor = a.ArrivalFlight,
                                                id_puerto_origen = a.AirportDepID,
                                                id_puerto_desembarque = a.AirportDesID,
                                                fecha_arribo = a.CreatedDate,
                                                tipo = "AWB",
                                                viaje_id = a.AwbID,
                                                operation = optipo,
                                                test = 6,
                                                naviera = b.Name,
                                                manifest = "NA",
                                                mfunction = 9,
                                                mtype = 785,
                                                cuscar = "NA",
                                                cuscardt = null,
                                                countries = a.Countries,
                                                id_status = 1
                                            }).Single();

                        return toInsertaamn;
                    }
                    else
                    {
                        var toInsertaamn = (from a in context.guia_export
                                            where a.AwbID == id
                                            join b in context.Carriers on a.CarrierID equals b.CarrierID
                                            select new manifiesto_encVM
                                            {
                                                funcsend = "DC",
                                                no_viaje = a.AwbNumber,
                                                id_naviera = a.CarrierID,
                                                vapor = a.ArrivalFlight,
                                                id_puerto_origen = a.AirportDepID,
                                                id_puerto_desembarque = a.AirportDesID,
                                                fecha_arribo = a.CreatedDate,
                                                tipo = "AWB",
                                                viaje_id = a.AwbID,
                                                operation = optipo,
                                                test = 6,
                                                naviera = b.Name,
                                                manifest = "NA",
                                                mfunction = 9,
                                                mtype = 785,
                                                cuscar = "NA",
                                                cuscardt = null,
                                                countries = a.Countries,
                                                id_status = 1
                                            }).Single();

                        return toInsertaamn;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error-" + e);
            }
        }

        public int? update_bl_aereo(cuscar_bl_info cuscar_bl_info)
        {

            using (var context = new db_aereoContext())
            {
                string query = "update cuscar_bl_info set cliente = '" + cuscar_bl_info.cliente + "', direccion = '" + cuscar_bl_info.direccion + "', comodity = '" + cuscar_bl_info.comodity + "', shipper = '" + cuscar_bl_info.shipper + "', tipo_docto = " + cuscar_bl_info.tipo_docto + ", ttm_id = " + cuscar_bl_info.ttm_id + ", flete = " + cuscar_bl_info.flete + " where cuscar_bl_id = " + cuscar_bl_info.cuscar_bl_id;
                context.Database.ExecuteSqlCommand(query);

                return cuscar_bl_info.cuscar_voyage_id;
            }
        }

        public cuscar_voyage_info close_cuscar(long id, string firma)
        {
            using (var context = new db_aereoContext())
            {
                var currentContainer = context.cuscar_voyage_info.Where(a => a.cuscar_voyage_id == id);

                if (firma != "")
                {
                    string query_update = "update cuscar_voyage_info set " +
                            "id_status = 0 ," +
                            "respuesta_sat = '" + firma + "' " +
                            "where cuscar_voyage_id =" + id;

                    context.Database.ExecuteSqlCommand(query_update);
                    context.SaveChanges();
                }
                

                return currentContainer.FirstOrDefault();
            }
        }

       
        /*
        public IEnumerable<totalesDet> totalesDet()
        {
            using (var context = new db_aereoEntities())
            {
                string query = "select sum(no_piezas) as no_piezas, sum(peso) as peso, sum(volumen) as volumen, count(1) as count FROM manifiesto_det";
                var data = context.Database.SqlQuery<totalesDet>(query).ToList();

                return data;
            }
        }
        */
        // getNombreCliente y getDireccionCliente se moveran de acá
        //public string getNombreCliente(int? idCliente)
        //{
        //    using (var context = new db_AdminEntities())
        //    {
        //        var cliente = context.clientes.Where(p => p.id_cliente == idCliente).Select(p => p.nombre_cliente);
        //        if (cliente.Count() != 0)
        //            return cliente.Single();
        //        else
        //            return "NF";
        //    }
        //}

        //public string getDireccionCliente(int? idCliente)
        //{
        //    using (var context = new db_AdminEntities())
        //    {
        //        var direccion = context.direcciones.Where(p => p.id_cliente == idCliente).Select(p => p.direccion_completa);
        //        if (direccion.Count() != 0)
        //            return direccion.Single();
        //        else
        //            return "NF";
        //    }
        //}
    }
}
