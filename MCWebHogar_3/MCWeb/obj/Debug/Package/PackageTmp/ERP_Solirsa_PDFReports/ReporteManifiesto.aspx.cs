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
    public partial class ReporteManifiesto : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string idManifest = Request.QueryString["idmanifest"].ToString();
                ManifestReport(idManifest);
            }            
        }

        #region Manifiesto        
        private void ManifestReport(string idManifest)
        {
            try
            {
                IDictionaryEnumerator allCaches = HttpRuntime.Cache.GetEnumerator();

                while (allCaches.MoveNext())
                {
                    Cache.Remove(allCaches.Key.ToString());
                }

                string clientSignatureURL = "";
                string employeeSignatureURL = "";

                MCWebHogar.DataSets.DSSolicitud dsReportePedido = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDManifest", idManifest, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "LoadManifestReport", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_Manifest_001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();estilosElementosBloqueados();", true);
                    return;
                }
                dsReportePedido.Tables["DT_ManifestReport_Header"].Merge(Result, true, MissingSchemaAction.Ignore);

                clientSignatureURL = Result.Rows[0]["ClientSignatureURL"].ToString();

                string fileNameSignatureClient = "";
                string fileNameSignatureEmployee = "";

                if (clientSignatureURL != "")
                {
                    string[] stringParts = clientSignatureURL.Split(new char[] { '/' });
                    fileNameSignatureClient = stringParts[stringParts.Length - 1];
                    string directoryName = HttpContext.Current.Server.MapPath("~");
                    string path = Path.Combine(directoryName, @"Assets\img\Reports\Manifest\" + stringParts[5]);
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileNameSignatureClient);

                    using (WebClient webClient = new WebClient())
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        ServicePointManager.DefaultConnectionLimit = 9999;

                        webClient.DownloadFile(clientSignatureURL, path);
                    }
                }
                
                employeeSignatureURL = Result.Rows[0]["EmployeeSignatureURL"].ToString();
                if (employeeSignatureURL != "")
                {
                    string[] stringParts = employeeSignatureURL.Split(new char[] { '/' });
                    fileNameSignatureEmployee = stringParts[stringParts.Length - 1];
                    string directoryName = HttpContext.Current.Server.MapPath("~");
                    string path = Path.Combine(directoryName, @"Assets\img\Reports\Manifest\" + stringParts[5]);
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileNameSignatureEmployee);

                    using (WebClient webClient = new WebClient())
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        ServicePointManager.DefaultConnectionLimit = 9999;

                        webClient.DownloadFile(employeeSignatureURL, path);
                    }
                }

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@ManifestID", idManifest, SqlDbType.Int);
                DT.DT1.Rows.Add("@Msg", "", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CurrentUser", "kpicado", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Sentence", "ResumeManifestProducts", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "usp_PRD_ManifestProduct_001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PedidoID");
                    dt.Columns.Add("DescripcionProducto");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReportePedido.Tables["DT_ManifestReport_Detail"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReportePedido.Tables["DT_ManifestReport_Detail"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptManifest.rdlc", "DT_ManifestReport_Header", "DT_ManifestReport_Header");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptManifest.rdlc", "DT_ManifestReport_Detail", "DT_ManifestReport_Detail");

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
                        }
                    }
                }

                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.EnableHyperlinks = true;

                ReportParameter rp_clientSignatureURL = new ReportParameter("clientSignatureURL", clientSignatureURL != "" ? new Uri(Server.MapPath(@"~/Assets/img/Reports/Manifest/" + fileNameSignatureClient)).AbsoluteUri : "Sin firma");
                ReportViewer1.LocalReport.SetParameters(rp_clientSignatureURL);

                ReportParameter rp_employeeSignatureURL = new ReportParameter("employeeSignatureURL", employeeSignatureURL != "" ? new Uri(Server.MapPath(@"~/Assets/img/Reports/Manifest/" + fileNameSignatureEmployee)).AbsoluteUri : "Sin firma");
                ReportViewer1.LocalReport.SetParameters(rp_employeeSignatureURL);
                
                ReportViewer1.LocalReport.Refresh();                

                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamIds;
                string mimeType = String.Empty;
                string encoding = String.Empty;
                string extension = string.Empty;
                byte[] bytes2 = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                //Generamos archivo en el servidor
                string strCurrentDir2 = Server.MapPath(".") + "\\ReportesTemp\\";
                string strFilePDF2 = "ReportePedido.pdf";
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