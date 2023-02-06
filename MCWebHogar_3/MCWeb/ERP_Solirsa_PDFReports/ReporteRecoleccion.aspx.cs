using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Microsoft.Reporting.WebForms;
using System.Net;

namespace MCWebHogar.ERP_Solirsa_PDFReports
{
    public partial class ReporteRecoleccion : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string idPickup = Request.QueryString["idpickup"].ToString();
                PickupReport(idPickup);
            }            
        }

        #region Recolección
        private void PickupReport(string idPickup)
        {
            try
            {
                IDictionaryEnumerator allCaches = HttpRuntime.Cache.GetEnumerator();

                while (allCaches.MoveNext())
                {
                    Cache.Remove(allCaches.Key.ToString());
                }

                string pickupIdentifier = "";

                MCWebHogar.DataSets.DSSolicitud dsReporte = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDPickup", idPickup, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadPickupInfo", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_Pickup_001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('No hay datos para mostrar');", true);
                    return;
                }
                dsReporte.Tables["DT_PickupReport_Header"].Merge(Result, true, MissingSchemaAction.Ignore);

                pickupIdentifier = Result.Rows[0]["PickupIdentifier"].ToString();

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@PickupID", idPickup, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadPickupEmployees", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_PickupEmployee_001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PickupID");
                    dt.Columns.Add("UserName");
                    dt.Rows.Add(idPickup, "Sin colaboradores adicionales.");
                    dsReporte.Tables["DT_PickupReport_Employees"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporte.Tables["DT_PickupReport_Employees"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@PickupID", idPickup, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadPickupNotes", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_PickupNotes_001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PickupID");
                    dt.Columns.Add("Detail");
                    dt.Rows.Add(idPickup, "Sin notas adicionales.");
                    dsReporte.Tables["DT_PickupReport_Notes"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporte.Tables["DT_PickupReport_Notes"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@PickupID", idPickup, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadPickupSupplier", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_PickupSupplier_001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PickupID");
                    dt.Columns.Add("SupplierName");
                    dt.Rows.Add(idPickup, "No hay registros.");
                    dsReporte.Tables["DT_PickupReport_Supplier"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporte.Tables["DT_PickupReport_Supplier"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPickup.rdlc", "DT_PickupReport_Header", "DT_PickupReport_Header");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPickup.rdlc", "DT_PickupReport_Employees", "DT_PickupReport_Employees");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPickup.rdlc", "DT_PickupReport_Notes", "DT_PickupReport_Notes");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPickup.rdlc", "DT_PickupReport_Supplier", "DT_PickupReport_Supplier");

                Microsoft.Reporting.WebForms.ReportViewer ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();

                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.DataSources.Clear();

                string report = "";
                foreach (DataRow dr in DT_Encabezado.Rows)
                {
                    FileStream fsReporte = null;
                    string nombre = dr["rpt"].ToString().Trim().Replace(".rdlc", "").Replace("MCWebHogar.", "");
                    ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                    fsReporte = new FileStream(Server.MapPath(@"..\" + nombre + ".rdlc"), FileMode.Open, FileAccess.Read);

                    ReportViewer1.LocalReport.LoadReportDefinition(fsReporte);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(String.Format("{0}.rdlc", @"..\" + nombre));

                    report = dr["DTName"].ToString().Trim();
                    foreach (DataTable dt in dsReporte.Tables)
                    {
                        if (dt.Rows.Count > 0 && dt.TableName.Trim() == report)
                        {
                            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(dr["DataSet"].ToString().Trim(), (DataTable)dt));
                            break;
                        }
                    }
                }

                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.EnableHyperlinks = true;
                
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamIds;
                string mimeType = String.Empty;
                string encoding = String.Empty;
                string extension = string.Empty;
                byte[] bytes2 = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                //Generamos archivo en el servidor
                string strCurrentDir2 = Server.MapPath(".") + "\\ReportesTemp\\";
                string strFilePDF2 = "ReporteRecoleccion_" + pickupIdentifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string strFilePathPDF2 = strCurrentDir2 + strFilePDF2;
                using (FileStream fs = new FileStream(strFilePathPDF2, FileMode.Create))
                {
                    fs.Write(bytes2, 0, bytes2.Length);
                }
                string direccion = "/ERP_Solirsa_PDFReports/ReportesTemp/" + strFilePDF2;
                string _open = "window.location.href = '" + direccion + "'";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", _open, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert(" + ex.ToString() +");", true);
                throw;
            }            
        }       
        #endregion
    }
}