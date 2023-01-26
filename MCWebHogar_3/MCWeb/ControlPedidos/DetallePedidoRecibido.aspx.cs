﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetallePedidoRecibido : System.Web.UI.Page
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
                    if (Session["IDRecibidoPedido"] == null)
                    {
                        Response.Redirect("PedidosRecibidos.aspx", true);
                    }
                    else
                    {
                        HDF_IDUsuario.Value = Session["Usuario"].ToString();
                        HDF_IDRecibidoPedido.Value = Session["IDRecibidoPedido"].ToString();                        
                        cargarDDLs();
                        cargarRecibidoPedido("");
                        cargarProductosRecibidoPedido();
                        ViewState["Ordenamiento"] = "ASC";
                        BTN_EditarRecibidoPedido.Visible = (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0);
                    }                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("CargarPedidoRecibido"))
                {
                    cargarRecibidoPedido("");
                    cargarProductosRecibidoPedido();
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
                else if (opcion.Contains("Identificacion"))
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

        protected void DDL_Impresoras_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string printer = DDL_Impresoras.SelectedValue;
            Session["Printer"] = printer;
            string script = "estilosElementosBloqueados();cerrarModalSeleccionarImpresora();alertifysuccess('Se ha seleccionado la impresora: " + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDDL_Impresoras_OnSelectedIndexChanged", script, true);
        }
        #endregion

        #region Recibido Pedido
        public void cargarRecibidoPedido(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDRecibidoPedido", HDF_IDRecibidoPedido.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarRecibidoPedidos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");
            
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
                        TXT_CodigoRecibidoPedido.Text = dr["NumeroRecibidoPedido"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        TXT_MontoPedido.Text = String.Format("{0:n}", dr["MontoPedidoRecibido"]);
                        TXT_EstadoRecibidoPedido.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaRecibidoPedido.Text = dr["FRecibidoPedido"].ToString().Trim();
                        TXT_HoraRecibidoPedido.Text = dr["HRecibidoPedido"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();
                        DDL_PuntoVenta.SelectedValue = dr["PuntoVentaID"].ToString().Trim();

                        HDF_EstadoRecibidoPedido.Value = dr["Estado"].ToString().Trim();
                        HDF_PedidoID.Value = dr["NumeroPedido"].ToString().Trim();

                        BTN_ConfirmarRecibidoPedido.Visible = HDF_EstadoRecibidoPedido.Value == "Revisión";
                        BTN_ConfirmarRecibidoPedido.Text = "Confirmar  pedido recibido # " + dr["ConsecutivoRecibidio"].ToString().Trim();

                        BTN_CompletarRecibidoPedido.Visible = HDF_EstadoRecibidoPedido.Value == "Revisión";
                        BTN_ActivarRecibidoPedido.Visible = HDF_EstadoRecibidoPedido.Value != "Revisión" && (ClasePermiso.Permiso("Editar", "Acciones", "Editar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0);

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

        protected void BTN_ConfirmarRecibidoPedido_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < DGV_ListaProductosRecibidoPedido.Rows.Count; index++)
            {
                TextBox cantidad = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                int cantidadRecibida = Convert.ToInt32(cantidad.Text.Trim());
                if (cantidadRecibida > 0)
                {
                    int IDRecibidoPedidoDetalle = Convert.ToInt32(DGV_ListaProductosRecibidoPedido.DataKeys[index].Value.ToString().Trim());
                    int IDRecibidoPedido = Convert.ToInt32(DGV_ListaProductosRecibidoPedido.DataKeys[index].Values[1].ToString().Trim());
                    int IDProducto = Convert.ToInt32(DGV_ListaProductosRecibidoPedido.DataKeys[index].Values[2].ToString().Trim());
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDRecibidoPedidoDetalle", IDRecibidoPedidoDetalle, SqlDbType.Int);
                    DT.DT1.Rows.Add("@RecibidoPedidoID", IDRecibidoPedido, SqlDbType.Int);
                    DT.DT1.Rows.Add("@ProductoID", IDProducto, SqlDbType.Int);
                    DT.DT1.Rows.Add("@CantidadRecibida", cantidadRecibida, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");                    
                }
            }

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDRecibidoPedido", HDF_IDRecibidoPedido.Value, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AumentarConsecutivoRecibido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");

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
                    cargarRecibidoPedido(script);
                    cargarProductosRecibidoPedido();
                }
            }
            else
            {
                string script = "desactivarloading();";
                cargarRecibidoPedido(script);
            }
        }

        protected void BTN_CompletarRecibidoPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDRecibidoPedido", HDF_IDRecibidoPedido.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmada", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarRecibidoPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarRecibidoPedido_Click", "alertifywarning('No se ha confirmado la orden de producción. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el pedido.');";
                    cargarRecibidoPedido(script);
                    cargarProductosRecibidoPedido();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el pedido.');";
                cargarRecibidoPedido(script);
            }
        }

        protected void BTN_ActivarRecibidoPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDRecibidoPedido", HDF_IDRecibidoPedido.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Revisión", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarRecibidoPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ActivarRecibidoPedido_Click", "alertifywarning('No se ha activado el pedido recibido. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el pedido recibido.');";
                    cargarRecibidoPedido(script);
                    cargarProductosRecibidoPedido();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el pedido recibido.');";
                cargarRecibidoPedido(script);
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

        protected void BTN_EditarRecibidoPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@RecibidoPedidoID", HDF_IDRecibidoPedido.Value, SqlDbType.VarChar);
            
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConsecutivosPedidoRecibido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");
            DGV_EditarConsecutivoPedidoRecibido.DataSource = Result;
            DGV_EditarConsecutivoPedidoRecibido.DataBind();

            UpdatePanel_ModalEditarPedidoRecibido.Update();
            UpdatePanel_ConsecutivoPedidoRecibido.Update();
            string script = "abrirModalEditarPedidoRecibido();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EditarRecibidoPedido_Click", script, true);
        }

        protected void DGV_EditarConsecutivoPedidoRecibido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int consecutivo = Convert.ToInt32(DGV_EditarConsecutivoPedidoRecibido.DataKeys[e.Row.RowIndex].Values[0].ToString());
                int idPedidoRecibido = Convert.ToInt32(DGV_EditarConsecutivoPedidoRecibido.DataKeys[e.Row.RowIndex].Values[1].ToString());
                int idPedido = Convert.ToInt32(DGV_EditarConsecutivoPedidoRecibido.DataKeys[e.Row.RowIndex].Values[2].ToString());
                DT.DT1.Clear();
                
                DT.DT1.Rows.Add("@Consecutivo", consecutivo, SqlDbType.Int);
                DT.DT1.Rows.Add("@RecibidoPedidoID", idPedidoRecibido, SqlDbType.Int);
                DT.DT1.Rows.Add("@PedidoID", idPedido, SqlDbType.Int);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosConsecutivo", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");
                GridView DGV_ListaProductos = e.Row.FindControl("DGV_ListaProductos") as GridView;
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
            }
        }
        #endregion

        #region Cargar Productos
        #region Productos RecibidoPedido
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosRecibidoPedido();
        }
        
        public void cargarProductosRecibidoPedido()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@RecibidoPedidoID", HDF_IDRecibidoPedido.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosRecibidoPedido.DataSource = Result;
                    DGV_ListaProductosRecibidoPedido.DataBind();
                    UpdatePanel_ListaProductosRecibidoPedido.Update();
                    DGV_DetallePedido.DataSource = Result;
                    DGV_DetallePedido.DataBind();
                    UpdatePanel_DetallePedido.Update();
                }
            }
            else
            {
                DGV_ListaProductosRecibidoPedido.DataSource = Result;
                DGV_ListaProductosRecibidoPedido.DataBind();
                UpdatePanel_ListaProductosRecibidoPedido.Update();
                DGV_DetallePedido.DataSource = Result;
                DGV_DetallePedido.DataBind();
                UpdatePanel_DetallePedido.Update();
            }
        }

        protected void DGV_ListaProductosRecibidoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (HDF_EstadoRecibidoPedido.Value != "Revisión")
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
        
        protected void DGV_ListaProductosRecibidoPedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                DropDownList ddlUnds = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                DropDownList ddlDecs = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Decenas") as DropDownList;

                if (e.CommandName == "minus")
                {
                    if (cantidadProducto > 0)
                    {
                        cantidadProducto--;
                    }
                }
                if (e.CommandName == "plus")
                {
                    if (cantidadProducto < 99)
                    {
                        cantidadProducto++;
                    }
                }
                int unds = Convert.ToInt32(cantidadProducto) % 10;
                int decs = Convert.ToInt32(cantidadProducto) / 10;
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                
            }
        }
        
        protected void DGV_ListaProductosRecibidoPedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@RecibidoPedidoID", HDF_IDRecibidoPedido.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");

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
                    DGV_ListaProductosRecibidoPedido.DataSource = Result;
                    DGV_ListaProductosRecibidoPedido.DataBind();
                    UpdatePanel_ListaProductosRecibidoPedido.Update();
                }
            }
            else
            {
                DGV_ListaProductosRecibidoPedido.DataSource = Result;
                DGV_ListaProductosRecibidoPedido.DataBind();
                UpdatePanel_ListaProductosRecibidoPedido.Update();
            }
        }

        protected void BTN_ImprimirPedidoRecibido_Click(object sender, EventArgs e)
        {
            cargarProductosRecibidoPedido();
            string printer = Session["Printer"].ToString().Trim();
            TXT_NombreImpresora.Text = printer;
            UpdatePanel_ModalDetallePedido.Update();
            string script = "abrirModalPedidoRecibido();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirOrdenProduccion_Click", script, true);
        }

        protected void BTN_ImprimirDetallePedido_Click(object sender, EventArgs e)
        {
            string sucursal = DDL_PuntoVenta.SelectedItem.Text;
            string printer = TXT_NombreImpresora.Text.Trim();
            string script = "estilosElementosBloqueados();imprimir('" + HDF_PedidoID.Value.ToString().Trim() +"', '" + sucursal + "', '" + printer + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaCategorias_RowCommand", script, true);
        }

        [WebMethod()]
        public static string BTN_ActualizarCantidad_Click(int idPedidoRecibidoDetalle, int cantidadProducto, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDRecibidoPedidoDetalle", idPedidoRecibidoDetalle, SqlDbType.Int);
            DT.DT1.Rows.Add("@CantidadRecibida", cantidadProducto, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarCantidad", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP13_0001");
            return "correcto";
        }
        #endregion
        #endregion
    }
}