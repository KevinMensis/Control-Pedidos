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
    public partial class DetalleInsumo : System.Web.UI.Page
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
                    if (Session["IDInsumo"] == null)
                    {
                        Response.Redirect("Insumo.aspx", true);
                    }
                    else
                    {
                        HDF_IDInsumo.Value = Session["IDInsumo"].ToString();
                        HDF_IDUsuario.Value = Session["Usuario"].ToString();
                        HDF_UsuarioID.Value = Session["UserId"].ToString();
                        cargarDDLs();
                        cargarInsumo("");
                        cargarProductosInsumo();
                        cargarProductosSinAsignar("");
                        ViewState["Ordenamiento"] = "ASC";
                    }                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("CargarInsumo"))
                {
                    cargarInsumo("");
                    cargarProductosInsumo();
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
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVentaSelect", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_PuntoVenta.DataSource = Result;
                DDL_PuntoVenta.DataTextField = "DescripcionPuntoVenta";
                DDL_PuntoVenta.DataValueField = "IDPuntoVenta";
                DDL_PuntoVenta.DataBind();
            }

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVentaInsumo", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DataView dv = new DataView(Result);
                dv.RowFilter = "IDPuntoVenta <> 0";
                LB_PuntoVenta.DataSource = dv;
                LB_PuntoVenta.DataTextField = "DescripcionPuntoVenta";
                LB_PuntoVenta.DataValueField = "IDPuntoVenta";
                LB_PuntoVenta.DataBind();
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

        #region Insumo
        public void cargarInsumo(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarInsumos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");
            
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
                        TXT_CodigoInsumo.Text = dr["NumeroInsumo"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_MontoInsumo.Text = String.Format("{0:n}", dr["MontoInsumo"]);
                        TXT_FechaInsumo.Text = dr["FInsumo"].ToString().Trim();
                        TXT_HoraInsumo.Text = dr["HInsumo"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();

                        BTN_AgregarProducto.Visible = TXT_FechaInsumo.Text == DateTime.Now.ToString("yyyy-MM-dd") || (ClasePermiso.Permiso("Agregar", "Acciones", "Agregar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0);

                        TXT_FechaInsumo.Enabled = (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0);

                        // HDF_EstadoInsumo.Value = dr["Estado"].ToString().Trim();

                        // BTN_ConfirmarInsumo.Visible = HDF_EstadoInsumo.Value != "Confirmado";

                        if (Session["NuevoInsumo"] != null)
                        {
                            ejecutar += "abrirModalAgregarProductos();";
                            Session.Remove("NuevoInsumo");
                        }

                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();

                        string script = "estilosElementosBloqueados();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarInsumo", script, true);
                    }
                }
            }
        }

        protected void BTN_ConfirmarInsumo_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarInsumo", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarInsumo_Click", "alertifywarning('No se ha confirmado el Insumo. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el Insumo.');";
                    cargarInsumo(script);
                    cargarProductosInsumo();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el Insumo.');";
                cargarInsumo(script);
            }
        }

        protected void BTN_ReporteInsumo_Click(object sender, EventArgs e)
        {
            try
            {
                MCWebHogar.DataSets.DSSolicitud dsReporteInsumo = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarInsumos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();estilosElementosBloqueados();", true);
                    return;
                }
                dsReporteInsumo.Tables["DT_EncabezadoInsumo"].Merge(Result, true, MissingSchemaAction.Ignore);

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@InsumoID", HDF_IDInsumo.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarReporteProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("InsumoID");
                    dt.Columns.Add("DescripcionProducto");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReporteInsumo.Tables["DT_DetalleInsumo"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReporteInsumo.Tables["DT_DetalleInsumo"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptInsumo.rdlc", "DT_EncabezadoInsumo", "DT_EncabezadoInsumo");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptInsumo.rdlc", "DT_DetalleInsumo", "DT_DetalleInsumo");

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
                    foreach (DataTable dt in dsReporteInsumo.Tables)
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
                string strFilePDF2 = "ReporteInsumo.pdf";
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

        protected void BTN_DescargarInsumo_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ReporteInsumo", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarInsumo_Click", "desactivarloading();estilosElementosBloqueados();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Insumo");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Insumo.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void TXT_FechaInsumo_OnChange(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@fechaInsumo", TXT_FechaInsumo.Text + " " + TXT_HoraInsumo.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarFechaInsumo", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarInsumo_Click", "alertifywarning('No se ha confirmado el Insumo. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha modificado la fecha del Insumo.');estilosElementosBloqueados();";
                    cargarInsumo(script);
                    cargarProductosInsumo();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha modificado la fecha del Insumo. Por favor, intente de nuevo.');";
                cargarInsumo(script);
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
            DDL_PuntoVenta.SelectedValue = "0";
            DGV_ListaProductosSinAgregar.Enabled = false;
            TXT_BuscarProductosSinAsignar.Enabled = false;
            LB_Categoria.Enabled = false;
            BTN_Agregar.Enabled = false;
            UpdatePanel_ListaProductosSinAgregar.Update();
            UpdatePanel_ModalAgregarProductos.Update();

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

            DT.DT1.Rows.Add("@InsumoID", HDF_IDInsumo.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@PuntoVentaID", DDL_PuntoVenta.SelectedValue, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@DescripcionProducto", TXT_BuscarProductosSinAsignar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosInsumo", SqlDbType.VarChar);

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
            if (DDL_PuntoVenta.SelectedValue == "0")
            {
                DGV_ListaProductosSinAgregar.Enabled = false;
                TXT_BuscarProductosSinAsignar.Enabled = false;
                LB_Categoria.Enabled = false;
                BTN_Agregar.Enabled = false;
                UpdatePanel_ListaProductosSinAgregar.Update();
                UpdatePanel_ModalAgregarProductos.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptAgregarProductos", "estilosElementosBloqueados();cargarFiltros();", true);
            }
            else
            {
                DGV_ListaProductosSinAgregar.Enabled = true;
                TXT_BuscarProductosSinAsignar.Enabled = true;
                LB_Categoria.Enabled = true;
                BTN_Agregar.Enabled = true;
                UpdatePanel_ListaProductosSinAgregar.Update();
                UpdatePanel_ModalAgregarProductos.Update();
                cargarProductosSinAsignar("productosMarcados();");
            }
        }

        protected void DGV_ListaProductosSinAsignar_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@InsumoID", HDF_IDInsumo.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosInsumo", SqlDbType.VarChar);

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
        public static string BTN_Agregar_Click(string idInsumo, int idPuntoVenta, int idProducto, int idUsuario, decimal cantidadProducto, string usuario)
        {        
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@InsumoID", idInsumo, SqlDbType.Int);
            DT.DT1.Rows.Add("@PuntoVentaID", idPuntoVenta, SqlDbType.Int);
            DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@UsuarioID", idUsuario, SqlDbType.Int);
            DT.DT1.Rows.Add("@CantidadInsumo", cantidadProducto, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AgregarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");

            return "correcto";
        }
        #endregion

        #region Productos Insumo
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosInsumo();
        }
        
        public void cargarProductosInsumo()
        {
            string puntosVenta = "";

            #region Puntos Venta
            foreach (ListItem l in LB_PuntoVenta.Items)
            {
                if (l.Selected)
                {
                    puntosVenta += "'" + l.Value + "',";
                }
            }
            puntosVenta = puntosVenta.TrimEnd(',');
            if (puntosVenta != "")
            {
                DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroSucursales", puntosVenta, SqlDbType.VarChar);
            }
            #endregion

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@InsumoID", HDF_IDInsumo.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosInsumo.DataSource = Result;
                    DGV_ListaProductosInsumo.DataBind();
                    UpdatePanel_ListaProductosInsumo.Update();
                    DGV_DetalleInsumo.DataSource = Result;
                    DGV_DetalleInsumo.DataBind();
                    UpdatePanel_DetalleInsumo.Update();
                    string script = "estilosElementosBloqueados();cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosInsumo", script, true);
                }
            }
            else
            {
                DGV_ListaProductosInsumo.DataSource = Result;
                DGV_ListaProductosInsumo.DataBind();
                UpdatePanel_ListaProductosInsumo.Update();
                DGV_DetalleInsumo.DataSource = Result;
                DGV_DetalleInsumo.DataBind();
                UpdatePanel_DetalleInsumo.Update();
                string script = "estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosInsumo", script, true);
            }
        }

        protected void DGV_ListaProductosInsumo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox cantidad = (TextBox)e.Row.FindControl("TXT_Cantidad");

                if (TXT_FechaInsumo.Text != DateTime.Now.ToString("yyyy-MM-dd") && (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0))
                {
                    cantidad.Enabled = false;
                    cantidad.CssClass = "form-control";
                }
            }
        }
        
        protected void DGV_ListaProductosInsumo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosInsumo.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
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
                cantidad.Text = cantidadProducto.ToString();
                if (modifico)
                    guardarProductoInsumo(index);
                
            }
        }

        protected void DGV_ListaProductosInsumo_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@InsumoID", HDF_IDInsumo.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");

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
                    DGV_ListaProductosInsumo.DataSource = Result;
                    DGV_ListaProductosInsumo.DataBind();
                    UpdatePanel_ListaProductosInsumo.Update();
                }
            }
            else
            {
                DGV_ListaProductosInsumo.DataSource = Result;
                DGV_ListaProductosInsumo.DataBind();
                UpdatePanel_ListaProductosInsumo.Update();
            }
        }        
        
        private void guardarProductoInsumo(int index)
        {
            TextBox cantidad = DGV_ListaProductosInsumo.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDInsumoDetalle = DGV_ListaProductosInsumo.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDInsumoDetalle", IDInsumoDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadInsumo", cantidadProducto, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosInsumo();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarInsumo("");
                }
            }
        }
        
        [WebMethod()]
        public static string guardarCantidadProductoInsumo(int idInsumoDetalle, decimal cantidadProducto, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDInsumoDetalle", idInsumoDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadInsumo", cantidadProducto, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP21_0001");

            return "correct";
        }

        protected void BTN_AbrirModalDetalleInsumo_Click(object sender, EventArgs e)
        {
            cargarProductosInsumo();
            string printer = Session["Printer"].ToString().Trim();
            TXT_NombreImpresora.Text = printer;
            UpdatePanel_DetalleInsumo.Update();
            string script = "abrirModalDetalleInsumo();estilosElementosBloqueados();cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AbrirModalDetalleInsumo_Click", script, true);
        }

        protected void BTN_ImprimirDetalleInsumo_Click(object sender, EventArgs e)
        {
            string fechaInsumo = TXT_FechaInsumo.Text;
            decimal monto = Convert.ToDecimal(TXT_MontoInsumo.Text);
            string montoInsumo = string.Format("{0:n0}", monto);
            string printer = TXT_NombreImpresora.Text.Trim();
            string script = "estilosElementosBloqueados();imprimir('" + montoInsumo + "', '" + fechaInsumo + "', '" + TXT_CodigoInsumo.Text + "', '" + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirDetalleEmapque_Click", script, true);
        }
        #endregion
        #endregion
    }
}