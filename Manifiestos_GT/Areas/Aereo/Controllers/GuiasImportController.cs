using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Manifiesto.Data.Aereo;
using Manifiesto.Data.Models;
using AutoMapper;
using Manifiesto.Web.Areas.Aereo.Models;

namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    public class GuiasImportController : Controller
    {
        /*    
        readonly IAereoService _aereoServices;

        public GuiasImportController(IAereoService aereoServices)
        {
            _aereoServices = aereoServices;
        }
        //
        // GET: /Aereo/awbImport/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            using (var context = new db_aereoEntities())
            {
                var list = context.awbiview.ToList();
                var jsonResult = Json(list.OrderByDescending(p => p.id).ToDataSourceResult(request));
                return jsonResult;
            }
        }

        public ActionResult Add(int id, string tipo, int optipo)
        {
            if (tipo != "")
            {
                try
                {
                    var aamntoInsert = this._aereoServices.FindToAddImport(id, optipo);
                    var dm = Mapper.Map<manifiesto_encVM, manifiesto_enc>(aamntoInsert);
                    var op = _aereoServices.AddImportMan(dm);
                    var result = _aereoServices.UpdatetMan(op.operation.Value);

                    return RedirectToAction("Edit", "Manifiesto", new { id = result, area = "Aereo" });
                }
                catch (Exception e)
                {
                    throw new NotImplementedException("Error en la transaccion.", e);
                }
            }
            else
                return View();
        }

        public ActionResult Detail(int id)
        {

            var guia = this._aereoServices.GetGuiasImport().Where(p => p.AwbID == id).Single();
            var vm = Mapper.Map<awbi, guiaImportViewModel>(guia);

            return View(vm);
        }
        */
  
    }
}
