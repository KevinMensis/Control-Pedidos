using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetalleDespacho : System.Web.UI.Page
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
                    if (Session["IDDespacho"] == null)
                    {
                        Response.Redirect("OrdenesProduccion.aspx", true);
                    }
                    else
                    {
                        HDF_IDDespacho.Value = Session["IDDespacho"].ToString();
                        cargarDDLs();
                        cargarDespacho("");
                        cargarProductosDespacho();
                        ViewState["Ordenamiento"] = "ASC";
                    }                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];                
                if (opcion.Contains("TXT_Buscar"))
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
            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVentaDespacho", SqlDbType.VarChar);

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
            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidosDespacho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                LB_Pedido.DataSource = Result;
                LB_Pedido.DataTextField = "NumeroPedido";
                LB_Pedido.DataValueField = "PedidoID";
                LB_Pedido.DataBind();
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

        #region Despacho
        public void cargarDespacho(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarDespachos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");
            
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
                        TXT_CodigoDespacho.Text = dr["NumeroDespacho"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_MontoDespacho.Text = String.Format("{0:n}", dr["MontoDespacho"]);
                        TXT_EstadoDespacho.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaDespacho.Text = dr["FDespacho"].ToString().Trim();
                        TXT_HoraDespacho.Text = dr["HDespacho"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();

                        HDF_EstadoDespacho.Value = dr["Estado"].ToString().Trim();

                        BTN_ConfirmarDespacho.Visible = HDF_EstadoDespacho.Value == "Preparación";
                        BTN_ConfirmarDespacho.Text = "Confirmar despacho # " + dr["ConsecutivoDespacho"].ToString().Trim();

                        BTN_CompletarDespacho.Visible = HDF_EstadoDespacho.Value == "Preparación";
                        
                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();

                        string script = "estilosElementosBloqueados();cargarFiltros();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedido", script, true);
                    }
                }
            }
        }

        protected void BTN_ConfirmarDespacho_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < DGV_ListaPedidosDespacho.Rows.Count; index++)
            {
                GridView gridListaProductos = DGV_ListaPedidosDespacho.Rows[index].FindControl("DGV_ListaProductos") as GridView;

                for (int i = 0; i < gridListaProductos.Rows.Count; i++)
                {
                    TextBox cantidad = gridListaProductos.Rows[i].FindControl("TXT_Cantidad") as TextBox;
                    int cantidadADespachar = Convert.ToInt32(cantidad.Text.Trim().Replace(",", ""));
                    if (cantidadADespachar > 0)
                    {
                        int idDetalleDespacho = Convert.ToInt32(gridListaProductos.DataKeys[i].Value.ToString().Trim());
                        int idProducto = Convert.ToInt32(gridListaProductos.DataKeys[i].Values[1].ToString().Trim());
                        int idPedido = Convert.ToInt32(gridListaProductos.DataKeys[i].Values[2].ToString().Trim());
                        int idDespacho = Convert.ToInt32(gridListaProductos.DataKeys[i].Values[3].ToString().Trim());
                        DT.DT1.Clear();

                        DT.DT1.Rows.Add("@IDDespachoDetalle", idDetalleDespacho, SqlDbType.Int);
                        DT.DT1.Rows.Add("@DespachoID", idDespacho, SqlDbType.Int);
                        DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.Int);
                        DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.Int);
                        DT.DT1.Rows.Add("@CantidadDespachada", cantidadADespachar, SqlDbType.Int);

                        DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                        DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

                        Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP11_0001");

                        DT.DT1.Clear();

                        DT.DT1.Rows.Add("@ProductoID", idProducto, SqlDbType.Int);
                        DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.Int);
                        DT.DT1.Rows.Add("@CantidadDespachada", cantidadADespachar, SqlDbType.Int);

                        DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                        DT.DT1.Rows.Add("@TipoSentencia", "UpdateDespacho", SqlDbType.VarChar);

                        Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");
                    }
                }                
            }

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AumentarConsecutivoDespacho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarDespacho_Click", "alertifywarning('No se ha confirmado el despacho. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "desactivarloading();";
                    cargarDespacho(script);
                    cargarProductosDespacho();
                }
            }
            else
            {
                string script = "desactivarloading();";
                cargarDespacho(script);
            }
        }

        protected void BTN_CompletarDespacho_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarDespacho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarRecibidoPedido_Click", "alertifywarning('No se ha confirmado la orden de producción. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el despacho.');";
                    cargarDespacho(script);
                    cargarProductosDespacho();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el despacho.');";
                cargarDespacho(script);
            }
        }

        protected void DDL_Reportes_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    switch (Convert.ToInt32(DDL_Reportes.SelectedValue))
        //    {
        //        case 1:
        //            ReporteOrdenProduccion();
        //            break;
        //        case 2:
        //            DescargarOrdenProduccion();
        //            break;
        //        default:
        //            break;
        //    }
        //    DDL_Reportes.SelectedValue = "0";
        //    UpdatePanel_Header.Update();
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Reportes_SelectedIndexChanged", "desactivarloading();estilosElementosBloqueados();cargarFiltros();", true);
        }
        #endregion

        #region Cargar Productos
        #region Productos Despacho
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosDespacho();
        }
        
        private void cargarProductosDespacho()
        {
            DT.DT1.Clear();

            string puntosVenta = "";
            string pedidos = "";

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

            #region Pedidos
            foreach (ListItem l in LB_Pedido.Items)
            {
                if (l.Selected)
                {
                    pedidos += "'" + l.Value + "',";
                }
            }
            pedidos = pedidos.TrimEnd(',');
            if (pedidos != "")
            {
                DT.DT1.Rows.Add("@FiltrarPedidos", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroPedidos", pedidos, SqlDbType.VarChar);
            }
            #endregion

            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaPedidosDespacho.DataSource = Result;
                    DGV_ListaPedidosDespacho.DataBind();
                    UpdatePanel_ListaPedidosDespacho.Update();                    
                    string script = "estilosElementosBloqueados();cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDespacho", script, true);
                    UpdatePanel_FiltrosProductos.Update();
                }
            }
            else
            {
                DGV_ListaPedidosDespacho.DataSource = Result;
                DGV_ListaPedidosDespacho.DataBind();
                UpdatePanel_ListaPedidosDespacho.Update(); 
                string script = "estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDespacho", script, true);
                UpdatePanel_FiltrosProductos.Update();
            }
        }

        protected void DGV_ListaPedidosDespacho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idPedido = Convert.ToInt32(DGV_ListaPedidosDespacho.DataKeys[e.Row.RowIndex].Values[1].ToString());
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@DespachoID", HDF_IDDespacho.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosPedido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP11_0001");
                GridView DGV_ListaProductos = e.Row.FindControl("DGV_ListaProductos") as GridView;
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();

                if (HDF_EstadoDespacho.Value == "Confirmado")
                {
                    DGV_ListaProductos.Enabled = false;
                }
            }
        }
        
        protected void DGV_ListaPedidosDespacho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Imprimir")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idPedido = Convert.ToInt32(DGV_ListaPedidosDespacho.DataKeys[index].Values[1].ToString());
                HDF_Pedido.Value = DGV_ListaPedidosDespacho.DataKeys[index].Values[3].ToString();
                HDF_PuntoVenta.Value = DGV_ListaPedidosDespacho.DataKeys[index].Values[4].ToString();
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@DespachoID", HDF_IDDespacho.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosPedido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP11_0001");

                int montoDespacho = 0;
                if (Result.Rows.Count > 0)
                {
                    for (int i = 0; i < Result.Rows.Count; i++)
                    {
                        int cantidad = Convert.ToInt32(Result.Rows[i]["CantidadDespachada"].ToString());
                        int precioUnitario = Convert.ToInt32(Result.Rows[i]["PrecioProducto"].ToString());
                        montoDespacho += cantidad * precioUnitario;
                    }
                }
                HDF_MontoDespacho.Value = string.Format("{0:n0}", montoDespacho);

                DT.DT1.Clear();
                DT.DT1.Rows.Add("@DespachoID", HDF_IDDespacho.Value, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarConsecutivosDespacho", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP11_0001");
                DGV_ConsecutivoDespacho.DataSource = Result;
                DGV_ConsecutivoDespacho.DataBind();

                string printer = Session["Printer"].ToString().Trim();
                TXT_NombreImpresora.Text = printer;
                UpdatePanel_DetallePedido.Update();
                UpdatePanel_ModalDetallePedido.Update();
                string script = "abrirModalDetallePedido();estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirDetallePedido_Click", script, true);
            }
        }
       
        protected void DGV_ListaPedidosDespacho_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDespacho", HDF_IDDespacho.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

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
                    DGV_ListaPedidosDespacho.DataSource = Result;
                    DGV_ListaPedidosDespacho.DataBind();
                    UpdatePanel_ListaPedidosDespacho.Update();
                }
            }
            else
            {
                DGV_ListaPedidosDespacho.DataSource = Result;
                DGV_ListaPedidosDespacho.DataBind();
                UpdatePanel_ListaPedidosDespacho.Update();
            }
        }

        protected void DGV_ConsecutivoDespacho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int consecutivo = Convert.ToInt32(DGV_ConsecutivoDespacho.DataKeys[e.Row.RowIndex].Values[0].ToString());
                int idDespacho = Convert.ToInt32(DGV_ConsecutivoDespacho.DataKeys[e.Row.RowIndex].Values[1].ToString());
                int idPedido = Convert.ToInt32(DGV_ConsecutivoDespacho.DataKeys[e.Row.RowIndex].Values[2].ToString());
                DT.DT1.Clear();
                DT.DT1.Rows.Add("@Consecutivo", consecutivo, SqlDbType.Int);
                DT.DT1.Rows.Add("@DespachoID", idDespacho, SqlDbType.Int);
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.Int);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosConsecutivo", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP11_0001");
                GridView DGV_ListaProductos = e.Row.FindControl("DGV_ListaProductos") as GridView;
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
            }
        }

        protected void DGV_ConsecutivoDespacho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "imprimir")
                {
                    string sucursal = HDF_PuntoVenta.Value;
                    string pedido = HDF_Pedido.Value;
                    decimal monto = Convert.ToDecimal(DGV_ConsecutivoDespacho.DataKeys[index].Values[3].ToString().Trim());
                    string montoDespacho = string.Format("{0:n0}", monto);
                    string printer = TXT_NombreImpresora.Text.Trim();
                    string script = "estilosElementosBloqueados();imprimir('" + montoDespacho + "', '" + sucursal + "', '" + pedido + "', " + index + ", '" + printer + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ConsecutivoDespacho_RowCommand", script, true);
                }
            }
        }
        #endregion
        #endregion
    }
}