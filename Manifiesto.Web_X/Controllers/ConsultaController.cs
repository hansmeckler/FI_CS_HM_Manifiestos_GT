using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using db_aereo.Entities;
using master_aimar.Entities;
using Manifiesto.Web.Models;
using Npgsql;
using master_aimar.Entities.Entities;
using MySql.Data.MySqlClient;

namespace Manifiesto.Web.Controllers
{
    public class ConsultaController : Controller
    {
        //
        // GET: /Consulta/

        public ActionResult Index()
        {

            var aereContext = new db_aereoContext();
            var master = new master_aimarEntities();

            var paises = master.paises.ToList();
            var regiones = master.region.ToList();

            var import = aereContext.guia_import.ToList();
            /*
            string query = "select extract(year from (STR_TO_DATE(a.CreatedDate,'%Y-%m-%d'))) as anio, a.Countries as origen, b.Country as pais, " +
                           "(case when (a.WeightsSymbol  <> 'KG' and a.WeightsSymbol  <> 'KGS') then (a.TotWeight)  else a.TotWeight End) AS peso " +
                           "from Awbi a " +
                           "left join Airports as b on(a.AirportDepID = b.AirportID) " +
                           "left join Airports as c on(a.AirportDesID = c.AirportID) " +
                           "where a.Countries = 'GT' and a.RoutingID = 0 " +
                           "and extract(year from (STR_TO_DATE(a.CreatedDate,'%Y-%m-%d')))in (2012,2013,2014,2015) " +
                           "group by anio, pais, a.Countries order by anio, origen;"; 
            var list = aereContext.Database.SqlQuery<reporteVM>(query).ToList();
            */
            string connStr = "Server=10.10.1.18;User Id=manifiestos;Password=m@N1f13sT0s;Database=db_aereo";
            var m_conn = new MySqlConnection(connStr);

            IList<reporteVM> list = new List<reporteVM>();

              using (var connec = new MySqlConnection(connStr))
                {
                    using (var coman = new MySqlCommand())
                    {
                        connec.Open();
                        coman.Connection = connec;
                        coman.CommandText = "select year(a.CreatedDate) as anio, a.Countries as origen, c.Country as pais, " +
                           "sum(a.TotWeight) AS peso " +
                           "from Awb a " +
                           "inner join Airports as c on(a.AirportDesID = c.AirportID) " +
                           "where a.Countries in ('GT', 'CR', 'SV', 'HN', 'NI', 'PA', 'BZ', 'N1')  and a.RoutingID <> 0 and a.WeightsSymbol in ('KG', 'KGS') " +
                           "and a.CreatedDate <= '2015-08-31' and year(a.CreatedDate)in (2012,2013,2014,2015) " +
                           "group by anio, pais, a.Countries order by anio, origen;"; 
                    
                        coman.CommandTimeout = 0;
                        MySqlDataReader reader = coman.ExecuteReader();
                        
                       while ((reader.Read())) {
                           reporteVM ob = new reporteVM();

                            ob.anio = Convert.ToInt32(reader["anio"]);
                            ob.origen = Convert.ToString(reader["origen"]);
                            ob.pais = Convert.ToString(reader["pais"]);
                            ob.peso = Convert.ToDecimal(reader["peso"]);
                            list.Add(ob);
		               }
                       connec.Close();
                    }
                }

          
            var testFinal = (from a in list
                              join b in paises on a.pais equals b.codigo
                              join c in regiones on b.id_region equals c.id_region
                              select new Totales
                              {
                                  anio = a.anio,                                
                                  import_export = false,
                                  cif = false,
                                  region = c.descripcion,
                                  pais = b.descripcion,
                                  peso = a.peso
                              }).OrderByDescending(a => a.region).ToList();


            aereo_temp myobject = new aereo_temp();

            foreach (var cc in testFinal)
            {

                myobject.anio = cc.anio;
                myobject.cif = cc.cif;
                myobject.import_export = cc.import_export;
                myobject.nombre_pais = cc.pais;
                myobject.region = cc.region;
                myobject.volumen = cc.peso;

                master.aereo_temp.Add(myobject);
                master.SaveChanges();
            }

            return View();
        }

    }
}
