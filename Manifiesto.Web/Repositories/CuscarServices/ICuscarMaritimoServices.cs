using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifiesto.Data.Models;
using ventas_gt.Entities.Entities;

namespace Manifiesto.Data.Trafico
{
    public interface ICuscarMaritimoServices
    {
        IEnumerable<listado_bls_VM> getListBls();

        bool validate_size_of_name_commodities(int id, string tipo_servicio);

        long add_cuscar(int id, string tipo_servicio);

        long add_cuscar2(int id, string tipo_servicio);
    }
}
