using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetallePedido : System.Web.UI.Page
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
                    if (Session["IDPedido"] == null)
                    {
                        Response.Redirect("Pedido.aspx", true);
                    }
                    else
                    {
                        HDF_IDPedido.Value = Session["IDPedido"].ToString();
                        cargarDDLs();
                        cargarPedido("");
                        cargarProductosSinAsignar();
                        cargarProductosPedido();
                        ViewState["Ordenamiento"] = "ASC";
                    }
                }
            }
            else
            {
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

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVenta", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_PuntoVenta.DataSource = Result;
                DDL_PuntoVenta.DataTextField = "DescripcionPuntoVenta";
                DDL_PuntoVenta.DataValueField = "IDPuntoVenta";
                DDL_PuntoVenta.DataBind();
            }
        }
        #endregion

        #region Pedidos
        public void cargarPedido(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDPedido", HDF_IDPedido.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

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
                        TXT_CodigoPedido.Text = dr["NumeroPedido"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_MontoPedido.Text = dr["MontoPedido"].ToString().Trim();
                        TXT_EstadoPedido.Text = dr["Estado"].ToString().Trim();
                        TXT_DescripcionPedido.Text = dr["DescripcionPedido"].ToString().Trim();
                        TXT_FechaPedido.Text = dr["FPedido"].ToString().Trim();
                        TXT_HoraPedido.Text = dr["HPedido"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();
                        DDL_PlantaProduccion.SelectedValue = dr["PlantaProduccionID"].ToString().Trim();
                        DDL_PuntoVenta.SelectedValue = dr["PuntoVentaID"].ToString().Trim();

                        HDF_EstadoPedido.Value = dr["Estado"].ToString().Trim();
                        if (HDF_EstadoPedido.Value != "Preparación")
                        {
                            BTN_AgregarProductos.Visible = false;
                            BTN_GuardarPedido.Visible = false;
                            BTN_ConfirmarPedido.Visible = false;
                        }

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

        protected void BTN_GuardarPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            int solicitante = Convert.ToInt32(DDL_Propietario.SelectedValue);
            int plantaProduccion = Convert.ToInt32(DDL_PlantaProduccion.SelectedValue);
            int puntoVenta = Convert.ToInt32(DDL_PuntoVenta.SelectedValue);

            DT.DT1.Rows.Add("@IDPedido", HDF_IDPedido.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@UsuarioID", solicitante, SqlDbType.Int);
            DT.DT1.Rows.Add("@PlantaProduccionID", plantaProduccion, SqlDbType.Int);
            DT.DT1.Rows.Add("@PuntoVentaID", puntoVenta, SqlDbType.Int);
            DT.DT1.Rows.Add("@DescripcionPedido", TXT_DescripcionPedido.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "EditarPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_GuardarPedido_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    cargarPedido("");
                }
            }
            else
            {
                cargarPedido("");
            }
        }

        protected void BTN_ConfirmarPedido_Click(object sender, EventArgs e)
        {
            string script = "abrirModalConfirmarPedido();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarPedido_OnClick", script, true);
        }

        protected void BTN_ConfirmacionPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDPedido", HDF_IDPedido.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarPedido_OnClick", "alertifywarning('No se ha confirmado el pedido. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "cerrarModalConfirmarPedido();alertifysuccess('Se ha confirmado el pedido.');";
                    cargarPedido(script);
                    cargarProductosPedido();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el pedido.');";
                cargarPedido(script);
            }
        }

        protected void BTN_ImprimirPedido_Click(object sender, EventArgs e)
        {
            try
            {
                MCWebHogar.DataSets.DSSolicitud dsReportePedido = new MCWebHogar.DataSets.DSSolicitud();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@IDPedido", HDF_IDPedido.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();estilosElementosBloqueados();", true);
                    return;
                }
                dsReportePedido.Tables["DT_EncabezadoPedido"].Merge(Result, true, MissingSchemaAction.Ignore);

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");
                if (Result.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PedidoID");
                    dt.Columns.Add("DescripcionProducto");
                    dt.Rows.Add("1", "No hay registros.");
                    dsReportePedido.Tables["DT_DetallePedido"].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    dsReportePedido.Tables["DT_DetallePedido"].Merge(Result, true, MissingSchemaAction.Ignore);
                }

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPedido.rdlc", "DT_EncabezadoPedido", "DT_EncabezadoPedido");
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptPedido.rdlc", "DT_DetallePedido", "DT_DetallePedido");

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
                string direccion = "/ControlPedidos/ReportesTemp/" + strFilePDF2;
                string _open = "window.open('" + direccion + "'  , '_blank');desactivarloading();estilosElementosBloqueados();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", _open, true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void BTN_DescargarPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDPedido", HDF_IDPedido.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ReportePedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarPedido_Click", "desactivarloading();estilosElementosBloqueados();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Pedido");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Pedido.xlsx");
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
        #region Asignar
        protected void BTN_CargarProductos_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptAgregarProductos", "abrirModalAgregarProductos();estilosElementosBloqueados();", true);
            return;
        }

        public void cargarProductosSinAsignar()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

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
                }
            }
            else
            {
                DGV_ListaProductosSinAgregar.DataSource = Result;
                DGV_ListaProductosSinAgregar.DataBind();
                UpdatePanel_ListaProductosSinAgregar.Update();
            }
        }

        protected void DGV_ListaProductosSinAsignar_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

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

        protected void BTN_Agregar_Click(object sender, EventArgs e)
        {
            int contador = 0;
            string fila = "";
            foreach (GridViewRow row in DGV_ListaProductosSinAgregar.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("CHK_Prodcuto");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        fila += row.RowIndex + ",";
                        contador++;
                    }
                }
            }
            if (fila != "" && contador > 0)
            {
                fila = fila.TrimEnd(',');
                string[] listaFilas = fila.Split(',');

                string productos = "";

                foreach (string f in listaFilas)
                {
                    productos += DGV_ListaProductosSinAgregar.DataKeys[Convert.ToInt32(f)].Value.ToString().Trim() + ",";
                }
                productos = productos.TrimEnd(',');

                DT.DT1.Clear();

                DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@ListaProductos", productos, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "AgregarProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_GuardarPedido_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        DGV_ListaProductos.DataSource = Result;
                        DGV_ListaProductos.DataBind();
                        UpdatePanel_ListaProductos.Update();
                        string script = "cerrarModalAgregarProductos();alertifysuccess('Productos agregados con éxito.');";
                        cargarProductosSinAsignar();
                        cargarPedido(script);
                    }
                }
                else
                {
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_ListaProductos.Update();
                    cargarProductosSinAsignar();
                }
            }
        }
        #endregion

        #region Productos Pedido
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosPedido();
        }

        public void cargarProductosPedido()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
        }

        protected void DGV_ListaProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox cantidad = (TextBox)e.Row.FindControl("TXT_Cantidad");
                DropDownList ddlUnds = (DropDownList)e.Row.FindControl("DDL_Unidades");
                DropDownList ddlDecs = (DropDownList)e.Row.FindControl("DDL_Decenas");
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                int unds = Convert.ToInt32(cantidadProducto) % 10;
                int decs = Convert.ToInt32(cantidadProducto) / 10;
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();

                if (HDF_EstadoPedido.Value != "Preparación")
                {
                    Button minus = (Button)e.Row.FindControl("BTN_Minus");
                    Button plus = (Button)e.Row.FindControl("BTN_Plus");
                    Button eliminar = (Button)e.Row.FindControl("BTN_EliminarProducto");

                    cantidad.Enabled = false;
                    cantidad.CssClass = "form-control";
                    ddlUnds.Enabled = false;
                    ddlUnds.CssClass = "form-control";
                    ddlDecs.Enabled = false;
                    ddlDecs.CssClass = "form-control";
                    minus.Enabled = false;
                    minus.CssClass = "btn btn-outline-primary btn-round";
                    plus.Enabled = false;
                    plus.CssClass = "btn btn-outline-primary btn-round";
                    eliminar.Enabled = false;
                    eliminar.CssClass = "btn btn-outline-danger btn-round";
                }
            }
        }

        protected void DGV_ListaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Eliminar")
                {
                    string IDPedidoDetalle = DGV_ListaProductos.DataKeys[index].Value.ToString().Trim();

                    DT.DT1.Clear();
                    DT.DT1.Rows.Add("@IDPedidoDetalle", IDPedidoDetalle, SqlDbType.VarChar);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "DeleteProducto", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                        {
                            cargarProductosPedido();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        }
                        else
                        {
                            cargarPedido("");
                            cargarProductosSinAsignar();
                            cargarProductosPedido();
                        }
                    }
                }
                else
                {
                    TextBox cantidad = DGV_ListaProductos.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                    decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                    DropDownList ddlUnds = DGV_ListaProductos.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                    DropDownList ddlDecs = DGV_ListaProductos.Rows[index].FindControl("DDL_Decenas") as DropDownList;
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
                        guardarProductoPedido(index);
                }
            }
        }

        protected void TXT_Cantidad_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            TextBox cantidad = sender as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            DropDownList ddlUnds = DGV_ListaProductos.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductos.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            int unds = Convert.ToInt32(cantidadProducto) % 10;
            int decs = Convert.ToInt32(cantidadProducto) / 10;
            if (cantidadProducto > 0 && cantidadProducto < 99)
            {
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                guardarProductoPedido(index);
            }
            else
            {
                unds = Convert.ToInt32(ddlUnds.SelectedValue);
                decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
                cantidadProducto = decs + unds;
                cantidad.Text = cantidadProducto.ToString();
            }
        }

        protected void DDL_DecenasUnidades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            DropDownList ddlUnds = DGV_ListaProductos.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductos.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductos.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            decimal cantidadProducto = decs + unds;
            cantidad.Text = cantidadProducto.ToString();
            guardarProductoPedido(index);
        }

        private void guardarProductoPedido(int index)
        {
            TextBox cantidad = DGV_ListaProductos.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDPedidoDetalle = DGV_ListaProductos.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDPedidoDetalle", IDPedidoDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadProduccion", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosPedido();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarPedido("");
                }
            }
        }

        protected void DGV_ListaProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@PedidoID", HDF_IDPedido.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP05_0001");

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
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
        }
        #endregion
        #endregion
    }
}