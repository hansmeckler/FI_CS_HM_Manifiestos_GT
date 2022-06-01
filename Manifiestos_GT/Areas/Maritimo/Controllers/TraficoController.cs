using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Trafico;
using Manifiesto.Web.Filters;

namespace Manifiesto.Web.Areas.Maritimo.Controllers
{
    [Authorize]
    public class TraficoController : Controller
    {

        readonly ICuscarMaritimoServices _traficoServices;
        //
        // GET: /Maritimo/Trafico/
        public TraficoController(ICuscarMaritimoServices traficoServices)
        {
            this._traficoServices = traficoServices;
        }

        public ActionResult Index()
        {
            var list = _traficoServices.getListBls().ToList();
            return View(list);
        }

        
        public ActionResult Create(int id, string tipo_servicio)
        {
            bool list = _traficoServices.validate_size_of_name_commodities(id, tipo_servicio);

            if (list)
            {
                var lastId = _traficoServices.add_cuscar2(id, tipo_servicio);
                if (lastId == 0)
                    return RedirectToAction("Error", "Manifest");
                else
                    return RedirectToAction("Edit", "Manifest", new { id = lastId });
            }
            else
                return View();
        }
        

    }
}
