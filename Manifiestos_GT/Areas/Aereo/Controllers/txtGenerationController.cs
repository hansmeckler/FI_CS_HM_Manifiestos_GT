using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manifiesto.Data.Admin;
using Manifiesto.Data.Aereo;
using master_aimar.Entities;
using db_aereo.Entities;

namespace Manifiesto.Web.Areas.Aereo.Controllers
{
    [Authorize]
    public class txtGenerationController : Controller
    {

        readonly IAereoService _aereoServices;
        readonly IAdminService _adminServices;

        public txtGenerationController(IAereoService aereoServices, IAdminService adminServices)
        {
            this._aereoServices = aereoServices;
            this._adminServices = adminServices;
        }

        [HttpPost]
        public ActionResult CreateTxtFile(FormCollection collection)
        {
            int id;
            try
            {

                List<string> myobj = new List<string>();

                int mno = Convert.ToInt32(collection[0]);
                int type = Convert.ToInt32(collection[1]);
                int function = Convert.ToInt32(collection[2]);

                //int fix = Convert.ToInt32(collection[3]);
                int fix = 0;

                id = 1;
                string manifest = "";
                string opcode = "";
                string glnsender = "";
                string glnreceiver = "";
                string no_viaje = "";
                string mensaje = "";
                string test = "6";
                string original = "";
                string puerto_origen = "";
                string puerto_desembarque = "";
                long? msumpieces = 0;
                decimal? msumweight = 0;
                decimal? msumvolume = 0;
                int? mcntbl = 0;
                

                int? no_piezas = 0;
                string peso = "0";
                string volumen = "0";
                string no_bl = "";
                int? id_cliente = 0;
                int? id_shipper = 0;
                string commodity = "";
                string cliente = "";
                string direccion = "";
                string shipper = "";
                string nit = "";

                var voyage_info = this._aereoServices.get_cuscar_voyage_info().Where(a => a.cuscar_voyage_id == mno);

                //voyage_info
                puerto_origen = voyage_info.Select(a => a.puerto_origen).FirstOrDefault();
                puerto_desembarque = voyage_info.Select(a => a.puerto_desembarque).FirstOrDefault();
                var operation = voyage_info.Select(a => a.operation).FirstOrDefault();
                var funcsend = voyage_info.Select(a => a.funcsend).FirstOrDefault();
                var company = voyage_info.Select(a => a.countries).FirstOrDefault();

                var bl_info = this._aereoServices.get_cuscar_bl_info().Where(a => a.cuscar_voyage_id == mno);

                //totales
                var totales = this._aereoServices.get_totales(mno).FirstOrDefault();
                msumpieces = totales.no_piezas;
                msumweight = totales.peso;
                msumvolume = totales.volumen;
                mcntbl = totales.count;

                var sequence = this._adminServices.getAaseq();

                string mtrans = String.Format("{0:0000}",(sequence.nextatransmission + 1));
                int? mmsg = (sequence.nextamessage + 1);
                int seqmanifest = Convert.ToInt32(this._adminServices.iif(fix == 0, "1", "0"));
                int? seqm = (sequence.nextamanifest + seqmanifest);
                string mmnfst = seqm.ToString();
                string mmnarc = "";

                if (company.Contains("LTF") == true)
                {
                    mmnarc = "E18F" + mtrans;
                    manifest = "18F" + (DateTime.Now.Year).ToString().Substring(2, 2) + "01" + this._adminServices.pad0(mmnfst, 4);
                    opcode = "18F";
                    glnsender = "7400000001299";
                }
                else
                {
                    mmnarc = "E49E" + mtrans;
                    manifest = "49E" + (DateTime.Now.Year).ToString().Substring(2, 2) + "01" + this._adminServices.pad0(mmnfst, 4);
                    opcode = "49E";
                    glnsender = "7400000000421";
                }

                glnreceiver = "7409000030025";

                no_viaje = voyage_info.Select(a => a.no_viaje).FirstOrDefault();

                myobj.Add("UNB+UNOA:2+" + glnsender + "+" + glnreceiver + "+" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ":" + DateTime.Now.Hour + DateTime.Now.Minute + "+" + this._adminServices.pad0(mtrans.ToString(), 4) + "+" + test);
                myobj.Add("UNH+" + this._adminServices.pad0(mmsg.ToString(), 5) + "+CUSCAR:D:01A:UN");
                myobj.Add("BGM+" + type + "+" + manifest + "+" + function);
                myobj.Add("RFF+" + this._adminServices.iif(type == 785, "AFB", "API") + this._adminServices.iif(original != "", "+" + original, ":" + ""));
                myobj.Add("NAD+" + funcsend + "+" + glnsender + "::9");


                if (function != 1)
                { 

                    if (operation == 23)
                    {
                        myobj.Add("LOC+125+" + puerto_origen.Substring(puerto_origen.Length - 3,3) + "::3");
                        myobj.Add("LOC+179" + "+" + puerto_desembarque.Substring(puerto_desembarque.Length - 3, 3) + "::3");
                    }
                    else if (operation == 22)
                    {
                        myobj.Add("LOC+8+" + puerto_desembarque.Substring(puerto_desembarque.Length - 3, 3) + "::3");
                        myobj.Add("LOC+178+" + puerto_origen.Substring(puerto_origen.Length - 3,3) + "::3");
                    }

                    myobj.Add("GIS+" + operation);
                    myobj.Add("CNT+10:" + mcntbl);
                    myobj.Add("CNT+32:" + msumweight);
                    myobj.Add("CNT+18:" + msumvolume);
                    myobj.Add("CNT+11:" + msumpieces);

                    if (operation == 23)
                        myobj.Add("CNT+21:" + bl_info.Select(a => a.flete).Sum());
                    else
                        myobj.Add("CNT+21:" + mcntbl);

                    int b = -1;
                    foreach (var bl in bl_info)
                    {
                        b++;
                        no_piezas = bl.no_piezas;
                        peso = string.Format("{0:0.00}", bl.peso);
                        volumen = string.Format("{0:0.00}", bl.volumen);
                        no_bl = bl.no_bl;
                        id_cliente = bl.id_cliente;
                        id_shipper = bl.id_shipper;
                        commodity = bl.comodity.Replace(System.Environment.NewLine, "");
                        cliente = bl.cliente;
                        direccion = bl.direccion;
                        shipper = bl.shipper;

                        if (voyage_info.Select(a => a.operation == 23).FirstOrDefault())
                        {
                            if (bl.tipo_docto == 1)
                                nit = "NIT" + bl.codigo_tributario + "+";
                            else if (bl.tipo_docto == 2)
                                nit = "AAO+";
                            else if (bl.tipo_docto == 3)
                                nit = "EEE+";
                            else if (bl.tipo_docto == 4)
                                nit = "NTG+";
                            else
                                return Json(new { ID = 2, status = "Error Nit", title = "No Existe Algun Nit para con el Cliente" }, JsonRequestBehavior.AllowGet);
                        }
                        else 
                        {
                            var cliente1 = _adminServices.getClientes().Where(a => a.id_cliente == bl.id_cliente);
                            if (cliente1.Any(a => a.id_cliente == bl.id_cliente))
                                nit = "NIT+";
                            else
                                nit = "+";
                        }

                        double? flete = 0;
                        var tipo_moneda = "USD";
                        if (operation == 23)
                        {
                            flete = bl.flete;
                            
                            if (bl.ttm_id == 1)
                                tipo_moneda = "GTQ";
                            else if (bl.ttm_id == 2)
                                tipo_moneda = "USD";
                            else
                                tipo_moneda = "EUR";
                            
                            //if (flete == 0) return Json(new { ID = 2, status = "Error", title = "Flete con valor cero para bl " + bl.no_bl }, JsonRequestBehavior.AllowGet);
                        }


                        int blcni = b + 1;
                        myobj.Add("CNI+" + blcni + "+::" + no_viaje);
                        myobj.Add("RFF+HWB:" + no_bl);
                        myobj.Add("CNT+7:" + peso);
                        myobj.Add("CNT+29:" + volumen.ToString());
                        myobj.Add("CNT+11:" + no_piezas.ToString());
                        myobj.Add("MOA+144:" + flete + ":" + tipo_moneda + "");
                        myobj.Add("LOC+35+" + puerto_origen.Substring(0, 2) + "::5");
                        myobj.Add("LOC+36+" + puerto_desembarque.Substring(0, 2) + "::5");
                        myobj.Add("LOC+88+7400000000674::9");
                        myobj.Add("GIS+" + operation + ":::8");
                        
                        if (bl.flete == 0)
                            myobj.Add("CPI+14+NS");
                        else
                            myobj.Add("CPI+14");

                        myobj.Add("CPI+2");
                        myobj.Add("TDT+30");
                        myobj.Add("TSR+10+++10");
                        myobj.Add("NAD+CZ++" + this._adminServices.Break35(shipper));

                        string cliente_break = cliente.Truncate(70);
                        string direccion_break = direccion.Truncate(105);

                        myobj.Add("NAD+CN+" + nit + this._adminServices.Break35(cliente_break) + this._adminServices.Break35(direccion_break));
                        myobj.Add("GID+1+" + no_piezas + ":PK:ZZZ::10");
                        myobj.Add("FTX+AAA+++" + commodity.ToString());
                        myobj.Add("MEA+AAI++KGM:" + peso.ToString());
                        myobj.Add("MEA+WX++KMQ:" + volumen.ToString());
                        //myobj.Add("MOA+125:" + flete + ":" + tipo_moneda + "");
                        myobj.Add("TMD+1");
                    }
                
                }

                myobj.Add("AUT+Cuscar  Ag Carga+AIMAR SA");
                myobj.Add("UNT+" + (myobj.Count() + 2) + "+" + this._adminServices.pad0(mmsg.ToString(), 5));
                myobj.Add("UNZ+1+" + this._adminServices.pad0(mtrans.ToString(), 4));

                using (var context_admin = new master_aimarEntities())
                {
                    string query_seq = "UPDATE aaseq SET nextatransmission = " + mtrans + ", nextamessage = " + mmsg + ", nextamanifest = " + mmnfst;
                    context_admin.Database.ExecuteSqlCommand(query_seq);
                    context_admin.SaveChanges();
                }

                DateTime ndt = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime d2 = DateTime.Now;
                string s = d2.ToString("yyyy/MM/dd");
                TimeSpan t = d2 - ndt;
                int nfile = t.Days + 1;
                string snfile = nfile.ToString();
                string nparttwo = this._adminServices.pad0(snfile, 3);
                string mfile = mmnarc + "." + nparttwo;

                string path = HttpContext.Server.MapPath("~/Files_A/");
                string file_name = path + mfile;
                string tLine = "";

                System.IO.StreamWriter objWriter;
                objWriter = new System.IO.StreamWriter(file_name);

                for (int i = 0; i < myobj.Count(); i++)
                {
                    tLine = tLine + myobj[i] + "\r\n";
                    objWriter.WriteLine(myobj[i] + "'");
                }

                objWriter.Close();


               foreach (var v in voyage_info)
               {
                    if (v.original == "NA")
                    {
                        using (var context = new db_aereoContext())
                        {
                            context.Database.ExecuteSqlCommand("update cuscar_voyage_info set cuscar = '" + mfile  + "', cuscardt = '" + s + "', manifest = '" + manifest + "', original = '" + manifest + "' where cuscar_voyage_id = " + mno);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        using (var context = new db_aereoContext())
                        {
                            context.Database.ExecuteSqlCommand("update cuscar_voyage_info set cuscar = '" + mfile  + "', cuscardt = '" + s + "', manifest = '" + manifest + "' where cuscar_voyage_id = " + mno);
                            context.SaveChanges();
                        }
                    }
               }
                mensaje = "File Generated";
                return Json(new { ID = id, file = "", manifiesto = manifest, status = mensaje, title = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                id = 2;
                return Json(new { ID = id, status = "Exception caught." + e, title = "Please try again or Contact with the Administrator." }, JsonRequestBehavior.AllowGet);
            }
        }

    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
