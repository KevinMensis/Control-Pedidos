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
    public partial class DetalleEmpaque : System.Web.UI.Page
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
                    if (Session["IDEmpaque"] == null)
                    {
                        Response.Redirect("Empaque.aspx", true);
                    }
                    else
                    {
                        HDF_IDEmpaque.Value = Session["IDEmpaque"].ToString();
                        HDF_IDUsuario.Value = Session["Usuario"].ToString();
                        HDF_UsuarioID.Value = Session["UserId"].ToString();
                        cargarDDLs();
                        cargarEmpaque("");
                        cargarProductosEmpaque();
                        cargarProductosSinAsignar("");
                        ViewState["Ordenamiento"] = "ASC";
                    }                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("CargarEmpaque"))
                {
                    cargarEmpaque("");
                    cargarProductosEmpaque();
                    cargarProductosSinAsignar("");
                }
                if (opcion.Contains("TXT_BuscarProductosSinAsignar") || opcion.Contains("BTN_Agregar"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_Buscar_OnTextChanged", "cargarFiltros();estilosElementosBloqueados();", true);
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

                    foreach (string impresora in nombresImpresoras)
                    {
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
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_Categoria_001");

            LB_Categoria.DataSource = Result;
            LB_Categoria.DataTextField = "DescripcionCategoria";
            LB_Categoria.DataValueField = "IDCategoria";
            LB_Categoria.DataBind();
        }
        protected void DDL_Impresoras_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string printer = DDL_Impresoras.SelectedValue;
            Session["Printer"] = printer;
            string script = "estilosElementosBloqueados();cerrarModalSeleccionarImpresora();alertifysuccess('Se ha seleccionado la impresora: " + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Impresoras_OnSelectedIndexChanged", script, true);
        }
        #endregion

        #region Empaque
        public void cargarEmpaque(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDEmpaque", HDF_IDEmpaque.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmpaques", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");
            
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
                        TXT_CodigoEmpaque.Text = dr["NumeroEmpaque"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_MontoEmpaque.Text = String.Format("{0:n}", dr["MontoEmpaque"]);
                        TXT_FechaEmpaque.Text = dr["FEmpaque"].ToString().Trim();
                        TXT_HoraEmpaque.Text = dr["HEmpaque"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();

                        BTN_AgregarProducto.Visible = TXT_FechaEmpaque.Text == DateTime.Now.ToString("yyyy-MM-dd");

                        TXT_FechaEmpaque.Enabled = (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0);
                        // HDF_EstadoEmpaque.Value = dr["Estado"].ToString().Trim();

                        // BTN_ConfirmarEmpaque.Visible = HDF_EstadoEmpaque.Value != "Confirmado";

                        if (Session["NuevoEmpaque"] != null)
                        {
                            ejecutar += "abrirModalAgregarProductos();";
                            Session.Remove("NuevoEmpaque");
                        }

                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();

                        string script = "estilosElementosBloqueados();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarEmpaque", script, true);
                    }
                }
            }
        }

        protected void BTN_ConfirmarEmpaque_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDEmpaque", HDF_IDEmpaque.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarEmpaque_Click", "alertifywarning('No se ha confirmado el Empaque. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el Empaque.');";
                    cargarEmpaque(script);
                    cargarProductosEmpaque();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el Empaque.');";
                cargarEmpaque(script);
            }
        }

        protected void BTN_ReporteEmpaque_Click(object sender, EventArgs e)
        {
            try
            {
                MCWebHogar.DataSets.DSSolicitud dsReporteEmpaque = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDEmpaque", HDF_IDEmpaque.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarEmpaques", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();estilosElementosBloqueados();", true);
                    return;
                }
                dsReporteEmpaque.Tables["DT_EncabezadoEmpaque"].Merge(Result, true, MissingSchemaAction.Ignore);

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@EmpaqueID", HDF_IDEmpaque.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarReporteProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("EmpaqueID");
                    dt.Columns.Add("DescripcionProducto");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReporteEmpaque.Tables["DT_DetalleEmpaque"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporteEmpaque.Tables["DT_DetalleEmpaque"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptEmpaque.rdlc", "DT_EncabezadoEmpaque", "DT_EncabezadoEmpaque");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptEmpaque.rdlc", "DT_DetalleEmpaque", "DT_DetalleEmpaque");

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
                    foreach (DataTable dt in dsReporteEmpaque.Tables)
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
                string strFilePDF2 = "ReporteEmpaque.pdf";
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

        protected void BTN_DescargarEmpaque_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDEmpaque", HDF_IDEmpaque.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ReporteEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarEmpaque_Click", "desactivarloading();estilosElementosBloqueados();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Empaque");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Empaque.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void TXT_FechaEmpaque_OnChange(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDEmpaque", HDF_IDEmpaque.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@fechaEmpaque", TXT_FechaEmpaque.Text + " " + TXT_HoraEmpaque.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarFechaEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_FechaEmpaque_OnChange", "alertifywarning('No se ha modificado la fehca del Empaque. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha modificado la fecha del Empaque.');estilosElementosBloqueados();";
                    cargarEmpaque(script);
                    cargarProductosEmpaque();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha modificado la fecha del Empaque. Por favor, intente de nuevo.');";
                cargarEmpaque(script);
            }
        }
        #endregion

        #region Cargar Productos
        #region Asignar
        protected void BTN_CargarProductos_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DGV_ListaProductosSinAgregar.Rows.Count; i++)
            {
                int index = i;
                int idProducto = Convert.ToInt32(DGV_ListaProductosSinAgregar.DataKeys[index].Value.ToString().Trim());
                TextBox TXT_CantidadAgregar = (TextBox)DGV_ListaProductosSinAgregar.Rows[index].FindControl("TXT_CantidadAgregar");
                CheckBox CHK_Producto = (CheckBox)DGV_ListaProductosSinAgregar.Rows[index].FindControl("CHK_Producto");
                CHK_Producto.Checked = false;
                TXT_CantidadAgregar.Text = "0";
            }

            UpdatePanel_ListaProductosSinAgregar.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptAgregarProductos", "abrirModalAgregarProductos();estilosElementosBloqueados();", true);
            return;
        }

        public void cargarProductosSinAsignar(string ejecutar)
        {
            DT.DT1.Clear();

            string categorias = "";

            #region Categorias
            foreach (ListItem l in LB_Categoria.Items)
            {
                if (l.Selected)
                {
                    categorias += "'" + l.Value + "',";
                }
            }
            categorias = categorias.TrimEnd(',');
            if (categorias != "")
            {
                DT.DT1.Rows.Add("@FiltrarCategoria", 1, SqlDbType.Int);
            }
            #endregion

            DT.DT1.Rows.Add("@EmpaqueID", HDF_IDEmpaque.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@DescripcionProducto", TXT_BuscarProductosSinAsignar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosSinAgregar.DataSource = Result;
                    DGV_ListaProductosSinAgregar.DataBind();
                    UpdatePanel_ListaProductosSinAgregar.Update();
                    UpdatePanel_ModalAgregarProductos.Update();
                    string script = "cargarFiltros();" + ejecutar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
                }
            }
            else
            {
                DGV_ListaProductosSinAgregar.DataSource = Result;
                DGV_ListaProductosSinAgregar.DataBind();
                UpdatePanel_ListaProductosSinAgregar.Update();
                UpdatePanel_ModalAgregarProductos.Update();
                string script = "cargarFiltros();" + ejecutar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
            }
        }

        protected void FiltrarProductos_OnClick(object sender, EventArgs e)
        {
            cargarProductosSinAsignar("productosMarcados();");
        }

        protected void DGV_ListaProductosSinAsignar_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@EmpaqueID", HDF_IDEmpaque.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");
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
                    DGV_ListaProductosSinAgregar.DataSource = Result;
                    DGV_ListaProductosSinAgregar.DataBind();
                    UpdatePanel_ListaProductosSinAgregar.Update();
                }
            }
            else
            {
                DGV_ListaProductosSinAgregar.DataSource = Result;
                DGV_ListaProductosSinAgregar.DataBind();
                UpdatePanel_ListaProductosSinAgregar.Update();
            }
        }

        protected void DGV_ListaProductosSinAsignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
            }
        }

        [WebMethod()]
        public static string BTN_Agregar_Click(string idEmpaque, int idProducto, int idUsuario, int cantidadProducto, string usuario)
        {        
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@EmpaqueID", idEmpaque, SqlDbType.Int);
            DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@UsuarioID", idUsuario, SqlDbType.Int);
            DT.DT1.Rows.Add("@CantidadEmpaque", cantidadProducto, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AgregarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");

            return "correcto";
        }
        #endregion

        #region Productos Empaque
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosEmpaque();
        }
        
        public void cargarProductosEmpaque()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@EmpaqueID", HDF_IDEmpaque.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosEmpaque.DataSource = Result;
                    DGV_ListaProductosEmpaque.DataBind();
                    UpdatePanel_ListaProductosEmpaque.Update();
                    DGV_DetalleEmpaque.DataSource = Result;
                    DGV_DetalleEmpaque.DataBind();
                    UpdatePanel_DetalleEmpaque.Update();
                    string script = "estilosElementosBloqueados();cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosEmpaque", script, true);
                }
            }
            else
            {
                DGV_ListaProductosEmpaque.DataSource = Result;
                DGV_ListaProductosEmpaque.DataBind();
                UpdatePanel_ListaProductosEmpaque.Update();
                DGV_DetalleEmpaque.DataSource = Result;
                DGV_DetalleEmpaque.DataBind();
                UpdatePanel_DetalleEmpaque.Update();
                string script = "estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosEmpaque", script, true);
            }
        }

        protected void DGV_ListaProductosEmpaque_RowDataBound(object sender, GridViewRowEventArgs e)
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
                int cents = (Convert.ToInt32(cantidadProducto) / 100);
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                ddlCents.SelectedValue = cents.ToString();

                if (TXT_FechaEmpaque.Text != DateTime.Now.ToString("yyyy-MM-dd") && (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0))
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
        
        protected void DGV_ListaProductosEmpaque_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosEmpaque.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                DropDownList ddlUnds = DGV_ListaProductosEmpaque.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                DropDownList ddlDecs = DGV_ListaProductosEmpaque.Rows[index].FindControl("DDL_Decenas") as DropDownList;
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
                    guardarProductoEmpaque(index);
                
            }
        }

        protected void DGV_ListaProductosEmpaque_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@EmpaqueID", HDF_IDEmpaque.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");

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
                    DGV_ListaProductosEmpaque.DataSource = Result;
                    DGV_ListaProductosEmpaque.DataBind();
                    UpdatePanel_ListaProductosEmpaque.Update();
                }
            }
            else
            {
                DGV_ListaProductosEmpaque.DataSource = Result;
                DGV_ListaProductosEmpaque.DataBind();
                UpdatePanel_ListaProductosEmpaque.Update();
            }
        }        
        
        protected void DDL_DecenasUnidades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            DropDownList ddlUnds = DGV_ListaProductosEmpaque.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosEmpaque.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            DropDownList ddlCents = DGV_ListaProductosEmpaque.Rows[index].FindControl("DDL_Centenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosEmpaque.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            int cents = Convert.ToInt32(ddlCents.SelectedValue) * 100;
            decimal cantidadProducto = cents + decs + unds;
            cantidad.Text = cantidadProducto.ToString();
            guardarProductoEmpaque(index);
        }

        private void guardarProductoEmpaque(int index)
        {
            TextBox cantidad = DGV_ListaProductosEmpaque.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDEmpaqueDetalle = DGV_ListaProductosEmpaque.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDEmpaqueDetalle", IDEmpaqueDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadEmpaque", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosEmpaque();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarEmpaque("");
                }
            }
        }
        
        [WebMethod()]
        public static string guardarCantidadProductoEmpaque(int idEmpaqueDetalle, int cantidadProducto, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDEmpaqueDetalle", idEmpaqueDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadEmpaque", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP15_0001");

            return "correct";
        }

        protected void BTN_AbrirModalDetalleEmpaque_Click(object sender, EventArgs e)
        {
            cargarProductosEmpaque();
            string printer = Session["Printer"].ToString().Trim();
            TXT_NombreImpresora.Text = printer;
            UpdatePanel_DetalleEmpaque.Update();
            string script = "abrirModalDetalleEmpaque();estilosElementosBloqueados();cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AbrirModalDetalleEmpaque_Click", script, true);
        }

        protected void BTN_ImprimirDetalleEmpaque_Click(object sender, EventArgs e)
        {
            string fechaEmpaque = TXT_FechaEmpaque.Text;
            decimal monto = Convert.ToDecimal(TXT_MontoEmpaque.Text);
            string montoEmpaque = string.Format("{0:n0}", monto);
            string printer = TXT_NombreImpresora.Text.Trim();
            string script = "estilosElementosBloqueados();imprimir('" + montoEmpaque + "', '" + fechaEmpaque + "', '" + TXT_CodigoEmpaque.Text + "', '" + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirDetalleEmapque_Click", script, true);
        }
        #endregion
        #endregion
    }
}