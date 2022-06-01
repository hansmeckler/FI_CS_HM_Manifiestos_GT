using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ventas_gt.Entities.Entities;
using Manifiesto.Data.Models;
using Manifiesto.Web.Repositories.Models;

namespace Manifiesto.Data.Maritimo
{
    public interface IMaritimoServices
    {
        IEnumerable<cargos_bl> get_cargos_bl();

        IEnumerable<cuscar_viaje_info> get_viaje_info();

        IEnumerable<cuscar_container_info> get_container_info();

        IEnumerable<cuscar_container_info> get_container_info_id(int id_viaje);

        IEnumerable<cuscar_bl_info> get_bl_info();

        IEnumerable<totalesDet2> getTotales(string tipo_servicio, int cno, int cond, long? bl_id_ref);

        decimal getSumPeso(long? vc_id, int function, long container_id);

        void update_cuscar(cuscar_container_info cuscar_actual, cuscar_container_info cuscar_to_update);

        cuscar_container_info close_cuscar(long id, string firma);

        cuscar_container_info close_cuscar_fcl(long id, string firma);

        long update_bl_info(cuscar_bl_info cuscar_bl_info);

        long update_container_info(cuscar_container_info cuscar_container_info);

        long update_bl_info_lcl(cuscar_bl_info cuscar_bl_info);

        void update_container_original(cuscar_container_info_update cuscar_update_info, int id);

        void update_container_original_fcl(cuscar_container_info_update cuscar_update_info, int id);

        void insert_history(string mchgfield, string mcf, string mct, string mmn_id, string mnm_table, string mid_field, int? mid_value, bool mvalid);

        void update_cuscar(cuscar_container_info_vm cuscar_container_update);
    }
}
