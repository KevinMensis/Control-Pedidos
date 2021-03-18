using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class OrdenesProduccion : System.Web.UI.Page
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
                    cargarODPs();
                    // cargarDDLs();
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
        #endregion

        #region ODPS
        private DataTable cargarODPsConsulta()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarODPs", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP07_0001");
        }

        private void cargarODPs()
        {                                                               
            Result = cargarODPsConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarODPs", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaOrdenesProduccion.DataSource = Result;
                    DGV_ListaOrdenesProduccion.DataBind();
                    UpdatePanel_ListaOrdenesProduccion.Update();
                }
            }
            else
            {
                DGV_ListaOrdenesProduccion.DataSource = Result;
                DGV_ListaOrdenesProduccion.DataBind();
                UpdatePanel_ListaOrdenesProduccion.Update();
            }
        }

        protected void TXT_FiltrarODPs_OnTextChanged(object sender, EventArgs e)
        {
            cargarODPs();
        }

        protected void DGV_ListaOrdenesProduccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idOrdenProduccion = DGV_ListaOrdenesProduccion.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDODP"] = idOrdenProduccion;
                    Response.Redirect("DetalleODP.aspx", true);
                }
            }
        }

        protected void DGV_ListaOrdenesProduccion_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarODPsConsulta();

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
                    DGV_ListaOrdenesProduccion.DataSource = Result;
                    DGV_ListaOrdenesProduccion.DataBind();
                    UpdatePanel_ListaOrdenesProduccion.Update();
                }
            }
            else
            {
                DGV_ListaOrdenesProduccion.DataSource = Result;
                DGV_ListaOrdenesProduccion.DataBind();
                UpdatePanel_ListaOrdenesProduccion.Update();
            }
        }

        protected void DGV_ListaOrdenesProduccion_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                string estado = rowView["Estado"].ToString().Trim();

                if (estado != "Completado")
                {
                    CheckBox chkSeleccionar = (CheckBox)e.Row.FindControl("seleccionarCheckBox");
                    chkSeleccionar.Visible = false;
                }
            }
        }
        #endregion

        #region Despacho
        protected void BTN_CrearDespacho_Click(object sender, EventArgs e)
        {
            int contador = 0;
            string fila = "";

            foreach (GridViewRow row in DGV_ListaOrdenesProduccion.Rows)
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
            if (fila != "" && contador == 1)
            {
                fila = fila.TrimEnd(',');
                string[] listaFilas = fila.Split(',');

                string pedidos = "";

                foreach (string f in listaFilas)
                {
                    pedidos += DGV_ListaOrdenesProduccion.DataKeys[Convert.ToInt32(f)].Values[4].ToString().Trim() + ",";
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
            else if (contador > 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDespacho_Click", "alertifywarning('Por favor, seleccione solo una orden de pedido para crear el Despacho.');", true);
                return;
            }
        }
        #endregion
    }
}