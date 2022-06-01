using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Admin;
using Manifiesto.Web.Repositories.Models;
using master_aimar.Entities.Entities;
using System.Web.Security;
using Manifiesto.Web.Filters;

namespace Manifiesto.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IAdminService _adminServices;

        public HomeController(IAdminService adminServices)
        {
            this._adminServices = adminServices;
        }

        [Authorize]
        public ActionResult Index()
        {
            int tipo_acceso = Convert.ToInt32(Session["tipo_acceso"]);

            if (tipo_acceso == 1)
            {

                IEnumerable<Manifiesto.Web.Repositories.Models.empresas_model> list_empresas
                                = Manifiesto.Data.Maritimo.MaritimoServices.EmpresasUsuarios(User.Identity.Name, "master-aimar");
                
                /*IEnumerable<empresas_model> list_empresas = (from a in _adminServices.get_empresas()
                                                             join b in _adminServices.get_usuarios_x_empresa() on a.id_empresa equals b.id_empresa
                                                             join c in _adminServices.get_usuarios_empresas() on b.id_usuario equals c.id_usuario
                                                             where c.pw_name == User.Identity.Name
                                                             select new empresas_model
                                                             {
                                                                 id_usuario = Convert.ToInt32(b.id_usuario),
                                                                 id_empresa = a.id_empresa,
                                                                 empresa = a.nombre_empresa,
                                                                 nombre_pais = a.nombre_pais
                                                             }).ToList();*/    
                return View(list_empresas);
            }
            else if (tipo_acceso == 2)
                return View("Index_Aereo");
            else return View();
        }

        public ActionResult Principal(int id_empresa)
        {
            this.Session["id_empresa"] = id_empresa;

            //empresas empresa_info = this._adminServices.get_empresas().Where(a => a.id_empresa == id_empresa).FirstOrDefault();

            IEnumerable<master_aimar.Entities.Entities.empresas> list_empresas
                = Manifiesto.Data.Maritimo.MaritimoServices.GetEmpresas(id_empresa, "master-aimar");

            empresas empresa_info = list_empresas.FirstOrDefault();

            return View(empresa_info);
        }

        public ActionResult Titulo(int? id_empresa)
        {
            try
            {
                //IEnumerable<empresas> empresa_info = this._adminServices.get_empresas().Where(a => a.id_empresa == id_empresa).ToList();

                int id = Convert.ToInt32(id_empresa) == 0 ? -1 : Convert.ToInt32(id_empresa);

                //IEnumerable<master_aimar.Entities.Entities.empresas> 
                IEnumerable<empresas> empresa_info 
                    = Manifiesto.Data.Maritimo.MaritimoServices.GetEmpresas(id, "master-aimar");
                    
                return PartialView(empresa_info);
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            return null;
        }


    }
}
