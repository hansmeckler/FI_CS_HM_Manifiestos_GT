using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db_aereo.Entities.Entities;

namespace Manifiesto.Web.Repositories.CuscarServices
{
    public interface ICuscarAereoServices
    {
        IEnumerable<cuscar_voyage_info> getCuscar();

        cuscar_voyage_info add_voyage_cuscar_info(cuscar_voyage_info vm);

        int update_cuscar_voyage_info(int op);
    }
}
