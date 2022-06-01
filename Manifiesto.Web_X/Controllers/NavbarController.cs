using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Web.Domain;
using master_aimar.Entities;
using master_aimar.Entities.Entities;
using Manifiesto.Web.Models;
using Manifiesto.Data.Admin;

namespace Manifiesto.Web.Controllers
{
    //[AllowAnonymous]
    public class NavbarController : Controller
    {

        readonly IAdminService _adminServices;

        public NavbarController(IAdminService adminServices)
        {
            this._adminServices = adminServices;
        }
        //
        // GET: /Navbar/
        //
        //[AllowAnonymous]
        public ActionResult Navbar(int? id_empresa)
        {
            var data = new TempData();
            var items = new List<Navbar>();

            int tipo_acceso = Convert.ToInt32(Session["tipo_acceso"]);

            if (Request.IsAuthenticated)
            {
                if (Session["tipo_acceso"] != null)
                {
                    if (tipo_acceso == 1)
                    {
                        var menu_items = data.navbarItems().ToList();

                        items = (from a in menu_items
                                 join b in this._adminServices.get_usuarios_permisos_manifiestos() on a.Id equals b.id_menu
                                 join c in this._adminServices.get_usuarios_empresas() on b.id_usuario equals c.id_usuario
                                 where c.pw_name == User.Identity.Name && b.id_empresa == id_empresa && a.tipo_opcion == 2
                                 select new Navbar
                                 {
                                     Id = a.Id,
                                     nameOption = a.nameOption,
                                     controller = a.controller,
                                     action = a.action,
                                     area = a.area,
                                     imageClass = a.imageClass,
                                     activeli = a.activeli,
                                     estatus = a.estatus,
                                     parentId = a.parentId,
                                     isParent = a.isParent,
                                     hasChild = a.hasChild
                                 }).ToList();

                        return PartialView("_navbar", items);

                    }
                    else
                    {
                        var menu_items = data.navbarItems().ToList();

                        items = (from a in menu_items
                                 join b in this._adminServices.get_usuarios_permisos_manifiestos() on a.Id equals b.id_menu
                                 join c in this._adminServices.get_usuarios_empresas() on b.id_usuario equals c.id_usuario
                                 where c.pw_name == User.Identity.Name && a.tipo_opcion == 1
                                 select new Navbar
                                 {
                                     Id = a.Id,
                                     nameOption = a.nameOption,
                                     controller = a.controller,
                                     action = a.action,
                                     area = a.area,
                                     imageClass = a.imageClass,
                                     activeli = a.activeli,
                                     estatus = a.estatus,
                                     parentId = a.parentId,
                                     isParent = a.isParent,
                                     hasChild = a.hasChild
                                 }).ToList();

                        return PartialView("_navbar", items);
                    }
                }
                else
                    return PartialView("_navbar", items);
            }
            else
            {
                items = items.ToList();
                return PartialView("_navbar", items);
            }
        }

    }

}
