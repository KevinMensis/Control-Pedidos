using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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
                        HDF_IDRecibidoPedido.Value = Session["IDRecibidoPedido"].ToString();                        
                        cargarDDLs();
                        cargarRecibidoPedido("");
                        cargarProductosRecibidoPedido();
                        ViewState["Ordenamiento"] = "ASC";
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
                        TXT_EstadoRecibidoPedido.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaRecibidoPedido.Text = dr["FRecibidoPedido"].ToString().Trim();
                        TXT_HoraRecibidoPedido.Text = dr["HRecibidoPedido"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();
                        DDL_PuntoVenta.SelectedValue = dr["PuntoVentaID"].ToString().Trim();

                        HDF_EstadoRecibidoPedido.Value = dr["Estado"].ToString().Trim();

                        BTN_ConfirmarRecibidoPedido.Visible = HDF_EstadoRecibidoPedido.Value == "Revisión";
                        BTN_ConfirmarRecibidoPedido.Text = "Confirmar  pedido recibido # " + dr["ConsecutivoRecibidio"].ToString().Trim();

                        BTN_CompletarRecibidoPedido.Visible = HDF_EstadoRecibidoPedido.Value == "Revisión";

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
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDRecibidoPedidoDetalle", IDRecibidoPedidoDetalle, SqlDbType.Int);
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
                    string script = "";
                    cargarRecibidoPedido(script);
                    cargarProductosRecibidoPedido();
                }
            }
            else
            {
                string script = "";
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
                }
            }
            else
            {
                DGV_ListaProductosRecibidoPedido.DataSource = Result;
                DGV_ListaProductosRecibidoPedido.DataBind();
                UpdatePanel_ListaProductosRecibidoPedido.Update();
            }
        }

        protected void DGV_ListaProductosRecibidoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (HDF_EstadoRecibidoPedido.Value != "Revisión")
                {                    
                    cantidad.Enabled = false;
                    cantidad.CssClass = "form-control";
                    ddlUnds.Enabled = false;
                    ddlUnds.CssClass = "form-control";
                    ddlDecs.Enabled = false;
                    ddlDecs.CssClass = "form-control";
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
        
        protected void TXT_Cantidad_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            TextBox cantidad = sender as TextBox;            
            DropDownList ddlUnds = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Decenas") as DropDownList;

            if (cantidad.Text != "")
            {
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                int unds = Convert.ToInt32(cantidadProducto) % 10;
                int decs = Convert.ToInt32(cantidadProducto) / 10;
                if (cantidadProducto > 0 && cantidadProducto < 99)
                {
                    ddlUnds.SelectedValue = unds.ToString();
                    ddlDecs.SelectedValue = decs.ToString();
                    cantidad.Text = cantidadProducto.ToString();
                }
                else
                {
                    unds = Convert.ToInt32(ddlUnds.SelectedValue);
                    decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
                    cantidadProducto = decs + unds;
                    cantidad.Text = cantidadProducto.ToString();
                }
            }
            else
            {
                cantidad.Text = "0";
                ddlUnds.SelectedValue = "0";
                ddlDecs.SelectedValue = "0";
            }
            UpdatePanel_ListaProductosRecibidoPedido.Update();
            string script = "estilosElementosBloqueados();enterCantidad(" + index + ");";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_Cantidad_OnTextChanged", script, true);
        }

        protected void DDL_DecenasUnidades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            DropDownList ddlUnds = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosRecibidoPedido.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            decimal cantidadProducto = decs + unds;
            cantidad.Text = cantidadProducto.ToString();
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
        #endregion
        #endregion
    }
}