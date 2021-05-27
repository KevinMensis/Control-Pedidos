using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Empaque : System.Web.UI.Page
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
                    cargarEmpaques();                    
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
            //TXT_FechaEmpaqueDesde.Text = "1900-01-01";
            //TXT_FechaEmpaqueHasta.Text = "1900-01-01";

            UpdatePanel_FiltrosEmpaques.Update();
        }
        #endregion

        #region Empaques
        private DataTable cargarEmpaquesConsulta()
        {
            DT.DT1.Clear();
            #region Fechas
            string fechaEmpaqueDesde = "1900-01-01";
            // string fechaEmpaqueHasta = "1900-01-01";

            try
            {
                fechaEmpaqueDesde = Convert.ToDateTime(TXT_FechaCreacionDesde.Text).ToString();
                // fechaEmpaqueHasta = Convert.ToDateTime(TXT_FechaEmpaqueHasta.Text).ToString();

            }
            catch (Exception e)
            {
                fechaEmpaqueDesde = "1900-01-01";
                // fechaEmpaqueHasta = "1900-01-01";
            }

            DT.DT1.Rows.Add("@fechaEmpaqueDesde", fechaEmpaqueDesde, SqlDbType.Date);
            // DT.DT1.Rows.Add("@fechaEmpaqueHasta", fechaEmpaqueHasta, SqlDbType.Date);
            #endregion
            
            UpdatePanel_FiltrosEmpaques.Update();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmpaques", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");
        }

        private void cargarEmpaques()
        {
            Result = cargarEmpaquesConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarEmpaques", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaEmpaques.DataSource = Result;
                    DGV_ListaEmpaques.DataBind();
                    UpdatePanel_ListaEmpaques.Update();
                }
            }
            else
            {
                DGV_ListaEmpaques.DataSource = Result;
                DGV_ListaEmpaques.DataBind();
                UpdatePanel_ListaEmpaques.Update();
            }
        }

        protected void TXT_FiltrarEmpaques_OnTextChanged(object sender, EventArgs e)
        {
            cargarEmpaques();
        }

        protected void DGV_ListaEmpaques_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idEmpaque = DGV_ListaEmpaques.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDEmpaque"] = idEmpaque;
                    Response.Redirect("DetalleEmpaque.aspx", true);
                }
            }
        }

        protected void DGV_ListaEmpaques_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarEmpaquesConsulta();

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
                    DGV_ListaEmpaques.DataSource = Result;
                    DGV_ListaEmpaques.DataBind();
                    UpdatePanel_ListaEmpaques.Update();
                }
            }
            else
            {
                DGV_ListaEmpaques.DataSource = Result;
                DGV_ListaEmpaques.DataBind();
                UpdatePanel_ListaEmpaques.Update();
            }
        }

        protected void DGV_ListaEmpaques_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                // string estado = rowView["Estado"].ToString().Trim();
            }
        }

        protected void BTN_CrearEmpaques_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearEmpaque", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP14_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearEmpaques_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    Session["IDEmpaque"] = Result.Rows[0][1].ToString().Trim();
                    Session["NuevoEmpaque"] = Result.Rows[0][1].ToString().Trim();
                    Response.Redirect("DetalleEmpaque.aspx", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearEmpaques_Click", "alertifywarning('No se ha podido crear la orden de empaque, por favor intente de nuevo.');", true);
                return;
            }
        }
        #endregion
    }
}