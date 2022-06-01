using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using Manifiesto.Web.Models;
using master_aimar.Entities;
using Manifiesto.Data.Admin;
using master_aimar.Entities.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Manifiesto.Web.Controllers
{
    public class AccountController : Controller
    {

        readonly IAdminService _adminServices;

        public AccountController(IAdminService adminServices)
        {
            _adminServices = adminServices;
        }

        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<cuscar_users_VM> cuscar_users = _adminServices.get_cuscar_users().Where(a => a.id_usuario != 1);
            return View(cuscar_users);
        }
        /*
        public ActionResult Get_Detail([DataSourceRequest] DataSourceRequest request)
        {
            return Json(this.service.GetAll().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add_Detail([DataSourceRequest] DataSourceRequest request, CategoriesViewModel categories)
        {
            var newcategory = this.service.Add(categories);
            return Json(new[] { newcategory }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update_Detail([DataSourceRequest] DataSourceRequest request, CategoriesViewModel categories)
        {

            this.service.Update(categories);
            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete_Detail([DataSourceRequest] DataSourceRequest request, CategoriesViewModel categories)
        {
            var id = categories.CategoryID;
            this.service.Delete(id);
            return Json(ModelState.ToDataSourceResult());
        }
        */
        [Authorize]
        public ActionResult Create()
        {
            return View(new cuscar_users_VM());
        }

        public ActionResult GetUsers_Read(string text)
        {
            var users = (from a in _adminServices.get_usuarios_empresas()
                         select new cuscar_users_VM
                         {
                             id_usuario = a.id_usuario,
                             nombre_usuario = a.pw_name,
                             nombre = a.pw_gecos,
                             apellido = "",
                             password = a.pw_passwd
                         });

            if (!string.IsNullOrEmpty(text))
            {
                users = users.Where(p => p.nombre_usuario.Contains(text));
            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(cuscar_users_VM cuscar_users)
        {
            this._adminServices.addUser(cuscar_users);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            return View(this._adminServices.get_cuscar_users().Select(a => new cuscar_users_VM 
                        { 
                            id_usuario = a.id_usuario,
                            nombre_usuario = a.nombre_usuario,
                            admin = a.admin,
                            aereo = a.aereo,
                            marino = a.marino
                        }).Where(a => a.id_usuario == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(cuscar_users_VM cuscar_users)
        {
            this._adminServices.editUser(cuscar_users);
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string pass = this._adminServices.Encrypt_MD5(model.Password);

            if (this._adminServices.get_users_login().Any(p => p.pw_name == model.UserName && p.pw_passwd == pass))
            {
                var usuario = this._adminServices.get_users_login().Where(a => a.pw_name == model.UserName && a.pw_passwd == pass).Select(a => a.id_usuario).FirstOrDefault();

                var passwd_dias = this._adminServices.get_users_login().Where(a => a.pw_name == model.UserName && a.pw_passwd == pass).Select(a => a.pw_passwd_dias).FirstOrDefault();
                var passwd_fecha = this._adminServices.get_users_login().Where(a => a.pw_name == model.UserName && a.pw_passwd == pass).Select(a => a.pw_passwd_fecha).FirstOrDefault();
                var dias = (DateTime.Now - passwd_fecha).Days;

                if (dias > passwd_dias)
                {
                    var cambio = System.Diagnostics.Process.Start("http://10.10.1.20/catalogo_admin/cambio/index.php");
                    ModelState.AddModelError("", "Su contraseña ha vencido, favor de actualizarla" + cambio);
                    return View(model);
                }
                else
                {
                    this.Session["tipo_acceso"] = model.tipo;
                    this.Session["id_usuario"] = usuario;
                    this._adminServices.DoAuth(model.UserName, model.RememberMe);

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this._adminServices.DoLogOff();
            return RedirectToAction("Login", "Account");
        }

    }
}
