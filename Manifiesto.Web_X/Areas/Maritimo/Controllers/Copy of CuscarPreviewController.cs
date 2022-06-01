using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Web.Areas.Maritimo.Models;
using Manifiesto.Data.Maritimo;
using Manifiesto.Data.Admin;
using ventas_gt.Entities.Entities;
using Manifiesto.Web.Repositories.Models;
using AutoMapper;
using master_aimar.Entities.Entities;
using Manifiesto.Data.Models;

namespace Manifiesto.Web.Areas.Maritimo.Controllers
{
    public class CuscarPreviewController2 : Controller
    {
        readonly IMaritimoServices _manifestServices;
        readonly IAdminService _adminServices;

        public CuscarPreviewController2(IMaritimoServices manifestServices, IAdminService adminServices)
        {
            this._manifestServices = manifestServices;
            this._adminServices = adminServices;
        }
        //
        // GET: /Maritimo/CuscarPreview/
        /*
        public ActionResult Index(FormCollection collection)
        {
           
            int pais = Convert.ToInt32(Session["id_empresa"]);

            int mno = Convert.ToInt32(collection[0]);
            int cno = Convert.ToInt32(collection[1]);
            int type = Convert.ToInt32(collection[2]);
            int function = Convert.ToInt32(collection[3]);
            int operation = Convert.ToInt32(collection[4]);
            string funcsend = collection[5];
            int fix = Convert.ToInt32(collection[6]);

            CuscarParametersVM parametros = new CuscarParametersVM() { no_viaje = mno, no_contenedor = cno, type = type, function = function, operation = operation, funcsend = funcsend, fix = fix };
            CuscarPreviewVM modelPreview = new CuscarPreviewVM();
            cuscar_viaje_infoVM cuscar_viaje_infoVM = new cuscar_viaje_infoVM();

            List<string> myobj = new List<string>();


            modelPreview.parameters = parametros;

            IEnumerable<cuscar_container_info> container_info = new List<cuscar_container_info>();
            IEnumerable<cuscar_bl_info> bl_info = new List<cuscar_bl_info>();
            cuscar_container_info_update cuscar_update_info = new cuscar_container_info_update();
            cuscar_container_info_vm cuscar_container_update = new cuscar_container_info_vm();

            var puertos = _manifestServices.get_viaje_info().Where(a => a.cuscar_viaje_id == mno).FirstOrDefault();

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

            long? noBls = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Count();

            var puerto_origen = puertos.puerto_origen;
            var puerto_desembarque = puertos.puerto_desembarque;

            // Container Info
            if (tipo == "LCL")
                container_info = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == cno).ToList();
            else
                container_info = _manifestServices.get_container_info().Where(a => a.cuscar_viaje_id == mno).ToList();

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

            int? seq_next = (sequence.nexttransmission + 1);

            string mmnarc = "";

            var manifest = container_info.Select(a => a.manifest).FirstOrDefault();

            if (pais == 1)
                manifest = "49E" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 5);
            else
                manifest = "18F" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 5);


            var moriginal = container_info.Select(a => a.original).FirstOrDefault();

            myobj.Add("Tipo: " + type + " No Manifiesto: " + manifest + "+" + function);
            myobj.Add("RFF+" + this._adminServices.iif(type == 785, "AFB", "API") + this._adminServices.iif(moriginal != manifest && moriginal != "NA", ":" + moriginal, ":" + manifestMaster));

            if (function != 1)
            {
                if (viaje != "")
                    myobj.Add("Viaje: " + viaje);
                else
                    myobj.Add("BL: " + no_bl);

                myobj.Add("Puerto Origen: " + this._adminServices.iif(operation == 22, "8+", "125+") + this._adminServices.iif(operation == 22, puerto_desembarque, puerto_origen) + "::6");
                myobj.Add("Puerto Destino: " + this._adminServices.iif(operation == 22, "178+", "179+") + this._adminServices.iif(operation == 22, puerto_origen, puerto_desembarque) + "::6");

                foreach (var vj in container_info)
                {
                    if (vj.mfunction != 1)
                    {
                        var mtype = vj.mtype;
                        var mfunction = vj.mfunction;
                        var mfuncsend = vj.funcsend;
                        var moperation = vj.operation;
                        comodity = vj.comodity;

                        mn_id = vj.cuscar_viaje_id;
                        vc_id = vj.cuscar_container_id;
                        no_contenedor = vj.no_contenedor;
                        mbl = null;

                        id_container_type = vj.id_container_type;
                        seal = vj.seal;
                        no_piezas = vj.no_piezas;
                        //id_tipo_paquete = vj.id_tipo_paquete;

                        if (tipo == "LCL")
                            peso = string.Format("{0:0.00}", vj.no_piezas);
                        else
                            peso = string.Format("{0:0.00}", vj.peso);

                        // Peso Bls
                        if (tipo == "LCL")
                            peso = _manifestServices.getSumPeso(container_info.Select(a => a.cuscar_viaje_id).FirstOrDefault(), function, vc_id);


                        volumen = string.Format("{0:0.00}", vj.volumen);
                        comodity = vj.comodity;
                        no_bls = vj.no_bls;

                        string iso = "";
                        int mt = 0;
                        var containerTypes = this._adminServices.getContainerType();

                        IEnumerable<container_typeViewModel> containerTypesVM = Mapper.Map<IEnumerable<container_type>, IEnumerable<container_typeViewModel>>(containerTypes);

                        iso = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.iso).FirstOrDefault();
                        mt = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.mt).FirstOrDefault();

                        if (no_contenedor != "")
                        {
                            myobj.Add("No Contenedor: " + no_contenedor + " ISO: " + iso);
                            myobj.Add("Peso: " + peso);
                            myobj.Add("Tipo : " + mt);

                            if (seal != "")
                                myobj.Add("Seal: " + seal);
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
                    cond = 3;
                    totales = this._manifestServices.getTotales(tipo, mno, cond, 0).FirstOrDefault();
                }

                long? totbls = totales.count; //totales.count;
                //no bls FCL

                if (tipo == "FCL")
                    if (noBls > 1)
                        totbls = noBls;

                decimal? totpeso = totales.peso;
                decimal? totvol = totales.volumen;
                decimal? totpcs = totales.no_piezas;

                myobj.Add("Total Bls: " + totbls);
                myobj.Add("Total peso: " + totpeso);

                if (no_contenedor != "" && totvol != 0)
                    myobj.Add("Total Volumen: " + totvol);

                myobj.Add("Total piezas: " + totpcs);

                if (no_contenedor != "")
                    myobj.Add("No Contenedores: " + container_info.Count());


                if (tipo == "FCL")
                {
                    string suma_flete = string.Format("{0:0.00}", _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Sum(a => a.flete));
                    if (operation == 23)
                        myobj.Add("Flete: " + suma_flete);
                    else
                        myobj.Add("Flete: 0");
                }
                else
                {
                    string suma_flete = string.Format("{0:0.00}", _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.cuscar_container_id == cno).Sum(a => a.flete));
                    if (operation == 23)
                        myobj.Add("Flete: " + suma_flete);
                    else
                        myobj.Add("Flete: 0");
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

                    if (id_aduana_det != 0) { }
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
                                totales = this._manifestServices.getTotales("FCL", mno, 6, bl_info.Select(a => a.bl_id).FirstOrDefault()).FirstOrDefault();
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
                            if (bl_det.codigo_tributario != "")
                                xnit = "NIT: " + bl_det.codigo_tributario + "+";
                            else
                                return Json(new { ID = 2, status = "Error", title = "Sin Nit para bl" + bl_det.no_bl }, JsonRequestBehavior.AllowGet);
                        }
                        else if (bl_det.tipo_docto == 2)
                            xnit = "AAO+";
                        else if (bl_det.tipo_docto == 3)
                            xnit = "EEE+";
                        else
                            return Json(new { ID = 2, status = "Error", title = "Falta algun tipo de documento ingresado para el Nit en el BL." }, JsonRequestBehavior.AllowGet);
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
                            volumen = bl_det.volumen.ToString();
                            no_piezas = bl_det.no_piezas;
                        }
                    }

                    int blcni = b + 1;
                    myobj.Add("Correlativo: " + blcni + "MBL :" + mbl);
                    myobj.Add("No BL: " + bl_det.no_bl);
                    myobj.Add("Peso: " + peso);
                    myobj.Add("No Piezas: " + no_piezas);

                    if (no_contenedor != "")
                    {
                        if (volumen != "0.00")
                            myobj.Add("Volumen: " + volumen);

                        int no_conte = container_info.Count();

                        if (tipo == "FCL")
                            if (noBls > 1 && no_containers == 1)
                                no_conte = container_info.Where(a => a.bl_id_ref == bl_det.bl_id || a.bl_id == bl_det.bl_id).Count();
                            else
                                no_conte = container_info.Where(a => a.bl_id_ref == bl_det.bl_id).Count();

                        myobj.Add("No Contenedores:" + no_conte);
                    }

                    decimal? flete = 0;
                    var tipo_moneda = "USD";
                    if (operation == 23)
                    {
                        flete = bl_det.flete;
                    }

                    myobj.Add("Flete: " + flete + " Tipo Moneda: " + tipo_moneda + "");
                    myobj.Add("Puerto Origen: " + puerto_origen.Substring(0, 2) + "::5" + "+" + puerto_origen + "::6");

                    if (id_aduana_det != 0)
                        myobj.Add("Aduanagln: " + aduanagln + "::9");

                    if (operation == 22)
                        myobj.Add("Puerto Desembarque: " + puerto_desembarque.Substring(0, 2) + "::5");
                    if (operation == 23)
                        myobj.Add("Destino Final: " + destino_final_det + "::5");
                    if (operation == 24)
                        myobj.Add("Destino Final: " + destino_final_det + "::5");

                    if (operation == 22)
                        myobj.Add("Puerto Embarque: " + whn(puerto_embarque_det) + "::9");
                    if (operation == 23)
                        myobj.Add("Puerto Embarque: " + whn(puerto_desembarque) + "::9");
                    if (operation == 24)
                        myobj.Add("Puerto Desembarque: " + whn(puerto_desembarque) + "::9");

                    myobj.Add("GIS+" + this._adminServices.iif(operation == 24 && id_aduana_det == 0, "23", operation.ToString()) + ":::8");

                    if (bl_det.flete == 0 || operation == 22)
                        myobj.Add("Sin Valor Flete");
                    else
                        myobj.Add("Con Valor Flete");

                    if (no_contenedor != "")
                        myobj.Add("Con Numero de Contenedor");
                    else
                        myobj.Add("Sin Numero de Contenedor");

                    string tests = bl_det.shipper;
                    myobj.Add("Shipper: " + this._adminServices.Break35(bl_det.shipper));

                    //myobj.Add("NAD+CN+" + xnit + this._adminServices.Break100_cliente(bl_det.cliente) + this._adminServices.Break100_direccion(bl_det.direccion_cliente));
                    //string cliente_direccion = bl_det.cliente + " " + bl_det.direccion_cliente;
                    //myobj.Add("NAD+CN+" + xnit + this._adminServices.Break35(cliente_direccion));

                    string cliente_break = bl_det.cliente.Truncate(70);
                    string direccion_cliente_break = bl_det.direccion_cliente.Truncate(105);

                    myobj.Add("Nit" + xnit + "Cliente: " + cliente_break + " Direccion: " + direccion_cliente_break);


                    if (tipo == "FCL")
                    {
                        if (noBls > 1)
                        {
                            no_piezas = bl_det.no_piezas;
                        }
                    }

                    if (no_contenedor == "")
                        myobj.Add("No de piezas: " + no_piezas);
                    else
                        myobj.Add("No de piezas: " + no_piezas + ":" + _adminServices.getTipoPaquete().Where(a => a.tipo_id == id_tipo_paquete).Select(a => a.iso).FirstOrDefault());

                    string como = comodity;

                    myobj.Add("Comodity: " + comodity);


                    myobj.Add("Peso: " + peso);

                    if (no_contenedor != "")
                    {
                        if (volumen != "0.00")
                            myobj.Add("Volumen: " + volumen);

                        if (tipo == "LCL")
                        {
                            var no_contenedor1 = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == bl_det.cuscar_container_id).Select(a => a.no_contenedor).FirstOrDefault();
                            myobj.Add("No Contenedor: " + no_contenedor1);
                        }
                        else
                        {
                            if (bl_info.Count() > 1 && no_containers == 1)
                            {
                                foreach (var vj1 in container_info.Where(a => a.bl_id_ref == bl_det.bl_id || a.bl_id == bl_det.bl_id))
                                {
                                    myobj.Add("No Contenedor: " + vj1.no_contenedor);
                                }
                            }
                            else if (bl_info.Count() > 1 && no_containers > 1)
                            {
                                foreach (var vj1 in container_info.Where(a => a.bl_id_ref == bl_det.bl_id))
                                {
                                    myobj.Add("No Contenedor: " + vj1.no_contenedor);
                                }
                            }
                            else
                            {
                                foreach (var vj2 in container_info)
                                {
                                    myobj.Add("No Contenedor: " + vj2.no_contenedor);
                                }
                            }
                        }

                    }
                }

            }


            myobj.Add("UNT+" + (myobj.Count() + 2) + "+" + mmsg);
            myobj.Add("UNZ+1+" + this._adminServices.pad0(mtrans.ToString(), 4));

            string tLine = "";
            
            for (int i = 0; i < myobj.Count(); i++)
            {
                tLine = tLine + myobj[i] + "\r\n";
            }
            
            modelPreview.myobj = myobj;


                
            //return PartialView("_cuscarPreview", modelPreview);
            return Json(new { ID = 1, allData = modelPreview, title = "Success" }, JsonRequestBehavior.AllowGet);
        }
        */
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
                default:
                    result = "NA";
                    break;
            }
            return result;
        }
    

      

    }

    
}
