using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Web.Areas.Maritimo.Models;
using Manifiesto.Data.Maritimo;
using ventas_gt.Entities.Entities;
using Manifiesto.Data.Admin;
using AutoMapper;
using master_aimar.Entities.Entities;
using Manifiesto.Data.Models;
using master_aimar.Entities;
using ventas_gt.Entities;
using System.Text;
using Manifiesto.Web.Repositories.Models;
using System.Text.RegularExpressions;
using Manifiesto.Web.Filters;

namespace Manifiesto.Web.Areas.Maritimo.Controllers
{
    [Authorize]
    public class txtGenerationController : Controller
    {
        readonly IMaritimoServices _manifestServices;
        readonly IAdminService _adminServices;

        public txtGenerationController(IMaritimoServices manifestServices, IAdminService adminServices)
        {
            this._manifestServices = manifestServices;
            this._adminServices = adminServices;
        }


        public ActionResult ErrorView(ErrorViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTxtFile(FormCollection collection)
        {
            int pais = Convert.ToInt32(Session["id_empresa"]);

            ErrorViewModel error_model = new ErrorViewModel();

            string mensaje;
            int id = 1;
            try
            {
                List<string> myobj = new List<string>();

                int mno = Convert.ToInt32(collection[0]);
                int cno = Convert.ToInt32(collection[1]);
                int type = Convert.ToInt32(collection[2]);
                int function = Convert.ToInt32(collection[3]);
                int operation = Convert.ToInt32(collection[4]);
                string funcsend = collection[5];
                int fix = Convert.ToInt32(collection[6]);

                IEnumerable<cuscar_container_info> container_info = new List<cuscar_container_info>();
                IEnumerable<cuscar_bl_info> bl_info = new List<cuscar_bl_info>();
                cuscar_container_info_update cuscar_update_info = new cuscar_container_info_update();
                cuscar_container_info_vm cuscar_container_update = new cuscar_container_info_vm();

                var puertos = _manifestServices.get_viaje_info().Where(a => a.cuscar_viaje_id == mno).FirstOrDefault();
                int id_deposito = Convert.ToInt32(puertos.id_deposito);
                string tipo = puertos.tipo;
                var viaje = puertos.no_viaje; 
                
                long? mn_id = 0;
                long vc_id = 0;
                string no_contenedor = "";
                string mbl = null;
                long? id_container_type = 0;
                string seal = "";
                long? no_piezas = 0;
                long? id_tipo_paquete = 0;
                string peso = "0.00";
                string volumen = "0.00";
                string comodity = "";
                int? no_bls = 0;
                var test = 6;
                long cuscar_container_id = 0;

                long? noBls = 0;

                var puerto_origen = puertos.puerto_origen;
                var puerto_desembarque = puertos.puerto_desembarque;

                // Container Info
                if (tipo == "LCL")
                    container_info = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == cno).ToList();
                else
                {
                    // si es adicion, eliminacion o reemplazo
                    if (function == 2 || function == 3 || function == 13 || function == 40)
                    {
                        container_info = _manifestServices.get_container_info_id(mno).ToList();
                        noBls = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.selected2 == true).Count();
                    }
                    else
                    {
                        container_info = _manifestServices.get_container_info().Where(a => a.cuscar_viaje_id == mno).ToList();
                        noBls = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Count();
                    }
                }

                cuscar_container_id = container_info.Select(a => a.cuscar_container_id).FirstOrDefault();

                var no_containers = this._manifestServices.get_container_info().Where(a => a.cuscar_viaje_id == mno).Count();

                long? viaje_id = 0;

                if (tipo == "LCL")
                     viaje_id = container_info.Select(a => a.viaje_id).FirstOrDefault();
                else
                    viaje_id = container_info.Select(a => a.bl_id).FirstOrDefault();

                var no_bl = bl_info.Select(a => a.no_bl).FirstOrDefault();

                string manifestMaster = _adminServices.getManifiestoMaster(tipo, viaje_id);

                var sequence = this._adminServices.getAaseq();
                int? mtrans = (sequence.nexttransmission + 1);
                int? mmsg = (sequence.nextmessage + 1);
                int seqmanifest = Convert.ToInt32(this._adminServices.iif(fix == 0, "1", "0"));
                int? seqm = (sequence.nextmanifest + seqmanifest);
                string mmnfst = seqm.ToString();

                string seq_next = String.Format("{0:0000}",(sequence.nexttransmission + 1));

                string mmnarc = "";

                if (pais == 1)
                    mmnarc = "P49E" + seq_next;
                else
                    mmnarc = "P18F" + seq_next;

                var manifest = container_info.Select(a => a.manifest).FirstOrDefault();

                if (pais == 1)
                    manifest = "49E" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 6);
                else
                    manifest = "18F" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 6);

                if (pais == 1)
                {
                    if (operation == 22)
                        myobj.Add("UNB+UNOA:2+7400000000421+" + dep(puerto_origen) + "+" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ":" + DateTime.Now.Hour + DateTime.Now.Minute + "+" + mtrans + "+" + test);
                    else
                        myobj.Add("UNB+UNOA:2+7400000000421+" + dep(puerto_desembarque) + "+" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ":" + DateTime.Now.Hour + DateTime.Now.Minute + "+" + mtrans + "+" + test);
                }
                else 
                { 
                    if (operation == 22)
                        myobj.Add("UNB+UNOA:2+7400000001299+" + dep(puerto_origen) + "+" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ":" + DateTime.Now.Hour + DateTime.Now.Minute + "+" + mtrans + "+" + test);
                    else
                        myobj.Add("UNB+UNOA:2+7400000001299+" + dep(puerto_desembarque) + "+" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ":" + DateTime.Now.Hour + DateTime.Now.Minute + "+" + mtrans + "+" + test);
                }

                var moriginal = container_info.Select(a => a.original).FirstOrDefault();

                myobj.Add("UNH+" + mmsg + "+CUSCAR:D:01A:UN");
                myobj.Add("BGM+" + type + "+" + manifest + "+" + function);
                myobj.Add("RFF+" + this._adminServices.iif(type == 785, "AFB", "API") + this._adminServices.iif(moriginal != manifest && moriginal != "NA", ":" + moriginal, "" ));
                
                if (pais == 1)
                    myobj.Add("NAD+" + funcsend + "+" + "7400000000421" + "::9");
                else
                    myobj.Add("NAD+" + funcsend + "+" + "7400000001299" + "::9");

                if (function != 1)
                {
                    if (viaje != "")
                        myobj.Add("FTX+ZZZ+++Viaje " + viaje);
                    else
                        myobj.Add("FTX+ZZZ+++BL " + no_bl);

                    myobj.Add("LOC+" + this._adminServices.iif(operation == 22, "8+", "125+") + this._adminServices.iif(operation == 22, puerto_desembarque, puerto_origen) + "::6");
                    myobj.Add("LOC+" + this._adminServices.iif(operation == 22, "178+", "179+") + this._adminServices.iif(operation == 22, puerto_origen, puerto_desembarque) + "::6");
                    myobj.Add("GIS+" + this._adminServices.iif(operation == 22, operation.ToString(), "23"));

                    foreach (var vj in container_info)
                    {
                        cuscar_container_update.cuscar_container_id = vj.cuscar_container_id;
                        cuscar_container_update.mfunction = function;
                        cuscar_container_update.operation = operation;
                        cuscar_container_update.funcsend = funcsend;
                        cuscar_container_update.mtype = Convert.ToInt32(type);

                        this._manifestServices.update_cuscar(cuscar_container_update);  

                        if (vj.mfunction != 1)
                        {                        
                            var mtype = vj.mtype;
                            var mfunction = vj.mfunction;
                            var mfuncsend = vj.funcsend;
                            var moperation = vj.operation;
                            //comodity = vj.comodity;

                            mn_id = vj.cuscar_viaje_id;
                            vc_id = vj.cuscar_container_id;
                            no_contenedor = vj.no_contenedor;
                            mbl = null;

                            id_container_type = vj.id_container_type;
                            seal = vj.seal;
                            no_piezas = vj.no_piezas;
                            //id_tipo_paquete = vj.id_tipo_paquete;

                            if (tipo == "LCL")
                                peso = string.Format("{0:0.00}", _manifestServices.getSumPeso(container_info.Select(a => a.cuscar_viaje_id).FirstOrDefault(), function, vc_id));
                            else
                                peso = string.Format("{0:0.00}", vj.peso);


                            volumen = string.Format("{0:0.00}", vj.volumen);
                            //comodity = vj.comodity;
                            no_bls = vj.no_bls;

                            string iso = "";
                            int mt = 0;
                            var containerTypes = this._adminServices.getContainerType();

                            IEnumerable<container_typeViewModel> containerTypesVM = Mapper.Map<IEnumerable<container_type>, IEnumerable<container_typeViewModel>>(containerTypes);

                            iso = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.iso).FirstOrDefault();
                            mt = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.mt).FirstOrDefault();

                            if (no_contenedor != "")
                            {
                                myobj.Add("EQD+CN+" + no_contenedor + "+" + iso + "::5++11+5");
                                myobj.Add("TSR++1");
                                myobj.Add("MEA+WT+AAB+KGM:" + peso);
                                myobj.Add("MEA+WT+T+KGM:" + mt);

                                if (seal != "")
                                    myobj.Add("SEL+" + seal + "+CA");
                            }
                        }
                    }

                    totalesDet2 totales = new totalesDet2();
                    int cond;
                    if (tipo == "LCL")
                    {
                        if (function == 2 || function == 3 || function == 13 || function == 40)
                        {
                            cond = 1;
                            totales = this._manifestServices.getTotales(tipo, cno, cond, 0).FirstOrDefault();
                        }
                        else
                        {
                            cond = 2;
                            totales = this._manifestServices.getTotales(tipo, cno, cond, 0).FirstOrDefault();
                        }
                    }
                    else
                    {
                        if (function == 2 || function == 3 || function == 13 || function == 40)
                        {
                            cond = 6;
                            totales = this._manifestServices.getTotales(tipo, mno, cond, 0).FirstOrDefault();
                        }
                        else
                        {
                            cond = 3;
                            totales = this._manifestServices.getTotales(tipo, mno, cond, 0).FirstOrDefault();
                        }
                    }

                    long? totbls = totales.count; //totales.count;
                    //no bls FCL
                    
                    if (tipo == "FCL")
                        if (noBls > 1)
                            totbls = noBls;
                   
                    decimal? totpeso = totales.peso;
                    decimal? totvol = totales.volumen;
                    decimal? totpcs = totales.no_piezas;

                    myobj.Add("CNT+10:" + totbls);
                    myobj.Add("CNT+32:" + string.Format("{0:0.00}", totpeso));
                    
                    if (no_contenedor != "" && totvol != 0)
                        myobj.Add("CNT+26:" + totvol);

                    myobj.Add("CNT+11:" + totpcs);
          
                    if (no_contenedor != "")
                        myobj.Add("CNT+16:" + container_info.Count());

                    // Total Valor de Flete 
                    if (tipo == "FCL")
                    {
                        if (operation == 23)
                        {
                            if (function == 2 || function == 3 || function == 13 || function == 40)
                            {
                                decimal? flete = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.selected2 == true).Sum(a => a.flete);
                                string suma_flete = string.Format("{0:0.00}", flete);

                                myobj.Add("CNT+21:" + suma_flete);
                            }
                            else
                            {
                                decimal? flete = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Sum(a => a.flete);
                                string suma_flete = string.Format("{0:0.00}", flete);

                                myobj.Add("CNT+21:" + suma_flete);
                            }
                        }
                        else
                            myobj.Add("CNT+21:0");
                    }
                    else 
                    {

                        if (operation == 23)
                        {
                            if (function == 2 || function == 3 || function == 13 || function == 40)
                            {
                                decimal? flete = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.cuscar_container_id == cno && a.selected2 == true).Sum(a => a.flete);
                                string suma_flete = string.Format("{0:0.00}", flete);

                                myobj.Add("CNT+21:" + suma_flete);
                            }
                            else
                            {
                                decimal? flete = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.cuscar_container_id == cno).Sum(a => a.flete);
                                string suma_flete = string.Format("{0:0.00}", flete);

                                myobj.Add("CNT+21:" + suma_flete);
                            }                          
                        }
                        else
                        {
                            myobj.Add("CNT+21:0");
                        }
                    }

                   
                    long? bl_vc_id = 0;
                    string no_bl_det = "";
                    long? id_cliente_det = 0;
                    string puerto_embarque_det = "";
                    string destino_final_det = "";
                    string shipper_det = "";
                    string cliente_det = "";
                    long? id_aduana_det = 0;
                    string aduanagln = "";
                    string xnit = "";

                    // BL Info
                    if (tipo == "LCL")
                    {
                        if (function == 2 || function == 3 || function == 13 || function == 40)
                            bl_info = _manifestServices.get_bl_info().Where(a => a.selected2 == true && a.cuscar_container_id == cno).OrderBy(a => a.cuscar_bl_id).ToList();
                        else
                            bl_info = _manifestServices.get_bl_info().Where(a => a.cuscar_container_id == cno).OrderBy(a => a.cuscar_bl_id).ToList();
                    }
                    else
                    {
                        if (function == 2 || function == 3 || function == 13 || function == 40)
                            bl_info = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.selected2 == true).OrderBy(a => a.cuscar_bl_id).ToList();
                        else
                            bl_info = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).OrderBy(a => a.cuscar_bl_id).ToList();
                    }
                    int b = -1;
                    foreach (var bl_det in bl_info)
                    {

                        if (tipo == "LCL")
                            mbl = container_info.Where(a => a.cuscar_container_id == bl_det.cuscar_container_id).Select(a => a.mbl).FirstOrDefault();

                        b++;
                        bl_vc_id = bl_det.cuscar_container_id;
                        no_bl_det = bl_det.no_bl;
                        id_cliente_det = bl_det.id_cliente;
                        puerto_embarque_det = bl_det.puerto_embarque;
                        destino_final_det = bl_det.destino_final;
                        shipper_det = bl_det.shipper;
                        cliente_det = bl_det.cliente;
                        id_aduana_det = bl_det.id_aduana;                      

                        if (id_aduana_det != 0)
                            aduanagln = _adminServices.getAduanas().Where(a => a.id_aduana == id_aduana_det).Select(a => a.gln).FirstOrDefault();

                        if (tipo == "LCL")
                        {
                            no_piezas = bl_det.no_piezas;
                            id_tipo_paquete = bl_det.id_tipo_paquete;
                            peso = string.Format("{0:0.00}", bl_det.peso);
                            volumen = string.Format("{0:0.00}", bl_det.volumen);
                            comodity = bl_det.comodity;
                        }
                        else
                        {
                            id_tipo_paquete = container_info.Select(a => a.id_tipo_paquete).FirstOrDefault();

                            if (container_info.Count() == 1)
                            {
                                if (noBls > 1)
                                    totales = this._manifestServices.getTotales("FCL", mno, 5, bl_info.Select(a => a.bl_id).FirstOrDefault()).FirstOrDefault();
                                else
                                    totales = this._manifestServices.getTotales("FCL", mno, 7, bl_info.Select(a => a.bl_id).FirstOrDefault()).FirstOrDefault();
                            }
                            else
                            {
                                totales = this._manifestServices.getTotales("FCL", mno, 4, bl_det.bl_id).FirstOrDefault();
                            }
                            
                            peso = string.Format("{0:0.00}", totales.peso);
                            volumen = string.Format("{0:0.00}", totales.volumen);
                            no_piezas = Convert.ToInt32(totales.no_piezas);
                        }

                        if (operation == 23)
                        {
                            if (bl_det.tipo_docto == 1)
                            {
                                if (bl_det.codigo_tributario != "" || bl_det.codigo_tributario != "C/F")
                                    xnit = "NIT" + bl_det.codigo_tributario + "+";
                                else
                                {
                                    return Json(new { ID = 2, status = "Error", title = "Sin Nit para bl" }, JsonRequestBehavior.AllowGet);
                                }
                                //return Json(new { ID = 2, status = "Error", title = "Sin Nit para bl" + bl_det.no_bl }, JsonRequestBehavior.AllowGet);
                            }
                            else if (bl_det.tipo_docto == 2)
                                xnit = "AAO+";
                            else if (bl_det.tipo_docto == 3)
                                xnit = "EEE+";
                            else if (bl_det.tipo_docto == 4)
                                xnit = "NTG+";
                            else
                            {
                                //error_model.error = "Falta algun tipo de documento ingresado para el Nit en el BL.";
                                //return RedirectToAction("ErrorView", error_model);
                                return Json(new { ID = 2, status = "Error", title = "Falta algun tipo de documento ingresado para el Nit en el BL." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var cliente = _adminServices.getClientes().Where(a => a.id_cliente == bl_det.id_cliente);
                            if (cliente.Any(a => a.id_cliente == bl_det.id_cliente))
                                xnit = "NIT+";
                            else
                                xnit = "+";
                        }


                        if (mbl == null)
                            mbl = bl_det.mbl;

                        if (tipo == "FCL")
                        {
                            if (noBls > 1)
                            {
                                peso = bl_det.peso.ToString();
                                volumen = string.Format("{0:0.00}", bl_det.volumen);
                                no_piezas = bl_det.no_piezas;
                            }
                        }

                        int blcni = b + 1;
                        myobj.Add("CNI+" + blcni + "+::" + mbl);
                        myobj.Add("RFF+BH:" + bl_det.no_bl);
                        myobj.Add("CNT+7:" + peso);
                        myobj.Add("CNT+11:" + no_piezas);

                        if (no_contenedor != "")
                        {
                            if (volumen != "0.00")
                                myobj.Add("CNT+15:" + volumen);

                            int no_conte = container_info.Count();

                            if (tipo == "FCL")
                                if (noBls > 1 && no_containers == 1)
                                    no_conte = 1;
                                else
                                    no_conte = container_info.Where(a => a.bl_id_ref == bl_det.bl_id).Count();

                            myobj.Add("CNT+16:" + no_conte);
                        }

                        decimal? flete = 0;
                        var tipo_moneda = "USD";
                        if (operation == 23)
                        {
                            flete = bl_det.flete;
                            /*
                            if (flete != 0)
                            {
                                if (bl_det.ttm_id == 1)
                                    tipo_moneda = "GTQ";
                                else if (bl_det.ttm_id == 2)
                                    tipo_moneda = "USD";
                                else
                                    tipo_moneda = "EUR";
                            }
                             * */
                            /*
                            if (flete == null || flete == 0)
                            {
                                error_model.error = "Flete sin valor o con valor 0";
                                return RedirectToAction("ErrorView", error_model);
                            } //return Json(new { ID = 2, status = "Error", title = "Flete sin valor para bl " + bl_det.no_bl }, JsonRequestBehavior.AllowGet);
                            */
                        }

                        myobj.Add("MOA+144:" + flete + ":"+ tipo_moneda +"");
                        myobj.Add("LOC+35+" + puerto_origen.Substring(0, 2) + "::5" + "+" + puerto_origen + "::6");

                        if (id_aduana_det != 0)
                            myobj.Add("LOC+17+" + aduanagln + "::9");

                        if (operation == 22)
                            myobj.Add("LOC+36+" + puerto_desembarque.Substring(0, 2) + "::5");
                        if (operation == 23)
                            myobj.Add("LOC+36+" + destino_final_det + "::5");
                        if (operation == 24)
                            myobj.Add("LOC+36+" + destino_final_det + "::5");

                        if (operation == 22)
                            myobj.Add("LOC+88+" + whn(puerto_embarque_det) + "::9");
                        if (puerto_desembarque == "GTPRQ" && id_deposito == 34029)
                        {
                            puerto_desembarque = "GTTCQ";
                            if (operation == 23)
                                myobj.Add("LOC+88+" + whn(puerto_desembarque) + "::9");
                            if (operation == 24)
                                myobj.Add("LOC+88+" + whn(puerto_desembarque) + "::9");
                        }
                        else
                        {
                            if (operation == 23)
                                myobj.Add("LOC+88+" + whn(puerto_desembarque) + "::9");
                            if (operation == 24)
                                myobj.Add("LOC+88+" + whn(puerto_desembarque) + "::9");
                        }
                        

                        myobj.Add("GIS+" + this._adminServices.iif(operation == 24 && id_aduana_det == 0, "23", operation.ToString()) + ":::8");

                        if (bl_det.flete == 0 || operation == 22 || operation == 24)
                            myobj.Add("CPI+14+NS");
                        else
                            myobj.Add("CPI+14");
                        
                        myobj.Add("CPI+2");
                        myobj.Add("TDT+30");
                        myobj.Add("RFF+ZZZ:1");

                        if (no_contenedor != "")
                            myobj.Add("TSR+10+++9");
                        else
                            myobj.Add("TSR+10+++10");

                        string tests = bl_det.shipper;
                        myobj.Add("NAD+CZ++" + this._adminServices.Break35(bl_det.shipper));
                        
                        //myobj.Add("NAD+CN+" + xnit + this._adminServices.Break100_cliente(bl_det.cliente) + this._adminServices.Break100_direccion(bl_det.direccion_cliente));
                        //string cliente_direccion = bl_det.cliente + " " + bl_det.direccion_cliente;
                        //myobj.Add("NAD+CN+" + xnit + this._adminServices.Break35(cliente_direccion));

                        string cliente_break = bl_det.cliente.Truncate(70);
                        string direccion_cliente_break = bl_det.direccion_cliente.Truncate(105);

                        myobj.Add("NAD+CN+" + xnit + this._adminServices.Break35(cliente_break) + this._adminServices.Break35(direccion_cliente_break));


                        if (tipo == "FCL")
                        {
                            if (noBls > 1)
                            {
                                no_piezas = bl_det.no_piezas;
                            }
                        }

                        if (no_contenedor == "")
                            myobj.Add("GID+1+" + no_piezas + ":NE:ZZZ::10");
                        else
                            myobj.Add("GID+1+" + no_piezas + ":" + _adminServices.getTipoPaquete().Where(a => a.tipo_id == id_tipo_paquete).Select(a => a.iso).FirstOrDefault() + ":ZZZ::9");

                        if (tipo == "FCL")
                        {
                            // obtiene agrupados los comodities de los contenedores
                            string comod = "";
                                var list_c = from p in container_info
                                    group p by new { p.comodity_id }
                                        into mygroup
                                        select mygroup.FirstOrDefault();

                            // recorres los comodities para asginar a variable y concatenarlos
                            foreach (var c in list_c)
                            {
                                comod = comod + " " + c.comodity;
                            }

                            comodity = comod.Substring(1);
                        }


                        if (comodity.Length <= 70)
                            myobj.Add("FTX+AAA+++" + comodity);
                        else
                        {
                            double parts = 70;
                            int k = 0;
                            var output = comodity
                                .ToLookup(c => Math.Floor(k++ / parts))
                                .Select(e => new String(e.ToArray()));

                            foreach (var i in output)
                            {
                                myobj.Add("FTX+AAA+++" + i);
                            }

                        }
                   
                        myobj.Add("MEA+AAI++KGM:" + peso);

                        if (no_contenedor != "")
                        {
                            if (volumen != "0.00")
                                myobj.Add("MEA+VOL++MTQ:" + volumen);

                            if (tipo == "LCL")
                            {
                                var no_contenedor1 = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == bl_det.cuscar_container_id).Select(a => a.no_contenedor).FirstOrDefault();
                                myobj.Add("SGP+" + no_contenedor1);
                            }
                            else
                            {
                                if (bl_info.Count() > 1 && no_containers == 1)
                                {
                                    foreach (var vj1 in container_info.Where(a => a.cuscar_viaje_id == bl_det.cuscar_viaje_id))
                                    {
                                        myobj.Add("SGP+" + vj1.no_contenedor);
                                    }
                                }
                                else if (bl_info.Count() > 1 && no_containers > 1)
                                {
                                    foreach (var vj1 in container_info.Where(a => a.bl_id_ref == bl_det.bl_id))
                                    {
                                        myobj.Add("SGP+" + vj1.no_contenedor);
                                    }
                                }
                                else 
                                {
                                    foreach(var vj2 in container_info)
                                    {
                                        myobj.Add("SGP+" + vj2.no_contenedor);
                                    }
                                }
                            }

                        }
                        myobj.Add("TMD+4");
                    }
                
                }
                
                myobj.Add("AUT+Cuscar 2.2 Ag Carga+Sistemas Aster");
                myobj.Add("UNT+" + (myobj.Count() + 2) + "+" + mmsg);
                myobj.Add("UNZ+1+" + this._adminServices.pad0(mtrans.ToString(), 4));
                
                
                using (var context_admin = new master_aimarEntities())
                {
                    context_admin.Database.ExecuteSqlCommand("UPDATE aaseq SET nexttransmission = " + mtrans + ", nextmessage = " + mmsg + ", nextmanifest = " + mmnfst);
                }
                
                DateTime ndt = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime d2 = DateTime.Now;

                //DateTime d1 = DateTime.Now;
                //DateTime d2 = DateTime.Now.AddDays(-1);

                TimeSpan t = d2 - ndt;
                int nfile = t.Days + 1;
                string snfile = nfile.ToString();
                string nparttwo = this._adminServices.pad0(snfile, 3);
                string mfile = mmnarc + "." + nparttwo;
                
                //string file_name = "C:\\" + mfile;
                string path = HttpContext.Server.MapPath("~/Files/");
                string file_name = path + mfile;
                string tLine = "";

                System.IO.StreamWriter objWriter;
                objWriter = new System.IO.StreamWriter(file_name);

                for (int i = 0; i < myobj.Count(); i++)
                {
                    tLine = tLine + myobj[i] + "\r\n";
                    objWriter.WriteLine(myobj[i] + "'");
                }

                objWriter.Close();
                
                var cuscars_co = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == cuscar_container_id).ToList();

                DateTime dt = DateTime.Now; // Or whatever
                string s = dt.ToString("yyyy/MM/dd");
                
                if (tipo == "FCL")
                {
                    cuscar_update_info.mfile = mfile;
                    cuscar_update_info.manifest = manifest;
                    cuscar_update_info.s = s;
                    cuscar_update_info.cuscar_viaje_id = mno;

                    foreach (var h in cuscars_co)
                    {                        
                        if (h.original == "NA")
                            this._manifestServices.update_container_original_fcl(cuscar_update_info, 1);
                        else
                            this._manifestServices.update_container_original_fcl(cuscar_update_info, 2);
                    }
                }
                else
                {
                    cuscar_update_info.mfile = mfile;
                    cuscar_update_info.manifest = manifest;
                    cuscar_update_info.s = s;
                    cuscar_update_info.cuscar_container_id = cuscar_container_id;

                    foreach (var h in cuscars_co)
                    {
                        if (h.original == "NA")
                            this._manifestServices.update_container_original(cuscar_update_info, 1);
                        else
                            this._manifestServices.update_container_original(cuscar_update_info, 2);
                    }
                }
                
                mensaje = "File Generated";
                //return RedirectToAction("Edit", "Manifest", new { id = mno, area = "Maritimo" });
                //return RedirectToAction("Edit", "Manifest", new { area = "Maritimo" });
                //return RedirectToAction("Edit", "Manifest", new { id = mno });
                return Json(new { ID = id, file = "", manifiesto = manifest, status = mensaje, title = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { ID = id, status = "Exception caught." + e, title = "Please try again or Contact with the Administrator." }, JsonRequestBehavior.AllowGet);
                //error_model.error = e.ToString();
                //return RedirectToAction("ErrorView", error_model);
            }
        }

        public string dep(string mport)
        {
            string result;
            switch (mport)
            {
                case "GTSTC":
                    result = "7409000030162";
                    break;
                case "GTPRQ":
                    result = "7409000030070";
                    break;
                case "GTPBR":
                    result = "7409000030179";
                    break;
                default:
                    result = "NA";
                    break;
            }
            return result;
        }

        public string whn(string mport)
        {
            string result;
            switch (mport)
            {
                case "GTSTC":
                    result = "7400000000094";
                    break;
                case "GTPRQ":
                    result = "7400000000087";
                    break;
                case "GTPBR":
                    result = "7400000000100";
                    break;
                case "GTTCQ":
                    result = "7400000002203";
                    break;
                default:
                    result = "NA";
                    break;
            }
            return result;
        }
    
    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
