using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Aereo;

namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    //[Authorize]
    public class ImportController : Controller
    {
        readonly IAereoService _aereoServices;
        //
        // GET: /Aereo/Import/

        public ImportController(IAereoService aereoServices)
        {
            this._aereoServices = aereoServices;
        }

        public ActionResult Index()
        {
            var list = this._aereoServices.getGuiasImportList().OrderByDescending(a => a.createddate).Take(1000).ToList();
            return View(list);
        }


    }
}
