using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db_aereo.Entities.Entities;
using Manifiesto.Web.Areas.Aereo.Models;
using Manifiesto.Data.Models;

namespace Manifiesto.Data.Aereo
{
    public interface IAereoService
    {

        IEnumerable<awbi> getGuiasImport();

        IEnumerable<guiasListViewModel> getGuiasImportList();

        IEnumerable<guiasListViewModel> getGuiasExportList();

        IEnumerable<cuscar_voyage_info> get_cuscar_voyage_info();

        IEnumerable<cuscar_bl_info> get_cuscar_bl_info();

        IEnumerable<totalesDet2> get_totales(int cuscar_voyage_id);

        manifiesto_encVM FindToAdd(int id, int optipo);

        int? update_bl_aereo(cuscar_bl_info cuscar_bl_info);

        cuscar_voyage_info close_cuscar(long id, string firma);
        /*
        IEnumerable<manifiesto_enc> GetManifiestos();

        IEnumerable<manifiesto_det> GetManifiestoDet();

        IEnumerable<awbi> GetGuiasImport();

        IEnumerable<awb> GetGuiasExport();

        IEnumerable<awbiview> GetImportView();

        IEnumerable<awbview> GetExportView();

        manifiesto_encVM FindToAddImport(int id, int optipo);
    
        IEnumerable<totalesDet> totalesDet();
         * */
    }
}
