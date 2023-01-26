using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetalleODP : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("../Default.aspx", true);
                }
                else
                {
                    if (Session["IDODP"] == null)
                    {
                        Response.Redirect("OrdenesProduccion.aspx", true);
                    }
                    else
                    {
                        HDF_IDODP.Value = Session["IDODP"].ToString();
                        HDF_IDUsuario.Value = Session["Usuario"].ToString();
                        cargarDDLs();
                        cargarODP("");
                        cargarCategoriasODP();
                        cargarProductosODP();
                        cargarPedidosODP("");
                        ViewState["Ordenamiento"] = "ASC";
                        ViewState["OrdenamientoCategorias"] = "ASC";                        
                    }                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("TXT_Cantidad"))
                {
                    int index = Convert.ToInt32(opcion.Split('$')[3].Replace("ctl", "")) - 2;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_Cantidad_OnTextChanged", "enterCantidad(" + index + ");", true);
                }
                if (opcion.Contains("DDL_ImpresorasLoad"))
                { 
                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("Text");
                    dt.Columns.Add("Value");
                    
                    string printer = Session["Printer"].ToString().Trim();
                    string[] nombresImpresoras = argument.Split(',');

                    dt.Rows.Add("Seleccione", "Seleccione");

                    foreach(string impresora in nombresImpresoras) {
                        dt.Rows.Add(impresora, impresora);
                    }

                    DDL_Impresoras.DataSource = dt;
                    DDL_Impresoras.DataTextField = "Text";
                    DDL_Impresoras.DataValueField = "Value";
                    DDL_Impresoras.DataBind();
                    UpdatePanel_SeleccionarImpresora.Update();

                    DDL_Impresoras.SelectedValue = printer;

                    string script = "estilosElementosBloqueados();abrirModalSeleccionarImpresora();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Impresoras", script, true);
                }
                if (opcion.Contains("Identificacion"))
                {
                    string identificacion = opcion.Split(';')[1];
                    Session["IdentificacionReceptor"] = identificacion;
                    Response.Redirect("../GestionProveedores/Proveedores.aspx", true);
                }
                if (opcion.Contains("Receta"))
                {
                    string negocio = opcion.Split(';')[1];
                    Session["RecetaNegocio"] = negocio;
                    Response.Redirect("../GestionCostos/CrearReceta.aspx", true);
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        
        private void cargarDDLs()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarUsuarios", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_Propietario.DataSource = Result;
                DDL_Propietario.DataTextField = "Nombre";
                DDL_Propietario.DataValueField = "IDUsuario";
                DDL_Propietario.DataBind();
            }

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPlantasProduccion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_PlantaProduccion.DataSource = Result;
                DDL_PlantaProduccion.DataTextField = "DescripcionPlantaProduccion";
                DDL_PlantaProduccion.DataValueField = "IDPlantaProduccion";
                DDL_PlantaProduccion.DataBind();
            }
        }

        protected void DDL_Impresoras_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string printer = DDL_Impresoras.SelectedValue;
            Session["Printer"] = printer;
            string script = "estilosElementosBloqueados();cerrarModalSeleccionarImpresora();alertifysuccess('Se ha seleccionado la impresora: " + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Impresoras_OnSelectedIndexChanged", script, true);
        }
        #endregion

        #region Orden de Producción
        private void cargarODP(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarODPs", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");
            
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    foreach (DataRow dr in Result.Rows)
                    {
                        TXT_CodigoODP.Text = dr["NumeroODP"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_CostoODP.Text = String.Format("{0:n}", dr["CostoODP"]);;
                        TXT_EstadoODP.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaODP.Text = dr["FODP"].ToString().Trim();
                        TXT_HoraODP.Text = dr["HODP"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();
                        DDL_PlantaProduccion.SelectedValue = dr["PlantaProduccionID"].ToString().Trim();

                        HDF_EstadoODP.Value = dr["Estado"].ToString().Trim();
                        HDF_IDsPedidos.Value = dr["IdsPedidos"].ToString().Trim();

                        BTN_ConfirmarODP.Visible = HDF_EstadoODP.Value == "Solicitada";
                        // BTN_ImprimirOrdenProduccion.Visible = HDF_EstadoODP.Value == "Solicitada";
                        BTN_CompletarODP.Visible = HDF_EstadoODP.Value == "Confirmada";

                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();
                        string script = "estilosElementosBloqueados();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedido", script, true);
                    }
                }
            }
        }

        private void generarDespacho()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDsPedidos", HDF_IDsPedidos.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearDespacho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDespacho_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string IDDespacho = Result.Rows[0][1].ToString().Trim();
                    generarRecibidoPedido(IDDespacho);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDespacho_Click", "alertifywarning('No se ha podido crear la orden de producción, por favor intente de nuevo.');", true);
                return;
            }
        }

        private void generarRecibidoPedido(string idDespacho)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDespacho", idDespacho, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@UsuarioID", Session["UserID"].ToString(), SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearRecibidoPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");
        }

        protected void BTN_ConfirmarODP_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmada", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarEstadoODP", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarODP_Click", "alertifywarning('No se ha confirmado la orden de producción. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado la orden de producción.');";
                    generarDespacho();
                    cargarODP(script);
                    cargarProductosODP();                    
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado la orden de producción.');";
                cargarODP(script);
            }
        }

        protected void BTN_CompletarODP_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Completado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarEstadoODP", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarODP_Click", "alertifywarning('No se ha confirmado la orden de producción. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha completado la orden de producción.');";
                    cargarODP(script);
                    cargarProductosODP();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado la orden de producción.');";
                cargarODP(script);
            }
        }

        protected void DDL_Reportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(DDL_Reportes.SelectedValue))
            {
                case 1:
                    ReporteOrdenProduccion();
                    break;
                case 2:
                    DescargarOrdenProduccion();
                    break;
                default:
                    break;
            }
            DDL_Reportes.SelectedValue = "0";
            UpdatePanel_Header.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Reportes_SelectedIndexChanged", "desactivarloading();estilosElementosBloqueados();cargarFiltros();", true);
        }

        private void ReporteOrdenProduccion()
        {
            try
            {
                MCWebHogar.DataSets.DSSolicitud dsReporteODP = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarODPs", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();estilosElementosBloqueados();", true);
                    return;
                }
                dsReporteODP.Tables["DT_EncabezadoODP"].Merge(Result, true, MissingSchemaAction.Ignore);

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosReporte", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ODPID");
                    dt.Columns.Add("DescripcionProducto");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReporteODP.Tables["DT_DetalleODP"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporteODP.Tables["DT_DetalleODP"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptODP.rdlc", "DT_EncabezadoODP", "DT_EncabezadoODP");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptODP.rdlc", "DT_DetalleODP", "DT_DetalleODP");

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
                    foreach (DataTable dt in dsReporteODP.Tables)
                    {
                        if (dt.Rows.Count > 0 && dt.TableName.Trim() == report)
                        {
                            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(dr["DataSet"].ToString().Trim(), (DataTable)dt));
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
                string strFilePDF2 = "ReporteODP.pdf";
                string strFilePathPDF2 = strCurrentDir2 + strFilePDF2;
                using (FileStream fs = new FileStream(strFilePathPDF2, FileMode.Create))
                {
                    fs.Write(bytes2, 0, bytes2.Length);
                }
                string direccion = "/ControlPedidos/ReportesTemp/" + strFilePDF2;
                string _open = "window.open('" + direccion + "'  , '_blank');desactivarloading();estilosElementosBloqueados();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", _open, true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DescargarOrdenProduccion()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ReporteODP", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarODP_Click", "desactivarloading();estilosElementosBloqueados();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "OrdenProduccion");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=OrdenProduccion.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        #endregion

        #region Cargar Productos
        #region Productos Pedido
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosODP();
        }

        public void cargarCategoriasODP()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaCategorias.DataSource = Result;
                    DGV_ListaCategorias.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductosODP.DataSource = Result;
                DGV_ListaProductosODP.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
        }
        
        public void cargarProductosODP()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosODP.DataSource = Result;
                    DGV_ListaProductosODP.DataBind();
                    UpdatePanel_ListaProductosODP.Update();
                }
            }
            else
            {
                DGV_ListaProductosODP.DataSource = Result;
                DGV_ListaProductosODP.DataBind();
                UpdatePanel_ListaProductosODP.Update();
            }
        }

        protected void DGV_ListaCategorias_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            if (ViewState["OrdenamientoCategorias"].ToString().Trim() == "ASC")
            {
                ViewState["OrdenamientoCategorias"] = "DESC";
            }
            else
            {
                ViewState["OrdenamientoCategorias"] = "ASC";
            }

            Result.DefaultView.Sort = e.SortExpression + " " + ViewState["OrdenamientoCategorias"].ToString().Trim();
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaCategorias.DataSource = Result;
                    DGV_ListaCategorias.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaCategorias.DataSource = Result;
                DGV_ListaCategorias.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
        }

        protected void DGV_ListaCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idCategoria = Convert.ToInt32(DGV_ListaCategorias.DataKeys[e.Row.RowIndex].Value.ToString());
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CategoriaID", idCategoria, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosCategoria", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");
                GridView DGV_ListaProductos = e.Row.FindControl("DGV_ListaProductos") as GridView;
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
            }
        }

        protected void DGV_ListaCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "imprimir")
                {
                    string categoria = DGV_ListaCategorias.DataKeys[index].Values[1].ToString().Trim();
                    string printer = TXT_NombreImpresora.Text.Trim();
                    string script = "estilosElementosBloqueados();imprimir('" + categoria + "', '" + TXT_CodigoODP.Text + "', " + index + ", '" + printer + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaCategorias_RowCommand", script, true);
                }
            }
        }

        protected void DGV_ListaProductosODP_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            if (ViewState["Ordenamiento"].ToString().Trim() == "ASC")
            {
                ViewState["Ordenamiento"] = "DESC";
            }
            else
            {
                ViewState["Ordenamiento"] = "ASC";
            }

            Result.DefaultView.Sort = e.SortExpression + " " + ViewState["Ordenamiento"].ToString().Trim();
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosODP.DataSource = Result;
                    DGV_ListaProductosODP.DataBind();
                    UpdatePanel_ListaProductosODP.Update();
                }
            }
            else
            {
                DGV_ListaProductosODP.DataSource = Result;
                DGV_ListaProductosODP.DataBind();
                UpdatePanel_ListaProductosODP.Update();
            }
        }

        protected void DGV_ListaProductosODP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox cantidad = (TextBox)e.Row.FindControl("TXT_Cantidad");
                DropDownList ddlUnds = (DropDownList)e.Row.FindControl("DDL_Unidades");
                DropDownList ddlDecs = (DropDownList)e.Row.FindControl("DDL_Decenas");
                DropDownList ddlCents = (DropDownList)e.Row.FindControl("DDL_Centenas");
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                int unds = Convert.ToInt32(cantidadProducto) % 10;
                int decs = (Convert.ToInt32(cantidadProducto) / 10) % 10;
                int cents = Convert.ToInt32(cantidadProducto) / 100;
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                ddlCents.SelectedValue = cents.ToString();

                if (HDF_EstadoODP.Value != "Confirmada" && (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0))
                {
                    cantidad.Enabled = false;
                    cantidad.CssClass = "form-control";
                    ddlUnds.Enabled = false;
                    ddlUnds.CssClass = "form-control";
                    ddlDecs.Enabled = false;
                    ddlDecs.CssClass = "form-control";
                    ddlCents.Enabled = false;
                    ddlCents.CssClass = "form-control";
                }
            }
        }
        
        protected void DGV_ListaProductosODP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosODP.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                DropDownList ddlUnds = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                DropDownList ddlDecs = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Decenas") as DropDownList;
                bool modifico = false;
                if (e.CommandName == "minus")
                {
                    if (cantidadProducto > 0)
                    {
                        modifico = true;
                        cantidadProducto--;
                    }
                }
                if (e.CommandName == "plus")
                {
                    if (cantidadProducto < 99)
                    {
                        modifico = true;
                        cantidadProducto++;
                    }
                }
                int unds = Convert.ToInt32(cantidadProducto) % 10;
                int decs = Convert.ToInt32(cantidadProducto) / 10;
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                if (modifico)
                    guardarProductoODP(index);
                
            }
        }
                                                                                       
        protected void DDL_DecenasUnidades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            DropDownList ddlUnds = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            DropDownList ddlCents = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Centenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosODP.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            int cents = Convert.ToInt32(ddlCents.SelectedValue) * 100;
            decimal cantidadProducto = cents + decs + unds;
            cantidad.Text = cantidadProducto.ToString();
            guardarProductoODP(index);
        }

        private void guardarProductoODP(int index)
        {
            TextBox cantidad = DGV_ListaProductosODP.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDODPDetalle = DGV_ListaProductosODP.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDODPDetalle", IDODPDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadProducida", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosODP();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarODP("");
                }
            }
        }

        [WebMethod()]
        public static string guardarProductoODP(int idODPDetalle, int cantidadProducto, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDODPDetalle", idODPDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadProducida", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP08_0001");

            return "correct";
        }
        
        protected void BTN_ImprimirOrdenProduccion_Click(object sender, EventArgs e)
        {
            string printer = Session["Printer"].ToString().Trim();
            TXT_NombreImpresora.Text = printer;
            UpdatePanel_ListaProductos.Update();
            string script = "abrirModalOrdenProduccion();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirOrdenProduccion_Click", script, true);
        }
        #endregion
        #endregion

        #region Pedidos
        protected DataTable consultaCargarPedidosODP(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDODP", HDF_IDODP.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");
        }

        private void cargarPedidosODP(string ejecutar)
        {
            Result = consultaCargarPedidosODP("CargarPedidosODP");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaPedidosODP.DataSource = Result;
                    DGV_ListaPedidosODP.DataBind();
                    UpdatePanel_DGVPedidosODP.Update();
                }
            }
            else
            {
                DGV_ListaPedidosODP.DataSource = Result;
                DGV_ListaPedidosODP.DataBind();
                UpdatePanel_DGVPedidosODP.Update();
            }
            string script = "estilosElementosBloqueados();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedidosODP", script, true);
        }

        protected void DGV_ListaPedidosODP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idPedido = Convert.ToInt32(DGV_ListaPedidosODP.DataKeys[e.Row.RowIndex].Values[1].ToString());
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");
                GridView DGV_ListaProductos = e.Row.FindControl("DGV_ListaProductos") as GridView;
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
            }
        }

        protected void BTN_VerDetallePedidos_Click(object sender, EventArgs e)
        {
            string script = "abrirModalPedidosODP();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_VerDetallePedidos_Click", script, true);
        }
        #endregion
    }
}