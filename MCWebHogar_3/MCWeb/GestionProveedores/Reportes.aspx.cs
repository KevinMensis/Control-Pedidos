using CapaLogica.Entidades.ControlPedidos;
using ClosedXML.Excel;
using MCWebHogar.ControlPedidos;
using MCWebHogar.ControlPedidos.Proveedores;
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
    public partial class Reportes : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();
        string identificacionReceptor = "";

        decimal TotalVenta = 0;
        decimal TotalDescuento = 0;
        decimal TotalImpuesto = 0;
        decimal TotalIVAI = 0;
        decimal Cantidad = 0;

        decimal MontoDescuentoUnitario = 0;
        decimal MontoImpuestoUnitario = 0;
        decimal PrecioImpuesto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            identificacionReceptor = Session["IdentificacionReceptor"].ToString().Trim();

            if (!Page.IsPostBack)
            {
                if (identificacionReceptor == "3101485961")
                {
                    li_MiKFe.Attributes.Add("class", "active");
                    H1_Title.InnerText = "La Piedra Calisa SA - Reportes";
                }
                else if (identificacionReceptor == "115210651")
                {
                    li_Esteban.Attributes.Add("class", "active");
                    H1_Title.InnerText = "Panadería La Central - Reportes";
                }
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
                    CargarDatos();
                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("Identificacion"))
                {
                    string identificacion = opcion.Split(';')[1];
                    Session["IdentificacionReceptor"] = identificacion;
                    Response.Redirect("../GestionProveedores/Proveedores.aspx", true);
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        protected void BTN_Sincronizar_Click(object sender, EventArgs e)
        {
            LecturaXML xml = new LecturaXML();
            xml.ReadXML();
            UpdatePanel_Progress.Update();
            xml.ReadEmails();
            UpdatePanel_Progress.Update();
            xml.ReadXML();
            cargarEmisores();
            cargarFacturas();
            cargarProductos();
            cargarComprasMensuales();
            cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
            string script = "desactivarloading();alertifysuccess('Sincronizacion completada');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_Sincronizar_Click", script, true);
        }

        protected void CargarDatos()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar); 
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0002");

            LB_Categorias.DataSource = Result;
            LB_Categorias.DataTextField = "DescripcionCategoria";
            LB_Categorias.DataValueField = "IDCategoria";
            LB_Categorias.DataBind();

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

            #region Fechas
            DateTime hoy = DateTime.Now;
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            TXT_FechaDesde.Text = Convert.ToString(hoy.Year) + "-01-01";
            TXT_FechaHasta.Text = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;                       
            #endregion
        }

        protected void Recargar_Click(object sender, EventArgs e)
        {
            cargarEmisores();
            cargarFacturas();
            cargarProductos();
            cargarComprasMensuales();
            cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
        } 
        #endregion 
      
        #region Emisores
        private DataTable cargarEmisoresConsulta(string consulta)
        {
            DT.DT1.Clear();

            #region Fechas
            DateTime hoy = DateTime.Now;
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            string fechaDesde = "1900-01-01";
            string fechaHasta = "1900-01-01";

            try
            {
                fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
            }
            catch (Exception e)
            {
                fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
            }
            try
            {
                hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
            }
            catch (Exception e)
            {
                fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
            }

            DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
            DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
            #endregion

            #region Busqueda
            DT.DT1.Rows.Add("@NombreComercial", TXT_BuscarEmisor.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_BuscarFactura.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);
            #endregion

            DT.DT1.Rows.Add("@IDEmisor", HDF_IDEmisor.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@IDFactura", HDF_IDFactura.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@IDProducto", HDF_IDProducto.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoProducto", DDL_TipoProducto.SelectedValue, SqlDbType.Int);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_FiltrosReportes.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");
        }

        private void cargarEmisores()
        {
            Result = cargarEmisoresConsulta("EmisoresReporte");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaEmisores.DataSource = Result;
                    DGV_ListaEmisores.DataBind();
                    UpdatePanel_ListaEmisores.Update();
                }
            }
            else
            {
                DGV_ListaEmisores.DataSource = Result;
                DGV_ListaEmisores.DataBind();
                UpdatePanel_ListaEmisores.Update();
            }            
        }

        protected void DGV_ListaEmisores_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarEmisoresConsulta("EmisoresReporte");

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
                    DGV_ListaEmisores.DataSource = Result;
                    DGV_ListaEmisores.DataBind();
                    UpdatePanel_ListaEmisores.Update();
                }
            }
            else
            {
                DGV_ListaEmisores.DataSource = Result;
                DGV_ListaEmisores.DataBind();
                UpdatePanel_ListaEmisores.Update();
            }
            string script = "cargarFiltros();freezeEmisorHeader();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaEmisores_Sorting", script, true);
        }

        protected void DGV_ListaEmisores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGV_ListaEmisores, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECEBDF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#fff'");                
            }
        }

        protected void DGV_ListaEmisores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idEmisor = Convert.ToInt32(DGV_ListaEmisores.DataKeys[rowIndex].Value.ToString().Trim());
                
                if (e.CommandName == "Select")
                {
                    HDF_IDEmisor.Value = HDF_IDEmisor.Value != "0" ? "0" : idEmisor.ToString();
                    HDF_IDFactura.Value = HDF_IDEmisor.Value != "0" ? "0" : HDF_IDFactura.Value;
                    HDF_IDProducto.Value = HDF_IDEmisor.Value != "0" ? "0" : HDF_IDProducto.Value;

                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
                }
            }
        }
        #endregion

        #region Facturas
        private DataTable cargarFacturasConsulta(string consulta)
        {
            if (HDF_IDEmisor.Value != "0")
            {
                DT.DT1.Clear();

                #region Fechas
                DateTime hoy = DateTime.Now;
                string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                string fechaDesde = "1900-01-01";
                string fechaHasta = "1900-01-01";

                try
                {
                    fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
                }
                catch (Exception e)
                {
                    fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
                }
                try
                {
                    hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                    mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                    dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }
                catch (Exception e)
                {
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }

                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
                #endregion

                #region Busqueda
                DT.DT1.Rows.Add("@NombreComercial", TXT_BuscarEmisor.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_BuscarFactura.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);
                #endregion

                DT.DT1.Rows.Add("@EmisorID", HDF_IDEmisor.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@IDFactura", HDF_IDFactura.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@IDProducto", HDF_IDProducto.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

                return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP02_0001");
            }
            else
            {
                return null;
            }
        }

        private void cargarFacturas()
        {
            Result = cargarFacturasConsulta("FacturasReporte");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaFacturas.DataSource = Result;
                    DGV_ListaFacturas.DataBind();
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_ListaFacturas.Update();
            }
        }

        protected void DGV_ListaFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarFacturasConsulta("FacturasReporte");

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
                    DGV_ListaFacturas.DataSource = Result;
                    DGV_ListaFacturas.DataBind();
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_ListaFacturas.Update();
            }
            string script = "cargarFiltros();freezeFacturaHeader();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaFacturas_Sorting", script, true);
        }

        protected void DGV_ListaFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGV_ListaFacturas, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECEBDF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#fff'");

                Label LBL_TotalVenta = (Label)e.Row.FindControl("LBL_TotalVenta");
                LBL_TotalVenta.Text = Decimal.Parse(LBL_TotalVenta.Text).ToString("N2");
                TotalVenta += Convert.ToDecimal(LBL_TotalVenta.Text);

                Label LBL_TotalDescuento = (Label)e.Row.FindControl("LBL_TotalDescuento");
                LBL_TotalDescuento.Text = Decimal.Parse(LBL_TotalDescuento.Text).ToString("N2");
                TotalDescuento += Convert.ToDecimal(LBL_TotalDescuento.Text);

                Label LBL_TotalImpuesto = (Label)e.Row.FindControl("LBL_TotalImpuesto");
                LBL_TotalImpuesto.Text = Decimal.Parse(LBL_TotalImpuesto.Text).ToString("N2");
                TotalImpuesto += Convert.ToDecimal(LBL_TotalImpuesto.Text);

                Label LBL_TotalComprobante = (Label)e.Row.FindControl("LBL_TotalComprobante");
                LBL_TotalComprobante.Text = Decimal.Parse(LBL_TotalComprobante.Text).ToString("N2");
                TotalIVAI += Convert.ToDecimal(LBL_TotalComprobante.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_TotalVenta = (Label)e.Row.FindControl("LBL_FOO_TotalVenta");
                LBL_FOO_TotalVenta.Text = TotalVenta.ToString("N2");

                Label LBL_FOO_TotalDescuento = (Label)e.Row.FindControl("LBL_FOO_TotalDescuento");
                LBL_FOO_TotalDescuento.Text = TotalDescuento.ToString("N2");

                Label LBL_FOO_TotalImpuesto = (Label)e.Row.FindControl("LBL_FOO_TotalImpuesto");
                LBL_FOO_TotalImpuesto.Text = TotalImpuesto.ToString("N2");

                Label LBL_FOO_TotalComprobante = (Label)e.Row.FindControl("LBL_FOO_TotalComprobante");
                LBL_FOO_TotalComprobante.Text = TotalIVAI.ToString("N2");
            }
        }

        protected void DGV_ListaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idFactura = Convert.ToInt32(DGV_ListaFacturas.DataKeys[rowIndex].Value.ToString().Trim());
                int idEmisor = Convert.ToInt32(DGV_ListaFacturas.DataKeys[rowIndex].Values[1].ToString().Trim());

                if (e.CommandName == "Select")
                {
                    HDF_IDFactura.Value = HDF_IDFactura.Value != "0" ? "0" : idFactura.ToString();
                    HDF_IDProducto.Value = HDF_IDFactura.Value != "0" ? "0" : HDF_IDProducto.Value;

                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
                }
            }
        }
        #endregion

        #region Productos
        private DataTable cargarProductosConsulta(string consulta)
        {
            if (HDF_IDEmisor.Value != "0")
            {
                DT.DT1.Clear();

                #region Fechas
                DateTime hoy = DateTime.Now;
                string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                string fechaDesde = "1900-01-01";
                string fechaHasta = "1900-01-01";

                try
                {
                    fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
                }
                catch (Exception e)
                {
                    fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
                }
                try
                {
                    hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                    mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                    dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }
                catch (Exception e)
                {
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }

                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
                #endregion

                #region Categorias
                string categorias = "";
                string categoriasText = "";
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

                #region Busqueda
                DT.DT1.Rows.Add("@NombreComercial", TXT_BuscarEmisor.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_BuscarFactura.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);
                #endregion

                DT.DT1.Rows.Add("@IDProducto", HDF_IDProducto.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@EmisorID", HDF_IDEmisor.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@FacturaID", HDF_IDFactura.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);
                                                    
                return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");
            }
            else
            {
                return null;
            }
        }

        private void cargarProductos()
        {
            Result = cargarProductosConsulta("ProductosReporte");

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

        protected void DGV_ListaProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarProductosConsulta("ProductosReporte");

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
            string script = "cargarFiltros();freezeProductoHeader();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaProductos_Sorting", script, true);
        }

        protected void DGV_ListaProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGV_ListaProductos, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECEBDF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#fff'");
            }
        }

        protected void DGV_ListaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProducto = Convert.ToInt32(DGV_ListaProductos.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "Select")
                {
                    HDF_IDProducto.Value = HDF_IDProducto.Value != "0" ? "0" : idProducto.ToString();
                    HDF_IDFactura.Value = HDF_IDProducto.Value != "0" ? "0" : HDF_IDFactura.Value;

                    string usuario = Session["UserID"].ToString().Trim();
                    string cargar = "";

                    if (HDF_IDProducto.Value != "0")
                    {
                        cargar = "abrirModalVerProductos();cargarGraficos(" + usuario + ", " + idProducto + ");";
                    
                        DT.DT1.Clear();

                        DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);

                        DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                        DT.DT1.Rows.Add("@TipoSentencia", "CargarDetalleProducto", SqlDbType.VarChar);

                        Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");

                        if (Result != null && Result.Rows.Count > 0)
                        {
                            foreach (DataRow dr in Result.Rows)
                            {
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

                        UpdatePanel_DetalleProducto.Update();
                    }
                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();" + cargar);
                }
            }
        }

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
        #endregion

        #region Histórico
        private DataTable cargarProductosDetalleConsulta(string consulta)
        {
            if (HDF_IDEmisor.Value != "0")
            {
                DT.DT1.Clear();

                #region Fechas
                DateTime hoy = DateTime.Now;
                string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                string fechaDesde = "1900-01-01";
                string fechaHasta = "1900-01-01";

                try
                {
                    fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
                }
                catch (Exception e)
                {
                    fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
                }
                try
                {
                    hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                    mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                    dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }
                catch (Exception e)
                {
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }

                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
                #endregion

                #region Categorias
                string categorias = "";
                string categoriasText = "";
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

                #region Busqueda
                DT.DT1.Rows.Add("@NombreComercial", TXT_BuscarEmisor.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_BuscarFactura.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);
                #endregion

                DT.DT1.Rows.Add("@ProductoID", HDF_IDProducto.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@EmisorID", HDF_IDEmisor.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@FacturaID", HDF_IDFactura.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

                return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");
            }
            else
            {
                return null;
            }
        }

        private void cargarProductosDetalle(string ejecutar)
        {
            Result = cargarProductosDetalleConsulta("ReporteProductoDetalle");

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
            string script = "cargarFiltros();desactivarloading();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDetalle", script, true);
        }

        protected void DGV_DetelleProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LBL_Cantidad = (Label)e.Row.FindControl("LBL_Cantidad");
                LBL_Cantidad.Text = Decimal.Parse(LBL_Cantidad.Text).ToString("N2");
                Cantidad += Convert.ToDecimal(LBL_Cantidad.Text);

                Label LBL_MontoDescuentoUnitario = (Label)e.Row.FindControl("LBL_MontoDescuentoUnitario");
                LBL_MontoDescuentoUnitario.Text = Decimal.Parse(LBL_MontoDescuentoUnitario.Text).ToString("N2");
                MontoDescuentoUnitario += Convert.ToDecimal(LBL_MontoDescuentoUnitario.Text) * Convert.ToDecimal(LBL_Cantidad.Text);            

                Label LBL_MontoImpuestoUnitario = (Label)e.Row.FindControl("LBL_MontoImpuestoUnitario");
                LBL_MontoImpuestoUnitario.Text = Decimal.Parse(LBL_MontoImpuestoUnitario.Text).ToString("N2");
                MontoImpuestoUnitario += Convert.ToDecimal(LBL_MontoImpuestoUnitario.Text) * Convert.ToDecimal(LBL_Cantidad.Text);

                Label LBL_PrecioUnitarioImpuesto = (Label)e.Row.FindControl("LBL_PrecioUnitarioImpuesto");
                LBL_PrecioUnitarioImpuesto.Text = Decimal.Parse(LBL_PrecioUnitarioImpuesto.Text).ToString("N2");
                PrecioImpuesto += Convert.ToDecimal(LBL_PrecioUnitarioImpuesto.Text) * Convert.ToDecimal(LBL_Cantidad.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_Cantidad = (Label)e.Row.FindControl("LBL_FOO_Cantidad");
                LBL_FOO_Cantidad.Text = Cantidad.ToString("N2");

                Label LBL_FOO_MontoDescuentoUnitario = (Label)e.Row.FindControl("LBL_FOO_MontoDescuentoUnitario");
                LBL_FOO_MontoDescuentoUnitario.Text = (MontoDescuentoUnitario).ToString("N2");

                Label LBL_FOO_MontoImpuestoUnitario = (Label)e.Row.FindControl("LBL_FOO_MontoImpuestoUnitario");
                LBL_FOO_MontoImpuestoUnitario.Text = (MontoImpuestoUnitario).ToString("N2");

                Label LBL_FOO_PrecioUnitarioImpuesto = (Label)e.Row.FindControl("LBL_FOO_PrecioUnitarioImpuesto");
                LBL_FOO_PrecioUnitarioImpuesto.Text = (PrecioImpuesto).ToString("N2");
            }
        }
        #endregion

        #region ComprasMensuales
        private DataTable cargarComprasMensualesConsulta(string consulta)
        {
            if (HDF_IDEmisor.Value != "0")
            {
                BTN_ImprimirComprasMensuales.Visible = true;
                UpdatePanel_ComprasMensualesHeader.Update();
                DT.DT1.Clear();

                #region Fechas
                DateTime hoy = DateTime.Now;
                string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                string fechaDesde = "1900-01-01";
                string fechaHasta = "1900-01-01";

                try
                {
                    fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
                }
                catch (Exception e)
                {
                    fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
                }
                try
                {
                    hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                    mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                    dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }
                catch (Exception e)
                {
                    fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
                }

                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
                #endregion

                #region Categorias
                string categorias = "";
                string categoriasText = "";
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

                #region Busqueda
                DT.DT1.Rows.Add("@NombreComercial", TXT_BuscarEmisor.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_BuscarFactura.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);
                #endregion

                DT.DT1.Rows.Add("@ProductoID", HDF_IDProducto.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@EmisorID", HDF_IDEmisor.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@FacturaID", HDF_IDFactura.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

                return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");
            }
            else
            {
                BTN_ImprimirComprasMensuales.Visible = false;
                UpdatePanel_ComprasMensualesHeader.Update();
                return null;
            }
        }

        private void cargarComprasMensuales()
        {
            Result = cargarComprasMensualesConsulta("ReporteProductoMensuales");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    while (DGV_ComprasMensualesCantidad.Columns.Count > 0)
                    {
                        DGV_ComprasMensualesCantidad.Columns.RemoveAt(0);
                        DGV_ComprasMensualesCostos.Columns.RemoveAt(0);
                    }
                        
                    DGV_ComprasMensualesCantidad.DataBind();
                    DGV_ComprasMensualesCostos.DataBind();

                    foreach(DataColumn col in Result.Columns)
                    {
                        BoundField bField = new BoundField();
                        bField.DataField = col.ColumnName;
                        bField.HeaderText = col.ColumnName.Replace("Compras", "").Replace("Cantidad", "");
                        bField.DataFormatString = "{0:n}";
                        bField.ItemStyle.ForeColor = System.Drawing.Color.Black;
                        bField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                        if (col.ColumnName == "DetalleProducto")
                        {
                            bField.HeaderText = "Descripción";
                            bField.ItemStyle.Width = Unit.Pixel(100);
                            DGV_ComprasMensualesCantidad.Columns.Add(bField);
                            DGV_ComprasMensualesCostos.Columns.Add(bField);
                        }
                        else if (col.ColumnName.Contains("Cantidad"))
                        {
                            DGV_ComprasMensualesCantidad.Columns.Add(bField);
                        }
                        else if (col.ColumnName.Contains("Compras"))
                        {
                            DGV_ComprasMensualesCostos.Columns.Add(bField);
                        }
                    }

                    DGV_ComprasMensualesCantidad.DataSource = Result;
                    DGV_ComprasMensualesCantidad.DataBind();

                    DGV_ComprasMensualesCostos.DataSource = Result;
                    DGV_ComprasMensualesCostos.DataBind();

                    UpdatePanel_ComprasMensualesCantidad.Update();
                    UpdatePanel_ComprasMensualesCostos.Update();
                }
            }
            else
            {
                DGV_ComprasMensualesCantidad.DataSource = Result;
                DGV_ComprasMensualesCantidad.DataBind();
                UpdatePanel_ComprasMensualesCantidad.Update();

                DGV_ComprasMensualesCostos.DataSource = Result;
                DGV_ComprasMensualesCostos.DataBind();
                UpdatePanel_ComprasMensualesCostos.Update();
            }
        }

        protected void DGV_ComprasMensualesCantidad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGV_ComprasMensualesCantidad, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECEBDF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#fff'");
            }
        }

        protected void DGV_ComprasMensualesCostos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGV_ComprasMensualesCostos, "Select$" + e.Row.RowIndex);
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECEBDF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#fff'");
            }
        }

        protected void DGV_ComprasMensualesCantidad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProducto = Convert.ToInt32(DGV_ComprasMensualesCantidad.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "Select")
                {
                    HDF_IDProducto.Value = HDF_IDProducto.Value != "0" ? "0" : idProducto.ToString();
                    HDF_IDFactura.Value = HDF_IDProducto.Value != "0" ? "0" : HDF_IDFactura.Value;
                        
                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
                }
            }
        }

        protected void DGV_ComprasMensualesCostos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProducto = Convert.ToInt32(DGV_ComprasMensualesCostos.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "Select")
                {
                    HDF_IDProducto.Value = HDF_IDProducto.Value != "0" ? "0" : idProducto.ToString();
                    HDF_IDFactura.Value = HDF_IDProducto.Value != "0" ? "0" : HDF_IDFactura.Value;

                    cargarEmisores();
                    cargarFacturas();
                    cargarProductos();
                    cargarComprasMensuales();
                    cargarProductosDetalle("freezeEmisorHeader();freezeFacturaHeader();freezeProductoHeader();freezeProductoDetalleHeader();freezeComprasMensualesCantidadHeader();freezeComprasMensualesCostosHeader();");
                }
            }
        }

        protected void BTN_ImprimirComprasMensuales_Click(object sender, EventArgs e)
        {
            try
            {
                MCWebHogar.DataSets.DSSolicitud dsReportecompras = new MCWebHogar.DataSets.DSSolicitud();

                Result = cargarComprasMensualesConsulta("ImprimirReporteProductoMensual");
                if (Result.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alertifywarning('No hay datos para mostrar');desactivarloading();", true);
                    return;
                }
                dsReportecompras.Tables["DT_DetalleCompras"].Merge(Result, true, MissingSchemaAction.Ignore);

                DataTable DT_Encabezado = new DataTable();

                DT_Encabezado.Columns.Add("Codigo");
                DT_Encabezado.Columns.Add("Descripcion");
                DT_Encabezado.Columns.Add("Procedure");
                DT_Encabezado.Columns.Add("rpt");
                DT_Encabezado.Columns.Add("DataSet");
                DT_Encabezado.Columns.Add("DTName");

                DT_Encabezado.TableName = "Encabezado";
                DT_Encabezado.Rows.Add("01", "Datos Encabezado", "EE_Reports", "MCWebHogar.rptCompras.rdlc", "DT_DetalleCompras", "DT_DetalleCompras");

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
                    foreach (DataTable dt in dsReportecompras.Tables)
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
                string strFilePDF2 = "ReporteCompras.pdf";
                string strFilePathPDF2 = strCurrentDir2 + strFilePDF2;
                using (FileStream fs = new FileStream(strFilePathPDF2, FileMode.Create))
                {
                    fs.Write(bytes2, 0, bytes2.Length);
                }
                string direccion = "/GestionProveedores/ReportesTemp/" + strFilePDF2;
                string _open = "window.open('" + direccion + "'  , '_blank');desactivarloading();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", _open, true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}