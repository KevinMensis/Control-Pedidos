﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetalleDesecho : System.Web.UI.Page
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
                    if (Session["IDDesecho"] == null)
                    {
                        Response.Redirect("Desecho.aspx", true);
                    }
                    else
                    {
                        HDF_IDDesecho.Value = Session["IDDesecho"].ToString();                        
                        cargarDDLs();
                        cargarDesecho("");
                        cargarProductosDesecho();
                        cargarProductosSinAsignar();
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

        #region Desecho
        public void cargarDesecho(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDesecho", HDF_IDDesecho.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarDesechos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP16_0001");
            
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
                        TXT_CodigoDesecho.Text = dr["NumeroDesecho"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        // TXT_EstadoDesecho.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaDesecho.Text = dr["FDesecho"].ToString().Trim();
                        TXT_HoraDesecho.Text = dr["HDesecho"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();

                        // HDF_EstadoDesecho.Value = dr["Estado"].ToString().Trim();

                        // BTN_ConfirmarDesecho.Visible = HDF_EstadoDesecho.Value != "Confirmado";
                        
                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();

                        string script = "estilosElementosBloqueados();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDesecho", script, true);
                    }
                }
            }
        }

        protected void BTN_ConfirmarDesecho_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDDesecho", HDF_IDDesecho.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarDesecho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP16_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarDesecho_Click", "alertifywarning('No se ha confirmado el Desecho. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el Desecho.');";
                    cargarDesecho(script);
                    cargarProductosDesecho();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el Desecho.');";
                cargarDesecho(script);
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
            DT.DT1.Rows.Add("@DesechoID", HDF_IDDesecho.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosDesecho", SqlDbType.VarChar);

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
            DT.DT1.Rows.Add("@DesechoID", HDF_IDDesecho.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosDesecho", SqlDbType.VarChar);

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

                DT.DT1.Rows.Add("@DesechoID", HDF_IDDesecho.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserID"].ToString().Trim(), SqlDbType.Int);
                DT.DT1.Rows.Add("@PuntoVentaID", DDL_PuntoVenta.SelectedValue, SqlDbType.Int);
                DT.DT1.Rows.Add("@ListaProductos", productos, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "AgregarProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP17_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_Agregar_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        string script = "cerrarModalAgregarProductos();alertifysuccess('Productos agregados con éxito.');";
                        cargarProductosDesecho();
                        cargarProductosSinAsignar();
                        cargarDesecho(script);
                    }
                }
                else
                {
                    cargarProductosDesecho();
                    cargarProductosSinAsignar();
                    cargarDesecho("");
                }
            }
        }
        #endregion

        #region Productos Desecho
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosDesecho();
        }
        
        public void cargarProductosDesecho()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@DesechoID", HDF_IDDesecho.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP17_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosDesecho.DataSource = Result;
                    DGV_ListaProductosDesecho.DataBind();
                    UpdatePanel_ListaProductosDesecho.Update();
                    string script = "estilosElementosBloqueados();cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDesecho", script, true);
                }
            }
            else
            {
                DGV_ListaProductosDesecho.DataSource = Result;
                DGV_ListaProductosDesecho.DataBind();
                UpdatePanel_ListaProductosDesecho.Update();
                string script = "estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDesecho", script, true);
            }
        }

        protected void DGV_ListaProductosDesecho_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }
        
        protected void DGV_ListaProductosDesecho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosDesecho.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                DropDownList ddlUnds = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                DropDownList ddlDecs = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Decenas") as DropDownList;
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
                    guardarProductoDesecho(index);
                
            }
        }
        
        protected void TXT_Cantidad_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            TextBox cantidad = sender as TextBox;            
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            DropDownList ddlUnds = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            int unds = Convert.ToInt32(cantidadProducto) % 10;
            int decs = Convert.ToInt32(cantidadProducto) / 10;
            if (cantidadProducto > 0 && cantidadProducto < 99)
            {                                 
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                guardarProductoDesecho(index);
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
            DropDownList ddlUnds = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosDesecho.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosDesecho.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            decimal cantidadProducto = decs + unds;
            cantidad.Text = cantidadProducto.ToString();
            guardarProductoDesecho(index);
        }

        private void guardarProductoDesecho(int index)
        {
            TextBox cantidad = DGV_ListaProductosDesecho.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDDesechoDetalle = DGV_ListaProductosDesecho.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDesechoDetalle", IDDesechoDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadDesecho", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP17_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosDesecho();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarDesecho("");
                }
            }
        }
        
        protected void DGV_ListaProductosDesecho_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@DesechoID", HDF_IDDesecho.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP17_0001");

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
                    DGV_ListaProductosDesecho.DataSource = Result;
                    DGV_ListaProductosDesecho.DataBind();
                    UpdatePanel_ListaProductosDesecho.Update();
                }
            }
            else
            {
                DGV_ListaProductosDesecho.DataSource = Result;
                DGV_ListaProductosDesecho.DataBind();
                UpdatePanel_ListaProductosDesecho.Update();
            }
        }        
        #endregion
        #endregion
    }
}