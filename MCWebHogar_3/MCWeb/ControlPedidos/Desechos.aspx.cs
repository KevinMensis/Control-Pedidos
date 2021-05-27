using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Desechos : System.Web.UI.Page
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
                    cargarDDLs();
                    cargarDesechos();
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
        private void cargarDDLs()
        {
            DateTime hoy = DateTime.Now.AddDays(-1);
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            TXT_FechaCreacionDesde.Text = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia; // "1900-01-01";
            //TXT_FechaDesechoDesde.Text = "1900-01-01";
            //TXT_FechaDesechoHasta.Text = "1900-01-01";

            UpdatePanel_FiltrosDesechos.Update();
        }
        #endregion

        #region Desechos
        private DataTable cargarDesechosConsulta()
        {
            DT.DT1.Clear();

            #region Fechas
            string fechaDesechoDesde = "1900-01-01";
            // string fechaDesechoHasta = "1900-01-01";

            try
            {
                fechaDesechoDesde = Convert.ToDateTime(TXT_FechaCreacionDesde.Text).ToString();
                // fechaDesechoHasta = Convert.ToDateTime(TXT_FechaDesechoHasta.Text).ToString();

            }
            catch (Exception e)
            {
                fechaDesechoDesde = "1900-01-01";
                // fechaDesechoHasta = "1900-01-01";
            }

            DT.DT1.Rows.Add("@fechaDesechoDesde", fechaDesechoDesde, SqlDbType.Date);
            // DT.DT1.Rows.Add("@fechaDesechoHasta", fechaDesechoHasta, SqlDbType.Date);
            #endregion

            UpdatePanel_FiltrosDesechos.Update();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarDesechos", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP16_0001");
        }

        private void cargarDesechos()
        {
            Result = cargarDesechosConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDesechos", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaDesechos.DataSource = Result;
                    DGV_ListaDesechos.DataBind();
                    UpdatePanel_ListaDesechos.Update();
                }
            }
            else
            {
                DGV_ListaDesechos.DataSource = Result;
                DGV_ListaDesechos.DataBind();
                UpdatePanel_ListaDesechos.Update();
            }
        }

        protected void TXT_FiltrarDesechos_OnTextChanged(object sender, EventArgs e)
        {
            cargarDesechos();
        }

        protected void DGV_ListaDesechos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idDesecho = DGV_ListaDesechos.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDDesecho"] = idDesecho;
                    Response.Redirect("DetalleDesecho.aspx", true);
                }
            }
        }

        protected void DGV_ListaDesechos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarDesechosConsulta();

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
                    DGV_ListaDesechos.DataSource = Result;
                    DGV_ListaDesechos.DataBind();
                    UpdatePanel_ListaDesechos.Update();
                }
            }
            else
            {
                DGV_ListaDesechos.DataSource = Result;
                DGV_ListaDesechos.DataBind();
                UpdatePanel_ListaDesechos.Update();
            }
        }

        protected void DGV_ListaDesechos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                // string estado = rowView["Estado"].ToString().Trim();
            }
        }

        protected void BTN_CrearDesechos_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearDesecho", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP16_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDesechos_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    Session["IDDesecho"] = Result.Rows[0][1].ToString().Trim();
                    Session["NuevoDesecho"] = Result.Rows[0][1].ToString().Trim();
                    Response.Redirect("DetalleDesecho.aspx", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDesechos_Click", "alertifywarning('No se ha podido crear la orden de desecho, por favor intente de nuevo.');", true);
                return;
            }
        }
        #endregion
    }
}