using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manifiesto.Web.Models;

namespace Manifiesto.Web.Domain
{
    public class TempData
    {
        public IEnumerable<Navbar> navbarItems()
        {
            var menu = new List<Navbar>();
            menu.Add(new Navbar { Id = 1, nameOption = "Aereo", controller = "Home", action = "Index", area="Aereo", estatus = true, isParent = true, parentId = 0, tipo_opcion = 1 });
            menu.Add(new Navbar { Id = 2, nameOption = "Maritimo", controller = "Home", action = "Index", area = "Maritimo", estatus = true, isParent = true, parentId = 0, tipo_opcion = 2 });
            menu.Add(new Navbar { Id = 3, nameOption = "Crear Nuevo Cuscar", controller = "Trafico", action = "Index", area = "Maritimo", estatus = true, isParent = false, parentId = 2, tipo_opcion = 2 });
            menu.Add(new Navbar { Id = 4, nameOption = "Listado de Cuscar", controller = "Manifest", action = "Index", area = "Maritimo", estatus = true, isParent = false, parentId = 2, tipo_opcion = 2 });
            menu.Add(new Navbar { Id = 5, nameOption = "Admin", controller = "Account", action = "Index", area = "", estatus = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Guias Import", controller = "Import", action = "Index", area = "Aereo", estatus = true, isParent = false, parentId = 1, tipo_opcion = 1 });
            menu.Add(new Navbar { Id = 7, nameOption = "Guias Export", controller = "Export", action = "Index", area = "Aereo", estatus = true, isParent = false, parentId = 1, tipo_opcion = 1 });
            menu.Add(new Navbar { Id = 8, nameOption = "Listado Cuscar", controller = "Manifest", action = "Index", area = "Aereo", estatus = true, isParent = false, parentId = 1, tipo_opcion = 1 });
            //menu.Add(new Navbar { Id = 5, nameOption = "Historico Cuscar's", controller = "Home", action = "Dropdown", area = "Maritimo", estatus = true, isParent = false, parentId = 2 });



            return menu.ToList();
        }



    }
}