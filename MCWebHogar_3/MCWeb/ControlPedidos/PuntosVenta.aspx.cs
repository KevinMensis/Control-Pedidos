using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class PuntosVenta : System.Web.UI.Page
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
                    cargarPuntosVenta("");
                    ViewState["Ordenamiento"] = "ASC";
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
        #endregion

        #region Puntos Venta
        private DataTable cargarPuntosVentaConsulta()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVentaAll", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");
        }

        private void ActivarDesactivarPuntoVenta(int idPuntoVenta, int estado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDPuntoVenta", idPuntoVenta, SqlDbType.Int);
            DT.DT1.Rows.Add("@Activo", estado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActivarDesactivarPuntoVenta", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    Result = cargarPuntosVentaConsulta();
                    DGV_ListaPuntosVenta.DataSource = Result;
                    DGV_ListaPuntosVenta.DataBind();
                    UpdatePanel_ListaPuntosVenta.Update();
                }
            }
            else
            {
                Result = cargarPuntosVentaConsulta();
                DGV_ListaPuntosVenta.DataSource = Result;
                DGV_ListaPuntosVenta.DataBind();
                UpdatePanel_ListaPuntosVenta.Update();
            }

            string script = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivarDesactivarPuntoVenta", script, true);
        }

        private void cargarPuntosVenta(string ejecutar)
        {
            Result = cargarPuntosVentaConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaPuntosVenta.DataSource = Result;
                    DGV_ListaPuntosVenta.DataBind();
                    UpdatePanel_ListaPuntosVenta.Update();
                }
            }
            else
            {
                DGV_ListaPuntosVenta.DataSource = Result;
                DGV_ListaPuntosVenta.DataBind();
                UpdatePanel_ListaPuntosVenta.Update();
            }
            string script = "" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPuntosVenta", script, true);
        }

        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarPuntosVenta("");
        }

        protected void DGV_ListaPuntosVenta_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarPuntosVentaConsulta();

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
                    DGV_ListaPuntosVenta.DataSource = Result;
                    DGV_ListaPuntosVenta.DataBind();
                    UpdatePanel_ListaPuntosVenta.Update();
                }
            }
            else
            {
                DGV_ListaPuntosVenta.DataSource = Result;
                DGV_ListaPuntosVenta.DataBind();
                UpdatePanel_ListaPuntosVenta.Update();
            }
        }

        protected void DGV_ListaPuntosVenta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idPuntoVenta = Convert.ToInt32(DGV_ListaPuntosVenta.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "activar")
                {
                    ActivarDesactivarPuntoVenta(idPuntoVenta, 1);
                }
                else if (e.CommandName == "desactivar")
                {
                    ActivarDesactivarPuntoVenta(idPuntoVenta, 0);
                }
                else if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDPuntoVenta", idPuntoVenta, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntoVenta", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDPuntoVenta.Value = dr["IDPuntoVenta"].ToString().Trim();
                            TXT_DescripcionPuntoVenta.Text = dr["DescripcionPuntoVenta"].ToString().Trim();
                            TXT_UbicacionPuntoVenta.Text = dr["UbicacionPuntoVenta"].ToString().Trim();
                        }
                    }

                    title_CrearPuntoVenta.InnerHtml = "Editar punto venta";
                    UpdatePanel_ModalCrearPuntoVenta.Update();

                    string script = "abrirModalCrearPuntoVenta();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearPuntoVenta_OnClick", script, true);
                }
            }
        }

        protected void DGV_ListaPuntosVenta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);
                bool estado = Convert.ToBoolean(DGV_ListaPuntosVenta.DataKeys[rowIndex].Values[1]);

                Button BTN_Activar = e.Row.Cells[0].FindControl("BTN_Activar") as Button;
                Button BTN_Eliminar = e.Row.Cells[0].FindControl("BTN_Eliminar") as Button;

                BTN_Activar.Visible = !estado;
                BTN_Eliminar.Visible = estado;
            }
        }

        protected void BTN_CrearPuntoVenta_OnClick(object sender, EventArgs e)
        {
            HDF_IDPuntoVenta.Value = "0";
            TXT_DescripcionPuntoVenta.Text = "";
            TXT_UbicacionPuntoVenta.Text = "";

            title_CrearPuntoVenta.InnerHtml = "Crear punto venta";
            UpdatePanel_ModalCrearPuntoVenta.Update();

            string script = "abrirModalCrearPuntoVenta();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearPuntoVenta_OnClick", script, true);
        }

        protected void BTN_GuardarProducto_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDPuntoVenta", HDF_IDPuntoVenta.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@DescripcionPuntoVenta", TXT_DescripcionPuntoVenta.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@UbicacionPuntoVenta", TXT_UbicacionPuntoVenta.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            if (HDF_IDPuntoVenta.Value == "0")
                DT.DT1.Rows.Add("@TipoSentencia", "CrearPuntoVenta", SqlDbType.VarChar);
            else
                DT.DT1.Rows.Add("@TipoSentencia", "EditarPuntoVenta", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");
            string script = "";
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    script += "alertifyerror('No se ha guardado el punto de venta. Error: " + Result.Rows[0][1].ToString().Trim() + "');";
                    cargarPuntosVenta(script);
                }
                else
                {
                    script += "cerrarModalCrearPuntoVenta();alertifysuccess('Se ha guardado el punto de venta con éxito.')";
                    cargarPuntosVenta(script);
                }
            }
            else
            {
                script += "alertifywarning('No se ha guardado el punto de venta. Por favor, intente nuevamente');";
                cargarPuntosVenta(script);
            }
        }
        #endregion
    }
}