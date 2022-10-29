using MCWebHogar.ControlPedidos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.GestionCostos
{
    public partial class CostosIndirectos : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        decimal PorcentajeTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("../Default.aspx", true);
                }
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Costos", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Costos.");
                    Response.Redirect("../ControlPedidos/Pedido.aspx");
                }
                else
                {
                    HDF_IDUsuario.Value = Session["Usuario"].ToString();
                    cargarDDLs();
                    cargarIngresos();
                    cargarCategoriasCargaFabril();
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                }
                else if (opcion.Contains("BTN_Eliminar"))
                {
                    BTN_EliminarFactor_Click();
                }
                else if (opcion.Contains("Identificacion"))
                {
                    string identificacion = opcion.Split(';')[1];
                    Session["IdentificacionReceptor"] = identificacion;
                    Response.Redirect("../GestionProveedores/Proveedores.aspx", true);
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
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                DDL_CategoriasFactores.DataSource = Result;
                DDL_CategoriasFactores.DataTextField = "CategoriaCargaFabril";
                DDL_CategoriasFactores.DataValueField = "IDCategoriaCargaFabril";
                DDL_CategoriasFactores.DataBind();
            }
        }

        protected void BTN_ActualizarCostosProductosTerminados_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarCostosProductosTerminados", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC06_0002");

            string script = "cargarFiltros(); desactivarloading();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }
        #endregion

        #region Ingresos
        private DataTable cargarIngresosConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");
        }

        private void cargarIngresos()
        {
            Result = cargarCategoriasCargaFabrilConsulta("CargarFactorIngreso");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    TXT_MontoIngreso.Text = String.Format("{0:n}", Result.Rows[0]["Monto"]);
                }
            }
        }

        protected void TXT_MontoIngreso_OnTextChanged(object sender, EventArgs e)
        {
            try 
            {
                decimal monto = Convert.ToDecimal(TXT_MontoIngreso.Text.Replace(",", ""));
                if (monto > 0)
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@Monto", TXT_MontoIngreso.Text, SqlDbType.Decimal);
                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);

                    DT.DT1.Rows.Add("@TipoSentencia", "ActualizarIngresos", SqlDbType.VarChar);
                    
                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");
                    cargarCategoriasCargaFabril();
                    
                    string script = "cargarFiltros();alertifysuccess('Ingreso guardado con éxito.')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_MontoIngreso_OnTextChanged", script, true);
                }
                else
                {
                    string script = "cargarFiltros();alertifyerror('Por favor, ingrese un monto mayor a 0.')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_MontoIngreso_OnTextChanged", script, true);
                }
            }
            catch (Exception ex)
            {
                string script = "cargarFiltros();alertifyerror('Por favor, ingrese un monto válido.')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_MontoIngreso_OnTextChanged", script, true);
            }
            
        }
        #endregion

        #region Costos indirectos
        private DataTable cargarCategoriasCargaFabrilConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@CategoriaCargaFabrilID", HDF_IDCategoriaCargaFabril.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");
        }

        private void cargarCategoriasCargaFabril()
        {
            Result = cargarCategoriasCargaFabrilConsulta("CargarFactoresCargaFabril");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaCostosIndirectos.DataSource = Result;
                    DGV_ListaCostosIndirectos.DataBind();
                    UpdatePanel_CostosIndirectos.Update();
                    string script = "cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDespacho", script, true);
                }
            }
            else
            {
                DGV_ListaCostosIndirectos.DataSource = Result;
                DGV_ListaCostosIndirectos.DataBind();
                UpdatePanel_CostosIndirectos.Update();
                string script = "cargarFiltros();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductosDespacho", script, true);
            }
        }

        protected void DGV_ListaCostosIndirectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LBL_Monto = (Label)e.Row.FindControl("LBL_Monto");
                LBL_Monto.Text = Decimal.Parse(LBL_Monto.Text).ToString("N2");

                Label LBL_Porcentaje = (Label)e.Row.FindControl("LBL_Porcentaje");
                LBL_Porcentaje.Text = Decimal.Parse(LBL_Porcentaje.Text).ToString("N2") + "%";
                PorcentajeTotal += Convert.ToDecimal(LBL_Porcentaje.Text.Replace("%", ""));

                int idCategoria = Convert.ToInt32(DGV_ListaCostosIndirectos.DataKeys[e.Row.RowIndex].Value.ToString());
                HDF_IDCategoriaCargaFabril.Value = idCategoria.ToString();

                Result = cargarCategoriasCargaFabrilConsulta("CargarFactoresDetalle");
                GridView DGV_DetalleFactores = e.Row.FindControl("DGV_DetalleFactores") as GridView;
                DGV_DetalleFactores.DataSource = Result;
                DGV_DetalleFactores.DataBind();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_Porcentaje = (Label)e.Row.FindControl("LBL_FOO_Porcentaje");
                LBL_FOO_Porcentaje.Text = PorcentajeTotal.ToString("N2") + "%";
            }
        }

        protected void DGV_ListaPedidosDespacho_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarCategoriasCargaFabrilConsulta("CargarFactoresCargaFabril");

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
                    DGV_ListaCostosIndirectos.DataSource = Result;
                    DGV_ListaCostosIndirectos.DataBind();
                    UpdatePanel_CostosIndirectos.Update();
                }
            }
            else
            {
                DGV_ListaCostosIndirectos.DataSource = Result;
                DGV_ListaCostosIndirectos.DataBind();
                UpdatePanel_CostosIndirectos.Update();
            }
        }

        protected void BTN_AgregarFactor_Click(object sender, EventArgs e)
        {
            HDF_IDFactorCargaFabril.Value = "0";
            Title_ModalFactor.InnerText = "Agregar factor";
            TXT_DetalleFactor.Text = "";
            TXT_Monto.Text = "0";
            UpdatePanel_AgregarEditarFactor.Update();

            string script = "cargarFiltros();abrirModalAgregarEditarFactor();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AgregarFactor_Click", script, true);
        }

        protected void BTN_GuardarFactor_Click(object sender, EventArgs e)
        {
            try
            {
                decimal monto = Convert.ToDecimal(TXT_Monto.Text);
                string detalle = TXT_DetalleFactor.Text;
                string script = "";

                if (monto < 0)
                {
                    script = "cargarFiltros();alertifyerror('Por favor, ingrese un monto mayor a 0.')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AgregarFactor_Click", script, true);

                    return;
                }
                if (detalle == "")
                {
                    script = "cargarFiltros();alertifyerror('Por favor, ingrese el detalle.')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AgregarFactor_Click", script, true);

                    return;
                }
                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDFactorCargaFabril", HDF_IDFactorCargaFabril.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@DetalleFactor", TXT_DetalleFactor.Text, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@CategoriaCargaFabrilID", DDL_CategoriasFactores.SelectedValue, SqlDbType.Int);
                DT.DT1.Rows.Add("@Monto", monto, SqlDbType.Decimal);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                if (HDF_IDFactorCargaFabril.Value == "0")
                {
                    DT.DT1.Rows.Add("@TipoSentencia", "IngresarFactorCargaFabril", SqlDbType.VarChar);
                }
                else
                {
                    DT.DT1.Rows.Add("@TipoSentencia", "ActualizarFactorCargaFabril", SqlDbType.VarChar);
                }
                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");
                cargarCategoriasCargaFabril();
                script = "cargarFiltros();cerrarModalAgregarEditarFactor();alertifysuccess('Factor guardado con éxito.')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AgregarFactor_Click", script, true);
            }
            catch (Exception ex)
            {
                string script = "cargarFiltros();alertifyerror('Por favor, ingrese un monto válido.')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_AgregarFactor_Click", script, true);

                return;
            }               
        }

        protected void BTN_EliminarFactor_Click()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDFactorCargaFabril", HDF_IDFactorCargaFabril.Value, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "EliminarFactorCargaFabril", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC05_0001");
            cargarCategoriasCargaFabril();
            
            string script = "cargarFiltros();alertifysuccess('Factor elimnado con éxito.')";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarFactor_Click", script, true);
        }
        #endregion
    }
}