using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Pedido : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Remove("IDPedido");
            if (!Page.IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("../Default.aspx", true);
                }
                else
                {
                    cargarPedido("");
                    cargarDDLs();
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        protected void BTN_CrearPedidos_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptAbrirModalCrearPedido", "abrirModalCrearPedido();", true);
            return;
        }

        private void cargarDDLs()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarUsuarios", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_Propietario.DataSource = Result;
                DDL_Propietario.DataTextField = "Nombre";
                DDL_Propietario.DataValueField = "IDUsuario";
                DDL_Propietario.DataBind();

                DataView dv = new DataView(Result);
                dv.RowFilter = "IDUsuario <> 0";
                LB_Solicitante.DataSource = dv;
                LB_Solicitante.DataTextField = "Nombre";
                LB_Solicitante.DataValueField = "IDUsuario";
                LB_Solicitante.DataBind();
            }

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", "kpicado", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPlantasProduccion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_PlantaProduccion.DataSource = Result;
                DDL_PlantaProduccion.DataTextField = "DescripcionPlantaProduccion";
                DDL_PlantaProduccion.DataValueField = "IDPlantaProduccion";
                DDL_PlantaProduccion.DataBind();

                DataView dv = new DataView(Result);
                dv.RowFilter = "IDPlantaProduccion <> 0";
                LB_PlantaProduccion.DataSource = dv;
                LB_PlantaProduccion.DataTextField = "DescripcionPlantaProduccion";
                LB_PlantaProduccion.DataValueField = "IDPlantaProduccion";
                LB_PlantaProduccion.DataBind();
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

                DataView dv = new DataView(Result);
                dv.RowFilter = "IDPuntoVenta <> 0";
                LB_Sucursal.DataSource = dv;
                LB_Sucursal.DataTextField = "DescripcionPuntoVenta";
                LB_Sucursal.DataValueField = "IDPuntoVenta";
                LB_Sucursal.DataBind();
            }

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEstadosPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {                
                LB_Estado.DataSource = Result;
                LB_Estado.DataTextField = "Estado";
                LB_Estado.DataValueField = "Estado";
                LB_Estado.DataBind();
            }

            TXT_FechaCreacionDesde.Text = "1900-01-01";
            TXT_FechaCreacionHasta.Text = "1900-01-01";
            TXT_FechaPedidoDesde.Text = "1900-01-01";
            TXT_FechaPedidoHasta.Text = "1900-01-01";

            UpdatePanel_FiltrosPedidos.Update();
        }
        #endregion

        #region Pedidos
        private DataTable cargarPedidoConsulta()
        {
            DT.DT1.Clear();
            string puntosVenta = "";
            string puntosVentaText = "";
            string plantaProduccion = "";
            string plantaProduccionText = "";
            string estados = "";
            string estadosText = "";
            string usuarios = "";
            string usuariosText = "";

            LBL_Filtro.Text = "Filtros: ";

            #region Puntos Venta
            foreach (ListItem l in LB_Sucursal.Items)
            {
                if (l.Selected)
                {
                    puntosVenta += "'" + l.Value + "',";
                    puntosVentaText += l.Text + ",";
                }
            }
            puntosVenta = puntosVenta.TrimEnd(',');
            puntosVentaText = puntosVentaText.TrimEnd(',');
            if (puntosVentaText != "")
            {
                LBL_Filtro.Text += " Sucursales=" + puntosVentaText + "; ";
                DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroSucursales", puntosVenta, SqlDbType.VarChar);
            }
            #endregion

            #region Plantas Produccion
            foreach (ListItem l in LB_PlantaProduccion.Items)
            {
                if (l.Selected)
                {
                    plantaProduccion += "'" + l.Value + "',";
                    plantaProduccionText += l.Text + ",";
                }
            }
            plantaProduccion = plantaProduccion.TrimEnd(',');
            plantaProduccionText = plantaProduccionText.TrimEnd(',');
            if (plantaProduccionText != "")
            {
                LBL_Filtro.Text += " Plantas Producción=" + puntosVentaText + "; ";
                DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltrpPlantasProduccion", plantaProduccion, SqlDbType.VarChar);
            }
            #endregion

            #region Estado
            foreach (ListItem l in LB_Estado.Items)
            {
                if (l.Selected)
                {
                    estados += "'" + l.Value + "',";
                    estadosText += l.Text + ",";
                }
            }
            estados = estados.TrimEnd(',');
            estadosText = estadosText.TrimEnd(',');
            if (estadosText != "")
            {
                LBL_Filtro.Text += " Estados=" + estadosText + "; ";
                DT.DT1.Rows.Add("@FiltrarEstados", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroEstados", estados, SqlDbType.VarChar);
            }
            #endregion

            #region Usuarios
            foreach (ListItem l in LB_Solicitante.Items)
            {
                if (l.Selected)
                {
                    usuarios += "'" + l.Value + "',";
                    usuariosText += l.Text + ",";
                }
            }
            usuarios = usuarios.TrimEnd(',');
            usuariosText = usuariosText.TrimEnd(',');
            if (usuariosText != "")
            {
                LBL_Filtro.Text += " Usuarios=" + usuariosText + "; ";
                DT.DT1.Rows.Add("@FiltrarUsuarios", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroUsuarios", usuarios, SqlDbType.VarChar);
            }
            #endregion

            if (LBL_Filtro.Text == "Filtros: ")
            {
                LBL_Filtro.Text += "Ninguno;";
            }

            UpdatePanel_FiltrosPedidos.Update();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPedidos", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");
        }

        private void cargarPedido(string ejecutar)
        {
            Result = cargarPedidoConsulta();
            
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaPedidos.DataSource = Result;
                    DGV_ListaPedidos.DataBind();
                    UpdatePanel_ListaPedidos.Update();
                    string script = "cargarFiltros();" + ejecutar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedido", script, true);

                }
            }
            else
            {
                DGV_ListaPedidos.DataSource = Result;
                DGV_ListaPedidos.DataBind();
                UpdatePanel_ListaPedidos.Update();
                string script = "cargarFiltros()";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedido", script, true);
            }
        }

        protected void TXT_FiltrarPedidos_OnTextChanged(object sender, EventArgs e)
        {
            cargarPedido("");
        }

        protected void BTN_CrearPedido_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            int solicitante = Convert.ToInt32(DDL_Propietario.SelectedValue);
            int plantaProduccion = Convert.ToInt32(DDL_PlantaProduccion.SelectedValue);
            int puntoVenta = Convert.ToInt32(DDL_PuntoVenta.SelectedValue);

            DT.DT1.Rows.Add("@UsuarioID", solicitante, SqlDbType.Int);
            DT.DT1.Rows.Add("@PlantaProduccionID", plantaProduccion, SqlDbType.Int);
            DT.DT1.Rows.Add("@PuntoVentaID", puntoVenta, SqlDbType.Int);
            DT.DT1.Rows.Add("@DescripcionPedido", TXT_DescripcionPedido.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearPedido", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearPedido_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    Session["IDPedido"] = Result.Rows[0][1].ToString().Trim();
                    Response.Redirect("DetallePedido.aspx", true);
                }
            }
            else
            {
                string script = "cerrarModalCrearPedido();alertifywarning('No se ha podido crear el pedido.');";
                cargarPedido(script);
            }
        }

        protected void DGV_ListaPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idPedido = DGV_ListaPedidos.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDPedido"] = idPedido;
                    Response.Redirect("DetallePedido.aspx", true);
                }                
            }
        }

        protected void DGV_ListaPedidos_Sorting(object sender, GridViewSortEventArgs e)
        {   
            Result = cargarPedidoConsulta();
            
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
                    DGV_ListaPedidos.DataSource = Result;
                    DGV_ListaPedidos.DataBind();
                    UpdatePanel_ListaPedidos.Update();
                    string script = "cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaPedidos_Sorting", script, true);
                }
            }
            else
            {
                DGV_ListaPedidos.DataSource = Result;
                DGV_ListaPedidos.DataBind();
                UpdatePanel_ListaPedidos.Update();
                string script = "cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaPedidos_Sorting", script, true);
            }
        }

        protected void DGV_ListaPedidos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                string estado = rowView["Estado"].ToString().Trim();

                if (estado != "Confirmado")
                {
                    CheckBox chkSeleccionar = (CheckBox)e.Row.FindControl("seleccionarCheckBox");
                    chkSeleccionar.Visible = false;
                }
            }
        }
        #endregion

        #region Ordenes de Produccion
        protected void BTN_CrearOrdenes_Click(object sender, EventArgs e)
        {
            int contador = 0;
            string fila = "";
            string plantaProduccionID = "";

            foreach (GridViewRow row in DGV_ListaPedidos.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("seleccionarCheckBox");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        fila += row.RowIndex + ",";
                        if (plantaProduccionID == "")
                        {
                            plantaProduccionID = DGV_ListaPedidos.DataKeys[row.RowIndex].Values[3].ToString().Trim();
                        }
                        else if (plantaProduccionID != DGV_ListaPedidos.DataKeys[row.RowIndex].Values[3].ToString().Trim()) 
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptSeleccionarBTN_CrearOrdenes_Click", "alertifywarning('Debe seleccionar solo pedios para la misma Planta de Producción.');", true);
                            return;
                        }
                        contador++;
                    }
                }
            }
            if (fila != "" && contador > 0)
            {
                fila = fila.TrimEnd(',');
                string[] listaFilas = fila.Split(',');

                string pedidos = "";
                
                foreach (string f in listaFilas)
                {
                    pedidos += DGV_ListaPedidos.DataKeys[Convert.ToInt32(f)].Value.ToString().Trim() + ",";
                }
                pedidos = pedidos.TrimEnd(',');

                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDsPedidos", pedidos, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@PlantaProduccionID", plantaProduccionID, SqlDbType.Int);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.Int);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CrearODP", SqlDbType.VarChar);
                
                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearOrdenes_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        Response.Redirect("OrdenesProduccion.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearOrdenes_Click", "alertifywarning('No se ha podido crear la orden de producción, por favor intente de nuevo.');", true);
                    return;
                }
            }
        }
        #endregion

        #region Despacho
        protected void BTN_CrearDespacho_Click(object sender, EventArgs e)
        {
            int contador = 0;
            string fila = "";

            foreach (GridViewRow row in DGV_ListaPedidos.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("seleccionarCheckBox");
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

                string pedidos = "";

                foreach (string f in listaFilas)
                {
                    pedidos += DGV_ListaPedidos.DataKeys[Convert.ToInt32(f)].Value.ToString().Trim() + ",";
                }
                pedidos = pedidos.TrimEnd(',');

                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDsPedidos", pedidos, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.Int);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CrearDespacho", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP10_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDespacho_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        Session["IDDespacho"] = Result.Rows[0][1].ToString().Trim();
                        Response.Redirect("DetalleDespacho.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDespacho_Click", "alertifywarning('No se ha podido crear la orden de producción, por favor intente de nuevo.');", true);
                    return;
                }
            }
        }
        #endregion
    }
}