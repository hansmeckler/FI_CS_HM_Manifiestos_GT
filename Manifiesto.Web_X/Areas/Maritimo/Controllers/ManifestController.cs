using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Maritimo;
using Manifiesto.Web.Areas.Maritimo.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web.Script.Serialization;
using Manifiesto.Data.Models;
using Manifiesto.Data.Admin;
using ventas_gt.Entities.Entities;
using Manifiesto.Web.Filters;

namespace Manifiesto.Web.Areas.Maritimo.Controllers
{
    [Authorize]
    public class ManifestController : Controller
    {
        readonly IMaritimoServices _manifestServices;
        readonly IAdminService _adminServices;

        public ManifestController(IMaritimoServices manifestServices, IAdminService adminServices)
        {
            this._manifestServices = manifestServices;
            this._adminServices = adminServices;
        }

        //
        // GET: /Maritimo/Manifest/
        public ActionResult Index()
        {

            var enc = _manifestServices.get_viaje_info().ToList();
            var det = _manifestServices.get_container_info().ToList();

            var type = _adminServices.getType().ToList();
            var function = _adminServices.getFunction().ToList();
            var operation = _adminServices.getOperation().ToList();

            var manifests = (from a in enc
                             join b in det on a.cuscar_viaje_id equals b.cuscar_viaje_id
                             join c in type on b.mtype equals c.Id
                             join d in function on b.mfunction equals d.Id
                             join e in operation on b.operation equals e.Id
                             select new manifestListViewModel
                             {
                                 id = a.cuscar_viaje_id,
                                 no_viaje = a.no_viaje,
                                 manifest = b.manifest,
                                 original = b.original,
                                 cuscar = b.cuscar,
                                 type = c.type,
                                 cuscardt = b.cuscardt,
                                 function = d.function,
                                 operation = e.operation,
                                 funcsend = b.funcsend,
                                 test = b.test,
                                 tipo = a.tipo,
                                 import_export = a.import_export
                             }).OrderByDescending(a => a.id).Take(1000).ToList();


            foreach (var i in manifests)
            { 
                if (i.import_export == true)
                    i.import_export_desc = "Import";
                else
                    i.import_export_desc = "Export";
            }
            
            return View(manifests);
        }


        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var viaje_info = _manifestServices.get_viaje_info().Where(a => a.cuscar_viaje_id == id).ToList();
            var container_info = _manifestServices.get_container_info().Where(a => a.cuscar_viaje_id == id).OrderBy(a => a.cuscar_container_id).ToList();
            var bl_info = _manifestServices.get_bl_info().Where(a => a.cuscar_viaje_id == id).OrderBy(a => a.cuscar_bl_id).ToList();

            foreach (var m in viaje_info)
            {
                if (m.tipo == "FCL")
                {
                    manifestViewModelFCL manifestFCL = new manifestViewModelFCL();
                    manifestFCL.cuscar_viaje_info = viaje_info.FirstOrDefault();
                    manifestFCL.cuscar_container_info = container_info;
                    manifestFCL.cuscar_bl_info = bl_info.ToList();

                    if (manifestFCL.cuscar_viaje_info.import_export == true)
                        manifestFCL.import_export = "Import";
                    else
                        manifestFCL.import_export = "Export";

                    return View("EditFCL", manifestFCL);
                }
                else
                {
                    manifestViewModelLCL manifestLCL = new manifestViewModelLCL();
                    manifestLCL.cuscar_viaje_info = viaje_info.FirstOrDefault();
                    manifestLCL.cuscar_container_info = container_info;
                    manifestLCL.cuscar_bl_info = bl_info.ToList();


                    if (manifestLCL.cuscar_viaje_info.import_export == true)
                        manifestLCL.import_export = "Import";
                    else
                        manifestLCL.import_export = "Export";

                    return View("EditLCL", manifestLCL);
                }
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult CloseManifestLCL(FormCollection collection)
        {
            long container_id = Convert.ToInt64(collection[0]);
            string firma = collection[1].ToString();

            var container = this._manifestServices.close_cuscar(container_id, firma);

            return RedirectToAction("Edit", new { id = container.cuscar_viaje_id });

        }

        [HttpPost]
        public ActionResult CloseManifestFCL(FormCollection collection)
        {
            long viaje_id = Convert.ToInt64(collection[0]);
            string firma = collection[1].ToString();

            var container = this._manifestServices.close_cuscar_fcl(viaje_id, firma);

            return RedirectToAction("Edit", new { id = container.cuscar_viaje_id });

        }

        public ActionResult Download(string nameFile)
        {
            string path = HttpContext.Server.MapPath("~/Files/");
            return File(path + nameFile, "application/text", nameFile);
        }

        [HttpPost]
        public ActionResult UpdateManifestFCL(FormCollection collection) 
        {
            bool select = false;
            long cuscar_bl_id = Convert.ToInt32(collection[0]);
            long cuscar_viaje_id = Convert.ToInt32(collection[1]);
            string no_bl = collection[2];
            string cliente = collection[3];
            string direccion_cliente = collection[4];
            string shipper = collection[5];
            int tipo_docto = Convert.ToInt32(collection[6]);
            var selected = collection[7];
            if (selected == "false")
                select = false;
            else
                select = true;
            cuscar_bl_info bl_info = new cuscar_bl_info();
            bl_info.cuscar_bl_id = cuscar_bl_id;
            bl_info.cuscar_viaje_id = cuscar_viaje_id;
            bl_info.no_bl = no_bl;
            bl_info.cliente = cliente;
            bl_info.shipper = shipper;
            bl_info.tipo_docto = tipo_docto;
            //bl_info.ttm_id = ttm_id;
            bl_info.direccion_cliente = direccion_cliente;
            bl_info.selected2 = select;

            long id_viaje = this._manifestServices.update_bl_info(bl_info);

            return RedirectToAction("Edit", new { id = id_viaje });
        }

        [HttpPost]
        public ActionResult UpdateManifestLCL(FormCollection collection)
        {
            bool select = false;
            long cuscar_bl_id = Convert.ToInt32(collection[0]);
            long cuscar_viaje_id = Convert.ToInt32(collection[1]);
            string no_bl = collection[2];
            string comodity = collection[3];
            string cliente = collection[4];
            string direccion_cliente = collection[5];
            string shipper = collection[6];
            int tipo_docto = Convert.ToInt32(collection[7]);

            //int tipo_moneda = Convert.ToInt32(collection[8]);            
            var selected = collection[8];
            if (selected == "false")
                select = false;
            else
                select = true;
            
            cuscar_bl_info bl_info = new cuscar_bl_info();
            bl_info.cuscar_bl_id = cuscar_bl_id;
            bl_info.cuscar_viaje_id = cuscar_viaje_id;
            bl_info.no_bl = no_bl;
            bl_info.comodity = comodity;
            bl_info.cliente = cliente;
            bl_info.shipper = shipper;
            bl_info.tipo_docto = tipo_docto;
            //bl_info.ttm_id = tipo_moneda;
            bl_info.direccion_cliente = direccion_cliente;
            bl_info.selected2 = select;

            long id_viaje = this._manifestServices.update_bl_info_lcl(bl_info);

            return RedirectToAction("Edit", new { id = id_viaje });
        }

        [HttpPost]
        public ActionResult UpdateContainer(FormCollection collection)
        {
            long cuscar_viaje_id = Convert.ToInt32(collection[0]);
            long cuscar_container_id = Convert.ToInt32(collection[1]);
            string comodity = collection[2];

            cuscar_container_info container_info = new cuscar_container_info();
            container_info.cuscar_viaje_id = cuscar_viaje_id;
            container_info.cuscar_container_id = cuscar_container_id;
            container_info.comodity = comodity;
            
            long id_viaje = this._manifestServices.update_container_info(container_info);

            return RedirectToAction("Edit", new { id = id_viaje });
        }

    }
}
