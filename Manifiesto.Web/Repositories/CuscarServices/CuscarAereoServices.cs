using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using db_aereo.Entities.Entities;
using db_aereo.Entities;
using Manifiesto.Data.Utils;

namespace Manifiesto.Web.Repositories.CuscarServices
{
    public class CuscarAereoServices : ICuscarAereoServices
    {

        public IEnumerable<cuscar_voyage_info> getCuscar()
        {
            using (var context = new db_aereoContext())
            {
                return context.cuscar_voyage_info.ToList();
            }
        }


        public cuscar_voyage_info add_voyage_cuscar_info(cuscar_voyage_info vm)
        {        
            if (vm == null)
            {
                throw new ArgumentNullException("vm");
            }            
            using (var context = new db_aereoContext())
            {
                context.cuscar_voyage_info.Add(vm);
                context.SaveChanges();
                return vm;
            }            
        }

        public void update_cuscar_bl_info(int cuscar_voyage_id, int op, string noViaje)
        {
            try
            {
                using (var context = new db_aereoContext())
                {
                    if (op == 23)
                    {
                        var bLen = 0;
                        var cbLen = context.guia_import.Where(p => p.AwbNumber == noViaje).Count();
                        if (cbLen != 0)
                        {
                            bLen = cbLen;
                        }
                        string toInsertAabl = "insert into cuscar_bl_info (cuscar_voyage_id,no_bl,no_piezas,peso,volumen,bl_id1,shipper,cliente,direccion,comodity,id_cliente,id_shipper,tipo_docto,flete) select '" + cuscar_voyage_id + "',replace(hawbnumber,'-','') as hawbnumber,totnoofpieces,totweight,chargeableweights,awbid, mid(shipperdata,1,175), SUBSTRING_INDEX(ConsignerData,'\n',1), SUBSTRING_INDEX(SUBSTRING_INDEX(ConsignerData,'\n',2),'\n',-1), mid(natureqtygoods,1,70),consignerID,shipperID,'1','0'  from Awbi where awbnumber = '" + noViaje + "' and hawbnumber <> 'Master' " + Utils.iif(bLen > 1, " and hawbnumber <> ''", "");
                        context.Database.ExecuteSqlCommand(toInsertAabl);
                    }
                    else
                    {
                        var bLen = 0;
                        var cbLen = context.guia_export.Where(p => p.AwbNumber == noViaje).Count();
                        if (cbLen != 0)
                        {
                            bLen = cbLen;
                        }
                        string toInsertAabl = "insert into cuscar_bl_info (cuscar_voyage_id,no_bl,no_piezas,peso,volumen,bl_id1,shipper,cliente,direccion,comodity,id_cliente,id_shipper,tipo_docto,flete) select '" + cuscar_voyage_id + "',replace(hawbnumber,'-','') as hawbnumber,totnoofpieces,totweight,chargeableweights,awbid, mid(shipperdata,1,175), SUBSTRING_INDEX(ConsignerData,'\n',1), SUBSTRING_INDEX(SUBSTRING_INDEX(ConsignerData,'\n',2),'\n',-1), mid(natureqtygoods,1,70),consignerID,shipperID,'1','0'  from Awb where awbnumber = '" + noViaje + "' and hawbnumber <> 'Master' " + Utils.iif(bLen > 1, " and hawbnumber <> ''", "");
                        context.Database.ExecuteSqlCommand(toInsertAabl);
                    }

                    var bl_voyage = context.cuscar_bl_info.Where(p => p.cuscar_voyage_id == cuscar_voyage_id).ToList();

                    if (bl_voyage != null)
                    {
                        for (var i = 0; i < bl_voyage.Count(); i++)
                        {
                            var idCliente = bl_voyage[i];
                            string queryUpdateAabl = "Update cuscar_bl_info set " + 
                                                      "shipper = '" + Manifiesto.Data.Admin.AdminService.getNombreCliente(idCliente.id_shipper) + " " + Manifiesto.Data.Admin.AdminService.getDireccionCliente(idCliente.id_shipper) + "'," + 
                                                      "cliente = '" + Manifiesto.Data.Admin.AdminService.getNombreCliente(idCliente.id_cliente) + "', " +
                                                      "direccion = '" + Manifiesto.Data.Admin.AdminService.getDireccionCliente(idCliente.id_cliente) + "', " + 
                                                      "codigo_tributario = '" + Manifiesto.Data.Admin.AdminService.getCodigoTributario(idCliente.id_cliente).Replace("-", "") + "' " +
                                                      "where cuscar_bl_id = " + idCliente.cuscar_bl_id;
                            context.Database.ExecuteSqlCommand(queryUpdateAabl);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error en la transaccion.", e);
            }
        }


        // pendiente de refactorizar
        public int update_cuscar_voyage_info(int op)
        {
            try
            {
                using (var context = new db_aereoContext())
                {
                    var cuscar_voyage_id = context.cuscar_voyage_info.Where(p => p.operation == op).OrderByDescending(p => p.cuscar_voyage_id).First();
                    //var cuscar_voyage_id = context.cuscar_voyage_info.Where(p => p.operation == op).Max();
                    var noViaje = context.cuscar_voyage_info.Where(p => p.cuscar_voyage_id == cuscar_voyage_id.cuscar_voyage_id).Select(p => p.no_viaje).First();

                    context.Database.ExecuteSqlCommand("Update cuscar_voyage_info set no_viaje = replace(no_viaje,'-','') where cuscar_voyage_id = " + cuscar_voyage_id.cuscar_voyage_id);

                    string sequenceMaxQuery = "select manifest from cuscar_voyage_info where no_viaje ='" + noViaje.Replace("-", "") + "' and cuscar_voyage_id > 0 limit 1 offset 0";

                    var sequenceQueryResult = context.Database.SqlQuery<string>(sequenceMaxQuery).FirstOrDefault();

                    if (sequenceQueryResult.Count() > 0)
                    {
                        context.Database.ExecuteSqlCommand("Update cuscar_voyage_info set original = '" + sequenceQueryResult + "' where cuscar_voyage_id = '" + cuscar_voyage_id.cuscar_voyage_id + "'");
                    }

                    // Inserta guias
                    update_cuscar_bl_info(cuscar_voyage_id.cuscar_voyage_id, op, noViaje);

                    string country1 = null;
                    string airport1 = null;
                    string country2 = null;
                    string airport2 = null;

                    var puertoOrigen = context.Airports.Where(p => p.AirportID == cuscar_voyage_id.id_puerto_origen);
                    if (puertoOrigen.Count() > 0 || puertoOrigen != null)
                    {
                        country1 = puertoOrigen.Select(p => p.Country).Single();
                        airport1 = puertoOrigen.Select(p => p.AirportCode).Single();
                    }

                    var puertoDestino = context.Airports.Where(p => p.AirportID == cuscar_voyage_id.id_puerto_desembarque);
                    if (puertoDestino.Count() > 0 || puertoDestino != null)
                    {
                        country2 = puertoDestino.Select(p => p.Country).Single();
                        airport2 = puertoDestino.Select(p => p.AirportCode).Single();
                    }

                    //Actualiza informacion de puertos
                    context.Database.ExecuteSqlCommand("Update cuscar_voyage_info set puerto_origen = '" + country1 + airport1 + "', puerto_desembarque = '" + country2 + airport2 + "' where cuscar_voyage_id = " + cuscar_voyage_id.cuscar_voyage_id);

                    return cuscar_voyage_id.cuscar_voyage_id;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error en la transaccion.", e);
            }
        }
    }
}