using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifiesto.Data.Entities;
using Manifiesto.Data.Models;
using master_aimar.Entities.Entities;
using ventas_gt.Entities.Entities;
using Manifiesto.Web.Models;


namespace Manifiesto.Data.Admin
{
    public interface IAdminService
    {
        void DoAuth(string userName, bool rememberMe);

        void DoLogOff();

        string Encrypt_MD5(string password);

        IEnumerable<Usuarios_EmpresasViewModel> get_users_login();

        string iif(bool condition, string t, string f);

        string pad0(string mstring, int l);

        //string Break35(string text);

        string Break100_cliente(string text);

        string Break100_direccion(string text);

        string Break35(string text);

        IEnumerable<cuscar_users_VM> get_cuscar_users();

        IEnumerable<usuarios_empresas> get_usuarios_empresas();

        cuscar_users_VM addUser(cuscar_users_VM vm);

        cuscar_users_VM editUser(cuscar_users_VM vm);

        aaseqEntity getAaseq();

        IEnumerable<TypeEntity> getType();

        IEnumerable<functionEntity> getFunction();

        IEnumerable<operationEntity> getOperation();

        IEnumerable<container_type> getContainerType();

        IEnumerable<clientes> getClientes();

        IEnumerable<tipo_paquete> getTipoPaquete();

        IEnumerable<aduanas> getAduanas();

        string getManifiestoMaster(string tipo, long? viaje_id);

        IEnumerable<empresas> get_empresas();

        IEnumerable<usuarios_x_empresa> get_usuarios_x_empresa();

        IEnumerable<usuarios_permisos_manifiestos> get_usuarios_permisos_manifiestos();

        //string getNombreCliente(long? idCliente);

        //string getDireccionCliente(long? idCliente);

    }
}
