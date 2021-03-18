﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class DetalleDevolucion : System.Web.UI.Page
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
                    if (Session["IDDevolucion"] == null)
                    {
                        Response.Redirect("Devolucion.aspx", true);
                    }
                    else
                    {
                        HDF_IDDevolucion.Value = Session["IDDevolucion"].ToString();                        
                        cargarDDLs();
                        cargarDevolucion("");
                        cargarProductosDevolucion();
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

        #region Devolucion
        public void cargarDevolucion(string ejecutar)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDevolucion", HDF_IDDevolucion.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarDevoluciones", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP18_0001");
            
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
                        TXT_CodigoDevolucion.Text = dr["NumeroDevolucion"].ToString().Trim(); ;
                        TXT_TotalProductos.Text = dr["CantidadProductos"].ToString().Trim();
                        // TXT_EstadoDevolucion.Text = dr["Estado"].ToString().Trim();
                        TXT_FechaDevolucion.Text = dr["FDevolucion"].ToString().Trim();
                        TXT_HoraDevolucion.Text = dr["HDevolucion"].ToString().Trim();
                        DDL_Propietario.SelectedValue = dr["UsuarioID"].ToString().Trim();

                        // HDF_EstadoDevolucion.Value = dr["Estado"].ToString().Trim();

                        // BTN_ConfirmarDevolucion.Visible = HDF_EstadoDevolucion.Value != "Confirmado";
                        
                        LBL_CreadoPor.Text = "Ingresado por: " + dr["QuienIngreso"].ToString().Trim() + ", " + dr["FIngreso"];
                        if (dr["QuienModifico"].ToString().Trim() == "" || dr["FModifico"].ToString().Trim() == "01/01/1900")
                            LBL_UltimaModificacion.Text = "";
                        else
                            LBL_UltimaModificacion.Text = "Última modificación por: " + dr["QuienModifico"].ToString().Trim() + ", " + dr["FModifico"];
                        UpdatePanel_Header.Update();

                        string script = "estilosElementosBloqueados();" + ejecutar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDevolucion", script, true);
                    }
                }
            }
        }

        protected void BTN_ConfirmarDevolucion_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDDevolucion", HDF_IDDevolucion.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Estado", "Confirmado", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ConfirmarDevolucion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP18_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarDevolucion_Click", "alertifywarning('No se ha confirmado el Devolucion. Error: " + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    string script = "alertifysuccess('Se ha confirmado el Devolucion.');";
                    cargarDevolucion(script);
                    cargarProductosDevolucion();
                }
            }
            else
            {
                string script = "alertifywarning('No se ha confirmado el Devolucion.');";
                cargarDevolucion(script);
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
            DT.DT1.Rows.Add("@DevolucionID", HDF_IDDevolucion.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosDevolucion", SqlDbType.VarChar);

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
            DT.DT1.Rows.Add("@DevolucionID", HDF_IDDevolucion.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosDevolucion", SqlDbType.VarChar);

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

                DT.DT1.Rows.Add("@DevolucionID", HDF_IDDevolucion.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserID"].ToString().Trim(), SqlDbType.Int);
                DT.DT1.Rows.Add("@PuntoVentaID", DDL_PuntoVenta.SelectedValue, SqlDbType.Int);
                DT.DT1.Rows.Add("@ListaProductos", productos, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "AgregarProductos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP19_0001");

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
                        cargarProductosDevolucion();
                        cargarProductosSinAsignar();
                        cargarDevolucion(script);
                    }
                }
                else
                {
                    cargarProductosDevolucion();
                    cargarProductosSinAsignar();
                    cargarDevolucion("");
                }
            }
        }
        #endregion

        #region Productos Devolucion
        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarProductosDevolucion();
        }
        
        public void cargarProductosDevolucion()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@DevolucionID", HDF_IDDevolucion.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP19_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductosDevolucion.DataSource = Result;
                    DGV_ListaProductosDevolucion.DataBind();
                    UpdatePanel_ListaProductosDevolucion.Update();
                    string script = "estilosElementosBloqueados();cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDevolucion", script, true);
                }
            }
            else
            {
                DGV_ListaProductosDevolucion.DataSource = Result;
                DGV_ListaProductosDevolucion.DataBind();
                UpdatePanel_ListaProductosDevolucion.Update();
                string script = "estilosElementosBloqueados();cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDevolucion", script, true);
            }
        }

        protected void DGV_ListaProductosDevolucion_RowDataBound(object sender, GridViewRowEventArgs e)
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
        
        protected void DGV_ListaProductosDevolucion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                
                TextBox cantidad = DGV_ListaProductosDevolucion.Rows[index].FindControl("TXT_Cantidad") as TextBox;
                decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
                DropDownList ddlUnds = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Unidades") as DropDownList;
                DropDownList ddlDecs = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Decenas") as DropDownList;
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
                    guardarProductoDevolucion(index);
                
            }
        }
        
        protected void TXT_Cantidad_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            TextBox cantidad = sender as TextBox;            
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            DropDownList ddlUnds = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            int unds = Convert.ToInt32(cantidadProducto) % 10;
            int decs = Convert.ToInt32(cantidadProducto) / 10;
            if (cantidadProducto > 0 && cantidadProducto < 99)
            {                                 
                ddlUnds.SelectedValue = unds.ToString();
                ddlDecs.SelectedValue = decs.ToString();
                cantidad.Text = cantidadProducto.ToString();
                guardarProductoDevolucion(index);
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
            DropDownList ddlUnds = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Unidades") as DropDownList;
            DropDownList ddlDecs = DGV_ListaProductosDevolucion.Rows[index].FindControl("DDL_Decenas") as DropDownList;
            TextBox cantidad = DGV_ListaProductosDevolucion.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            int unds = Convert.ToInt32(ddlUnds.SelectedValue);
            int decs = Convert.ToInt32(ddlDecs.SelectedValue) * 10;
            decimal cantidadProducto = decs + unds;
            cantidad.Text = cantidadProducto.ToString();
            guardarProductoDevolucion(index);
        }

        private void guardarProductoDevolucion(int index)
        {
            TextBox cantidad = DGV_ListaProductosDevolucion.Rows[index].FindControl("TXT_Cantidad") as TextBox;
            decimal cantidadProducto = (Convert.ToDecimal(cantidad.Text));
            string IDDevolucionDetalle = DGV_ListaProductosDevolucion.DataKeys[index].Value.ToString().Trim();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDDevolucionDetalle", IDDevolucionDetalle, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadDevolucion", cantidadProducto, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "UpdateProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP19_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    cargarProductosDevolucion();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptguardarProductoPedido", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                }
                else
                {
                    cargarDevolucion("");
                }
            }
        }
        
        protected void DGV_ListaProductosDevolucion_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@DevolucionID", HDF_IDDevolucion.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP19_0001");

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
                    DGV_ListaProductosDevolucion.DataSource = Result;
                    DGV_ListaProductosDevolucion.DataBind();
                    UpdatePanel_ListaProductosDevolucion.Update();
                }
            }
            else
            {
                DGV_ListaProductosDevolucion.DataSource = Result;
                DGV_ListaProductosDevolucion.DataBind();
                UpdatePanel_ListaProductosDevolucion.Update();
            }
        }        
        #endregion
        #endregion
    }
}