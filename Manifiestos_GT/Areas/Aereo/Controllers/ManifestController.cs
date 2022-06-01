using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Aereo;
using AutoMapper;
using db_aereo.Entities.Entities;
using Manifiesto.Data.Models;
using Manifiesto.Web.Repositories.CuscarServices;
using db_aereo.Entities;
using Manifiesto.Web.Areas.Aereo.Models;


namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    //[Authorize]
    public class ManifestController : Controller
    {

        readonly IAereoService _aereoServices;
        readonly ICuscarAereoServices _cuscarServices;

        public ManifestController(IAereoService aereoServices, ICuscarAereoServices cuscarServices)
        {
            this._aereoServices = aereoServices;
            this._cuscarServices = cuscarServices;
        }
        

        public ActionResult Index()
        {
            var list = this._aereoServices.get_cuscar_voyage_info().ToList();
            return View(list);
        }


        public ActionResult Create(int id, int tipo_servicio)
        {
            manifiesto_encVM guia = findGuia(id, tipo_servicio);
            var dm = Mapper.Map<manifiesto_encVM, cuscar_voyage_info>(guia);

            cuscar_voyage_info value = create_voyage(dm);

            var result = this._cuscarServices.update_cuscar_voyage_info(value.operation.Value);           
            return RedirectToAction("Edit", "Manifest", new { id = result, area = "Aereo" });
        }

        public manifiesto_encVM findGuia(int id, int tipo_servicio)
        {
            var guia = this._aereoServices.FindToAdd(id, tipo_servicio);
            return guia;
        }

        public cuscar_voyage_info create_voyage(cuscar_voyage_info dm)
        {
            var op = this._cuscarServices.add_voyage_cuscar_info(dm);
            return op;
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var context = new db_aereoContext())
                {
                    var vm = new ManifiestoViewModel();

                    var encabezado = context.cuscar_bl_info.Where(p => p.cuscar_voyage_id == id);
                    var encList = this._cuscarServices.getCuscar().Where(p => p.cuscar_voyage_id == id).Single();
                    var detList = context.cuscar_bl_info.Where(p => p.cuscar_voyage_id == id).ToList();

                    var sumPiezas = encabezado.Select(p => p.no_piezas).Sum();
                    vm.no_piezas = sumPiezas;

                    var sumPeso = encabezado.Select(p => p.peso).Sum();
                    vm.peso = sumPeso;

                    var manifiesto_enc_vm = Mapper.Map<cuscar_voyage_info, manifiestoEncViewModel>(encList);
                    IEnumerable<manifiestoDetViewModel> vmList = Mapper.Map<IEnumerable<cuscar_bl_info>, IEnumerable<manifiestoDetViewModel>>(detList);

                    vm.manifiesto_enc = manifiesto_enc_vm;
                    vm.manifiesto_det = vmList;

                    return View(vm);
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException("Error en la transaccion.", e);
            }
        }

        [HttpPost]
        public ActionResult CloseManifestAereo(FormCollection collection)
        {
            long container_id = Convert.ToInt64(collection[0]);
            string firma = collection[1].ToString();

            var container = this._aereoServices.close_cuscar(container_id, firma);

            return RedirectToAction("Edit", new { id = container.cuscar_voyage_id });

        }

        public ActionResult Download(string nameFile)
        {
            string path = HttpContext.Server.MapPath("~/Files_A/");
            return File(path + nameFile, "application/text", nameFile);
        }

        [HttpPost]
        public ActionResult UpdateManifest(FormCollection collection)
        {
            int cuscar_bl_id = Convert.ToInt32(collection[0]);
            int cuscar_viaje_id = Convert.ToInt32(collection[1]);
            string shipper = collection[2];
            string cliente = collection[3];
            string direccion = collection[4];
            string commodity = collection[5];
            int tipo_docto = Convert.ToInt32(collection[6]);
            int ttm_id = Convert.ToInt32(collection[7]);
            double? flete = Convert.ToDouble(collection[8]);

            cuscar_bl_info bl_info = new cuscar_bl_info();
            bl_info.cuscar_bl_id = cuscar_bl_id;
            bl_info.cuscar_voyage_id = cuscar_viaje_id;
            bl_info.cliente = cliente;
            bl_info.direccion = direccion;
            bl_info.shipper = shipper;
            bl_info.tipo_docto = tipo_docto;
            bl_info.ttm_id = ttm_id;
            bl_info.flete = flete;
            bl_info.comodity = commodity;

            int? id_viaje = this._aereoServices.update_bl_aereo(bl_info);

            return RedirectToAction("Edit", new { id = cuscar_viaje_id });
        }

    }

}
