using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Aereo;

namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    //[Authorize]
    public class ExportController : Controller
    {
        readonly IAereoService _aereoServices;


        public ExportController(IAereoService aereoServices)
        {
            this._aereoServices = aereoServices;
        }

        // 
        // GET: /Aereo/Export/
        
        public ActionResult Index()
        {
            var list = this._aereoServices.getGuiasExportList().OrderByDescending(a => a.createddate).Take(1000).ToList();
            return View(list);
        }

    }
}
