using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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
                        cargarDDLs();
                        cargarODP("");
                        cargarProductosODP();
                        cargarProductosODPImprimir();
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
        }
        #endregion

        #region Orden de Producción
        public void cargarODP(string ejecutar)
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
                        TXT_CostoODP.Text = dr["CostoODP"].ToString().Trim();
                        TXT_EstadoODP.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaODP.Text = dr["FODP"].ToString().Trim();
                        TXT_HoraODP.Text = dr["HODP"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();
                        DDL_PlantaProduccion.SelectedValue = dr["PlantaProduccionID"].ToString().Trim();

                        HDF_EstadoODP.Value = dr["Estado"].ToString().Trim();

                        BTN_ConfirmarODP.Visible = HDF_EstadoODP.Value == "Solicitada";
                        BTN_ImprimirOrdenProduccion.Visible = HDF_EstadoODP.Value == "Solicitada";
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
        #endregion

        #region Cargar Productos        
        #region Productos Pedido
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosODP();
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

        private void cargarProductosODPImprimir()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@ODPID", HDF_IDODP.Value, SqlDbType.VarChar);

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

        protected void DGV_ListaProductosODP_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (HDF_EstadoODP.Value != "Confirmada")
                {
                    Button minus = (Button)e.Row.FindControl("BTN_Minus");
                    Button plus = (Button)e.Row.FindControl("BTN_Plus");

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
        
        protected void TXT_Cantidad_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            TextBox cantidad = sender as TextBox;            
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            DropDownList ddlUnds = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            int unds = Convert.ToInt32(cantidadProducto) % 10;
            int decs = Convert.ToInt32(cantidadProducto) / 10;
            if (cantidadProducto > 0 && cantidadProducto < 99)
            {                                 
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                guardarProductoODP(index);
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
            DropDownList ddlUnds = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosODP.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosODP.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            decimal cantidadProducto = decs + unds;
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

        protected void BTN_ImprimirOrdenProduccion_Click(object sender, EventArgs e)
        {
            string script = "abrirModalOrdenProduccion();estilosElementosBloqueados();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ImprimirOrdenProduccion_Click", script, true);
        }
        #endregion
        #endregion
    }
}