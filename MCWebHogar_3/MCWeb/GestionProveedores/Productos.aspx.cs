using CapaLogica.Entidades.ControlPedidos;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MCWebHogar.ControlPedidos;
using MCWebHogar.ControlPedidos.Proveedores;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.GestionProveedores
{
    public partial class Productos : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Proveedores", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Proveedores.");
                    Response.Redirect("../ControlPedidos/Pedido.aspx");
                }
                else
                {
                    cargarFiltros();
                    cargarProductos("");
                    HDF_IDUsuario.Value = Session["Usuario"].ToString();
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarProductos("");
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        private void cargarFiltros()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmisores", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

            LB_Emisores.DataSource = Result;
            LB_Emisores.DataTextField = "NombreComercial";
            LB_Emisores.DataValueField = "IDEmisor";
            LB_Emisores.DataBind();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0002");

            LB_Categorias.DataSource = Result;
            LB_Categorias.DataTextField = "DescripcionCategoria";
            LB_Categorias.DataValueField = "IDCategoria";
            LB_Categorias.DataBind();

            DDL_Categoria.DataSource = Result;
            DDL_Categoria.DataTextField = "DescripcionCategoria";
            DDL_Categoria.DataValueField = "IDCategoria";
            DDL_Categoria.DataBind();
            UpdatePanel_ModalEditarProducto.Update();

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");

            dt.Rows.Add(0, "Tipo productos -todos-");
            dt.Rows.Add(1, "MateriaPrima");
            dt.Rows.Add(2, "Venta");

            DDL_TipoProducto.DataSource = dt;
            DDL_TipoProducto.DataTextField = "Text";
            DDL_TipoProducto.DataValueField = "Value";
            DDL_TipoProducto.DataBind();

            UpdatePanel_FiltrosProductos.Update();
        }

        protected void BTN_Sincronizar_Click(object sender, EventArgs e)
        {            
            LecturaXML xml = new LecturaXML();
            xml.ReadEmails();
            xml.ReadXML();
            cargarProductos("");
            string script = "desactivarloading();alertifysuccess('Sincronizacion completada');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_Sincronizar_Click", script, true);            
        }
        #endregion 
      
        #region Productos
        private DataTable cargarProductosConsulta(string consulta)
        {
            DT.DT1.Clear();

            string emisores = "";
            string emisoresText = "";
            string categorias = "";
            string categoriasText = "";

            #region Emisores
            foreach (ListItem l in LB_Emisores.Items)
            {
                if (l.Selected)
                {
                    emisores += "'" + l.Value + "',";
                    emisoresText += l.Text + ",";
                }
            }
            emisores = emisores.TrimEnd(',');
            emisoresText = emisoresText.TrimEnd(',');
            if (emisoresText != "")
            {
                DT.DT1.Rows.Add("@FiltrarEmisor", 1, SqlDbType.Int);
            }
            #endregion

            #region Categorias
            foreach (ListItem l in LB_Categorias.Items)
            {
                if (l.Selected)
                {
                    categorias += "'" + l.Value + "',";
                    categoriasText += l.Text + ",";
                }
            }
            categorias = categorias.TrimEnd(',');
            categoriasText = categoriasText.TrimEnd(',');
            if (categoriasText != "")
            {
                DT.DT1.Rows.Add("@FiltrarCategoria", 1, SqlDbType.Int);
            }
            #endregion

            #region Tipo Producto
            switch (DDL_TipoProducto.SelectedValue)
            {
                case "1":
                    DT.DT1.Rows.Add("@EsMateriaPrima", 1, SqlDbType.Int);
                    break;
                case "2":
                    DT.DT1.Rows.Add("@EsVenta", 1, SqlDbType.Int);
                    break;
            }
            #endregion

            DT.DT1.Rows.Add("@EmisoresFiltro", emisores, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@DetalleProducto", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_FiltrosProductos.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");
        }

        private void cargarProductos(string ejecutar)
        {
            Result = cargarProductosConsulta("CargarProductosAll");

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
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        private void cargarProductosDetalle(string ejecutar, int idProducto)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosDetalle", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_DetelleProductos.DataSource = Result;
                    DGV_DetelleProductos.DataBind();
                    UpdatePanel_DetalleProductos.Update();
                }
            }
            else
            {
                DGV_DetelleProductos.DataSource = Result;
                DGV_DetelleProductos.DataBind();
                UpdatePanel_DetalleProductos.Update();
            }
            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarGraficos(" + usuario + ", " + idProducto +");";
            string script = "cargarFiltros();" + cargar + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void FiltrarProductos_OnClick(object sender, EventArgs e)
        {
            Result = cargarProductosConsulta("CargarProductosAll");

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

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptFiltrarProductos_OnClick", script, true);
        }

        protected void DGV_ListaProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarProductosConsulta("CargarProductosAll");

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
            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaProductos_Sorting", script, true);
        }

        protected void DGV_ListaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProducto = Convert.ToInt32(DGV_ListaProductos.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarDetalleProducto", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDProducto.Value = dr["IDProducto"].ToString().Trim();
                            TXT_DetalleProducto.Text = dr["DetalleProducto"].ToString().Trim();
                            TXT_CodigoPOS.Text = dr["CodigoProductoWebpos"].ToString().Trim();
                            TXT_FechaActualizado.Text = dr["FechaActualizacion"].ToString().Trim();
                            TXT_PrecioVenta.Text = dr["PrecioVenta"].ToString().Trim();
                            TXT_PrecioUnitario.Text = String.Format("{0:n}", dr["PrecioUnitario"]);
                            TXT_MontoDescuento.Text = String.Format("{0:n}", dr["MontoDescuento"]);
                            TXT_Impuesto.Text = String.Format("{0:n0}%", dr["PorcentajeImpuesto"]);
                            TXT_MontoImpuesto.Text = String.Format("{0:n}", dr["MontoImpuesto"]);
                            TXT_MontoImpuestoIncluido.Text = String.Format("{0:n}", dr["MontoImpuestoIncluido"]);
                            TXT_Porcentaje25.Text = String.Format("{0:n}", dr["Porcentaje25"]);
                            DDL_Categoria.SelectedValue = dr["CategoriaID"].ToString().Trim();
                        }
                    }

                    cargarUnidadesMedida();
                    UpdatePanel_ModalEditarProducto.Update();

                    string script = "abrirModalEditarProducto();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EditarProducto_OnClick", script, true);
                }
                if (e.CommandName == "VerProductos")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarDetalleProducto", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDProducto.Value = dr["IDProducto"].ToString().Trim();
                            TXT_DetalleProductoD.Text = dr["DetalleProducto"].ToString().Trim();
                            TXT_FechaActualizadoD.Text = dr["FechaActualizacion"].ToString().Trim();
                            TXT_PrecioUnitarioD.Text = String.Format("{0:n}", dr["PrecioUnitario"]);
                            TXT_MontoDescuentoD.Text = dr["PrecioVenta"].ToString().Trim();
                            TXT_ImpuestoD.Text = String.Format("{0:n0}%", dr["PorcentajeImpuesto"]);
                            TXT_MontoImpuestoD.Text = String.Format("{0:n}", dr["MontoImpuesto"]);
                            TXT_MontoImpuestoIncluidoD.Text = String.Format("{0:n}", dr["MontoImpuestoIncluido"]);
                            TXT_Porcentaje25D.Text = String.Format("{0:n}", dr["Porcentaje25"]);
                            TXT_PrecioVentaD.Text = String.Format("{0:n}", dr["PrecioVenta"]);
                        }
                    }

                    UpdatePanel_DetalleProductos.Update();

                    cargarProductosDetalle("abrirModalVerProductos();", idProducto);
                }
            }
        }

        protected void BTN_GuardarProducto_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProducto", HDF_IDProducto.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@DetalleProducto", TXT_DetalleProducto.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CodigoProductoWebpos", TXT_CodigoPOS.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CategoriaID", DDL_Categoria.SelectedValue, SqlDbType.Int);
            DT.DT1.Rows.Add("@PrecioVenta", TXT_PrecioVenta.Text, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Actualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");
            string script = "";
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    script += "alertifyerror('No se ha guardado el producto. Error: " + Result.Rows[0][1].ToString().Trim() + "');";
                    cargarProductos(script);
                }
                else
                {
                    script += "cerrarModalEditarProducto();alertifysuccess('Se ha guardado el producto con éxito.')";
                    cargarProductos(script);
                }
            }
            else
            {
                script += "alertifywarning('No se ha guardado el producto. Por favor, intente nuevamente');";
                cargarProductos(script);
            }
        }

        protected void BTN_DescargarProducto_OnClick(object sender, EventArgs e)
        {
            DataTable ResultHistorico = new DataTable();

            Result = cargarProductosConsulta("CargarProductosReporte");
            ResultHistorico = cargarProductosConsulta("ReporteProductosHistorico");
            
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Productos");
                wb.Worksheets.Add(ResultHistorico, "Historico");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Productos.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            UpdatePanel_ListaProductos.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarProducto_OnClick", "desactivarloading();cargarFiltros();", true);
        }

        #region Unidades de Medida
        private DataTable cargarUnidadesMedidaConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoID", HDF_IDProducto.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_ModalEditarProducto.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0003");
        }

        private void cargarUnidadesMedida()
        {
            Result = cargarUnidadesMedidaConsulta("CargarUnidadesMedida");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_UnidadesMedida.DataSource = Result;
                    DGV_UnidadesMedida.DataBind();
                    UpdatePanel_ModalEditarProducto.Update();
                }
            }
            else
            {
                DGV_UnidadesMedida.DataSource = Result;
                DGV_UnidadesMedida.DataBind();
                UpdatePanel_ModalEditarProducto.Update();
            }
        }
        #endregion

        [WebMethod()]
        public static List<DatosHistoricos> cargarGraficoHistoricoPrecios(string idUsuario, string idProducto)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarHistoricoPrecios", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.dia = dr["Fecha"].ToString().Trim();
                    dh.precioUnitario = Convert.ToDecimal(dr["PrecioUnitarioFinal"].ToString().Trim());

                    lista.Add(dh);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return lista;
        }

        [WebMethod()]
        public static string BTN_GuardarProducto_Click(string idProducto, string detalleProducto, string codigoProductoWebpos, string categoria, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@DetalleProducto", detalleProducto, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CodigoProductoWebpos", codigoProductoWebpos, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CategoriaID", categoria, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Actualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");            
            return "correcto";
        }

        [WebMethod()]
        public static string BTN_GuardarUnidadesMedida_Click(string idProducto, int idUnidadMedida, int cantidadEquivalente, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@IDUnidadMedida", idUnidadMedida, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadEquivalente", cantidadEquivalente, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Actualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0003");
            return "correcto";
        }
        #endregion
    }
}