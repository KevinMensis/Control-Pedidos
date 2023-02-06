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
    public partial class ReporteControlPesaje : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string idInbounOrder = Request.QueryString["idinboundorder"].ToString();
                InbounOrderReport(idInbounOrder);
            }            
        }

        #region Control de Pesaje
        private void InbounOrderReport(string idInbounOrder)
        {
            try
            {
                IDictionaryEnumerator allCaches = HttpRuntime.Cache.GetEnumerator();

                while (allCaches.MoveNext())
                {
                    Cache.Remove(allCaches.Key.ToString());
                }

                string inboundOrderIdentifier = "";

                MCWebHogar.DataSets.DSSolicitud dsReportePedido = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDInboundOrder", idInbounOrder, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadInboundOrderInfo", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_InboundOrder_001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('No hay datos para mostrar');", true);
                    return;
                }
                dsReportePedido.Tables["DT_InboundOrderReport_Header"].Merge(Result, true, MissingSchemaAction.Ignore);

                inboundOrderIdentifier = Result.Rows[0]["InboundOrderIdentifier"].ToString();

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@InboundOrderID", idInbounOrder, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "ResumeInboundOrderProducts", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_InboundOrderProduct_001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("IDInbounOrderProduct");
                    dt.Columns.Add("DetailCategory");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReportePedido.Tables["DT_InboundOrderReport_Detail"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReportePedido.Tables["DT_InboundOrderReport_Detail"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptInboundOrder.rdlc", "DT_InboundOrderReport_Header", "DT_InboundOrderReport_Header");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptInboundOrder.rdlc", "DT_InboundOrderReport_Detail", "DT_InboundOrderReport_Detail");

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
                    foreach (DataTable dt in dsReportePedido.Tables)
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

                ReportViewer1.LocalReport.Refresh();                

                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamIds;
                string mimeType = String.Empty;
                string encoding = String.Empty;
                string extension = string.Empty;
                byte[] bytes2 = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                //Generamos archivo en el servidor
                string strCurrentDir2 = Server.MapPath(".") + "\\ReportesTemp\\";
                string strFilePDF2 = "ReporteControlPesaje_" + inboundOrderIdentifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
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