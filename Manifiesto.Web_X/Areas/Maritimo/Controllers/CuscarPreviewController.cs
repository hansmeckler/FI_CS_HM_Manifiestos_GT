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
    [Authorize]
    public class CuscarPreviewController : Controller
    {
        readonly IMaritimoServices _manifestServices;
        readonly IAdminService _adminServices;

        public CuscarPreviewController(IMaritimoServices manifestServices, IAdminService adminServices)
        {
            this._manifestServices = manifestServices;
            this._adminServices = adminServices;
        }

        

        //
        // GET: /Maritimo/CuscarPreview/

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

            CuscarParametersVM parametros = new CuscarParametersVM() { no_viaje = mno, no_contenedor = cno, type = type, function_t = function, operation = operation, funcsend = funcsend, fix = fix };
            CuscarPreviewVM modelPreview = new CuscarPreviewVM();
            cuscar_viaje_infoVM cuscar_viaje_infoVM = new cuscar_viaje_infoVM();

            IList<cuscar_bl_infoVM> cuscar_bl_infoVM_list = new List<cuscar_bl_infoVM>();
            IList<cuscar_container_infoVM> cuscar_container_infoVM_list = new List<cuscar_container_infoVM>();

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
            long cuscar_container_id = 0;

            long? noBls = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Count();

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

            int? seq_next = (sequence.nexttransmission + 1);

            string mmnarc = "";

            if (pais == 1)
                mmnarc = "P49E" + seq_next;
            else
                mmnarc = "P18F" + seq_next;

            var manifest = container_info.Select(a => a.manifest).FirstOrDefault();

            if (pais == 1)
                manifest = "49E" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 5);
            else
                manifest = "18F" + (DateTime.Now.Year).ToString().Substring(2, 2) + this._adminServices.pad0(mmnfst, 5);


            var moriginal = container_info.Select(a => a.original).FirstOrDefault();


            modelPreview.no_cuscar = manifest;
            modelPreview.type = type.ToString();
            modelPreview.function = function.ToString();
            modelPreview.sender = funcsend;
 
            if (function != 1)
            {
                
                if (viaje != "")
                    cuscar_viaje_infoVM.no_viaje = viaje;
                else
                   cuscar_viaje_infoVM.no_viaje = no_bl;
                
               cuscar_viaje_infoVM.puerto_origen = this._adminServices.iif(operation == 22, "8+", "125+") + this._adminServices.iif(operation == 22, puerto_desembarque, puerto_origen);
               cuscar_viaje_infoVM.puerto_desembarque = this._adminServices.iif(operation == 22, "178+", "179+") + this._adminServices.iif(operation == 22, puerto_origen, puerto_desembarque);

               //cuscar_viaje_infoVM.import_export = operation;
                int i = 0;
                foreach (var vj in container_info)
                {              
                    cuscar_container_infoVM cuscar_container_infoVM = new cuscar_container_infoVM();
                    i++;
                    /*
                    cuscar_container_update.cuscar_container_id = vj.cuscar_container_id;
                    cuscar_container_update.mfunction = function;
                    cuscar_container_update.operation = operation;
                    cuscar_container_update.funcsend = funcsend;
                    cuscar_container_update.mtype = Convert.ToInt32(type);
                    */
                    //this._manifestServices.update_cuscar(cuscar_container_update);

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
                            peso = string.Format("{0:0.00}", _manifestServices.getSumPeso(container_info.Select(a => a.cuscar_viaje_id).FirstOrDefault(), function, vc_id));
                        else
                            peso = string.Format("{0:0.00}", vj.peso);

                        // Peso Bls
                        //if (tipo == "LCL")
                            //peso = _manifestServices.getSumPeso(container_info.Select(a => a.cuscar_viaje_id).FirstOrDefault(), function, vc_id);


                        volumen = string.Format("{0:0.00}", vj.volumen);
                        comodity = vj.comodity;
                        no_bls = vj.no_bls;

                        cuscar_container_infoVM.volumen = vj.volumen.ToString();
                        cuscar_container_infoVM.comodity = vj.comodity;
                        cuscar_container_infoVM.no_bls = vj.no_bls;
                        cuscar_container_infoVM.bl_id = vj.bl_id;
                        cuscar_container_infoVM.bl_id_ref = vj.bl_id_ref;
                        cuscar_container_infoVM.cuscar_container_id = vj.cuscar_container_id;
                        cuscar_container_infoVM.container_type = vj.container_type;
                        cuscar_container_infoVM.bl_id_ref = vj.bl_id_ref;
                        cuscar_container_infoVM.no_bl = this._manifestServices.get_bl_info().Where(a => a.bl_id == vj.bl_id_ref).Select(a => a.no_bl).FirstOrDefault();

                        string iso = "";
                        int mt = 0;
                        var containerTypes = this._adminServices.getContainerType();

                        IEnumerable<container_typeViewModel> containerTypesVM = Mapper.Map<IEnumerable<container_type>, IEnumerable<container_typeViewModel>>(containerTypes);

                        iso = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.iso).FirstOrDefault();
                        mt = containerTypesVM.Where(a => a.id_container_type == vj.id_container_type).Select(a => a.mt).FirstOrDefault();
                   
                        if (no_contenedor != "")
                        {
                            cuscar_container_infoVM.no_contenedor = no_contenedor;
                            cuscar_container_infoVM.peso = peso;
                            cuscar_container_infoVM.mt = mt.ToString();

                            if (seal != "")
                                cuscar_container_infoVM.seal = seal;
                        }

                        cuscar_container_infoVM.no_contenedor = no_contenedor;

                        cuscar_container_infoVM.no_piezas = vj.no_piezas;
                        cuscar_container_infoVM.comodity = vj.comodity;
                        cuscar_container_infoVM.tipo_paquete = vj.tipo_paquete;
                        cuscar_container_infoVM.peso = peso;
                        cuscar_container_infoVM_list.Add(cuscar_container_infoVM);
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

                modelPreview.total_bls = totbls.ToString();
                modelPreview.total_peso = string.Format("{0:0.00}", totpeso);


                if (no_contenedor != "" && totvol != 0)
                    modelPreview.total_volumen = totvol.ToString();

                modelPreview.total_piezas = totpcs.ToString();

                if (no_contenedor != "")
                    modelPreview.no_contenedores = container_info.Count().ToString();

                if (tipo == "FCL")
                {
                    string suma_flete = string.Format("{0:0.00}", _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno).Sum(a => a.flete));
                    if (operation == 23)
                        modelPreview.flete = suma_flete;
                    else
                        modelPreview.flete = "0";
                }
                else
                {
                    string suma_flete = string.Format("{0:0.00}", _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == mno && a.cuscar_container_id == cno).Sum(a => a.flete));
                    if (operation == 23)
                        modelPreview.flete = suma_flete;
                    else
                        modelPreview.flete = "0";
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
                int z = 0;
                foreach (var bl_det in bl_info)
                {
                    cuscar_bl_infoVM cuscar_bl_infoVM = new cuscar_bl_infoVM();
                    z++;
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

                    cuscar_bl_infoVM.cuscar_container_id = bl_det.cuscar_container_id;

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
                                totales = this._manifestServices.getTotales("FCL", mno, 5, bl_det.bl_id).FirstOrDefault();
                            else
                                totales = this._manifestServices.getTotales("FCL", mno, 7, bl_det.bl_id).FirstOrDefault();
                        }
                        else
                        {
                            totales = this._manifestServices.getTotales("FCL", mno, 4, bl_det.bl_id).FirstOrDefault();
                        }

                        peso = string.Format("{0:0.00}", totales.peso);
                        volumen = string.Format("{0:0.00}", totales.volumen);
                        no_piezas = Convert.ToInt32(totales.no_piezas);
                    }

                    cuscar_bl_infoVM.tipo_docto = bl_det.tipo_docto;
                    

                    if (operation == 23)
                    {
                        if (bl_det.tipo_docto == 1)
                        {
                            cuscar_bl_infoVM.tipo_docto_desc = "NIT";
                            if (bl_det.codigo_tributario != "")
                                xnit = bl_det.codigo_tributario;
                            else
                                return Json(new { ID = 2, status = "Error", title = "Sin Nit para bl" + bl_det.no_bl }, JsonRequestBehavior.AllowGet);
                        }
                        else if (bl_det.tipo_docto == 2)
                            cuscar_bl_infoVM.tipo_docto_desc = "AAO";
                        else if (bl_det.tipo_docto == 3)
                            cuscar_bl_infoVM.tipo_docto_desc = "EEE";
                        else if (bl_det.tipo_docto == 4)
                            cuscar_bl_infoVM.tipo_docto_desc = "NTG";
                        else
                            return Json(new { ID = 2, status = "Error", title = "Falta algun tipo de documento ingresado para el Nit en el BL." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var cliente = _adminServices.getClientes().Where(a => a.id_cliente == bl_det.id_cliente);
                        if (cliente.Any(a => a.id_cliente == bl_det.id_cliente))
                            cuscar_bl_infoVM.tipo_docto_desc = "NIT";
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

                    cuscar_bl_infoVM.correlativo = blcni.ToString();
                    cuscar_bl_infoVM.mbl = bl_det.mbl;
                    cuscar_bl_infoVM.no_bl = bl_det.no_bl;
                    cuscar_bl_infoVM.peso = peso;
                    cuscar_bl_infoVM.no_piezas = no_piezas.ToString();
                    cuscar_bl_infoVM.bl_id = bl_det.bl_id;

                    if (no_contenedor != "")
                    {
                        if (volumen != "0.00")
                        {
                            cuscar_bl_infoVM.volumen = volumen;
                        }
                        int no_conte = container_info.Count();

                        if (tipo == "FCL")
                            if (noBls > 1 && no_containers == 1)
                                no_conte = 1;
                            else
                                no_conte = container_info.Where(a => a.bl_id_ref == bl_det.bl_id).Count();

                        cuscar_bl_infoVM.no_contenedor = no_conte.ToString();
                    }

                    decimal? flete = 0;
                    var tipo_moneda = "USD";

                    if (operation == 23)
                        flete = bl_det.flete;

                    cuscar_bl_infoVM.flete = flete;
                    cuscar_bl_infoVM.tipo_moneda = tipo_moneda;

                    cuscar_bl_infoVM.puerto_embarque = puerto_origen.Substring(0, 2) + puerto_origen;

                    if (id_aduana_det != 0)
                        cuscar_bl_infoVM.aduanagln = aduanagln;

                    if (operation == 22)
                        cuscar_bl_infoVM.puerto_desembarque = puerto_desembarque;
                    if (operation == 23)
                        cuscar_bl_infoVM.destino_final = destino_final_det;
                    if (operation == 24)
                        cuscar_bl_infoVM.destino_final = destino_final_det;

                    if (operation == 22)
                        cuscar_bl_infoVM.puerto_embarque = whn(puerto_embarque_det);
                    if (puerto_desembarque == "GTPRQ" && puertos.id_deposito == 34029)
                    {
                        if (operation == 23)
                            cuscar_bl_infoVM.puerto_desembarque = "GTTCQ";
                        if (operation == 24)
                            cuscar_bl_infoVM.puerto_desembarque = "GTTCQ";
                    }
                    else
                    {
                        if (operation == 23)
                            cuscar_bl_infoVM.puerto_desembarque = puerto_desembarque;
                        if (operation == 24)
                            cuscar_bl_infoVM.puerto_desembarque = puerto_desembarque;
                    }

           
                    if (bl_det.flete == 0 || operation == 22)
                        cuscar_bl_infoVM.flete_val = "Sin Flete";
                    else
                        cuscar_bl_infoVM.flete_val = "Con Flete";

                    string tests = bl_det.shipper;
                    cuscar_bl_infoVM.shipper = this._adminServices.Break35(bl_det.shipper);

                    string cliente_break = bl_det.cliente.Truncate(70);
                    string direccion_cliente_break = bl_det.direccion_cliente.Truncate(105);

                    cuscar_bl_infoVM.nit = xnit;

                    cuscar_bl_infoVM.cliente = cliente_break;
                    cuscar_bl_infoVM.direccion_cliente = direccion_cliente_break;

                    if (tipo == "FCL")
                    {
                        if (noBls > 1)
                            no_piezas = bl_det.no_piezas;
                    }

                    if (no_contenedor == "")
                        cuscar_bl_infoVM.no_piezas = no_piezas.ToString();
                    else
                        cuscar_bl_infoVM.no_piezas = no_piezas + " - "  + _adminServices.getTipoPaquete().Where(a => a.tipo_id == id_tipo_paquete).Select(a => a.iso).FirstOrDefault();

                    string como = comodity;

                    cuscar_bl_infoVM.comodity = comodity;
                    cuscar_bl_infoVM.peso = peso;
                    
                    if (no_contenedor != "")
                    {
                        if (volumen != "0.00")
                            cuscar_bl_infoVM.volumen = volumen;

                        if (tipo == "LCL")
                        {
                            var no_contenedor1 = _manifestServices.get_container_info().Where(a => a.cuscar_container_id == bl_det.cuscar_container_id).Select(a => a.no_contenedor).FirstOrDefault();
                            cuscar_bl_infoVM.no_contenedor = no_contenedor1;
                        }
                        else
                        {
                            if (bl_info.Count() > 1 && no_containers == 1)
                            {
                                foreach (var vj1 in container_info.Where(a => a.cuscar_viaje_id == bl_det.cuscar_viaje_id))
                                {
                                    cuscar_bl_infoVM.no_contenedor = vj1.no_contenedor;
                                }
                            }
                            else if (bl_info.Count() > 1 && no_containers > 1)
                            {
                                foreach (var vj1 in container_info.Where(a => a.bl_id_ref == bl_det.bl_id))
                                {
                                    cuscar_bl_infoVM.no_contenedor = vj1.no_contenedor;
                                }
                            }
                            else
                            {
                                foreach (var vj2 in container_info)
                                {
                                    cuscar_bl_infoVM.no_contenedor = vj2.no_contenedor;
                                }
                            }
                        }

                    }
                    
                    cuscar_bl_infoVM_list.Add(cuscar_bl_infoVM);
                }

            }
            modelPreview.cuscar_viaje_info = cuscar_viaje_infoVM;
            modelPreview.cuscar_bl_info = cuscar_bl_infoVM_list;
            modelPreview.cuscar_container_info = cuscar_container_infoVM_list;
                
            //return PartialView("_cuscarPreview", modelPreview);
            return Json(new { ID = 1,  allData = modelPreview, title = "Success" }, JsonRequestBehavior.AllowGet);
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
                default:
                    result = "NA";
                    break;
            }
            return result;
        }
    

      

    }

    
}
