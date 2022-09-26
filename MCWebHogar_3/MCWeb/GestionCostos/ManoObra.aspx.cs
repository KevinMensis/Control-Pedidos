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
    public partial class ManoObra : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Costos", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Costos.");
                    Response.Redirect("../ControlPedidos/Pedido.aspx");
                }
                else
                {
                    HDF_IDUsuario.Value = Session["Usuario"].ToString();
                    cargarDDLs();
                    cargarCostosProduccion("");
                    cargarCostosEmpleados("");
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                }
                else if (opcion.Contains("Identificacion"))
                {
                    string identificacion = opcion.Split(';')[1];
                    Session["IdentificacionReceptor"] = identificacion;
                    Response.Redirect("../GestionProveedores/Proveedores.aspx", true);
                }
                else if (opcion.Contains("CargarCostos"))
                {
                    cargarCostosProduccion("");
                    cargarCostosEmpleados("");
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
            
        }
        #endregion

        #region Costos Producción
        private DataTable cargarCostosProduccionConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");
        }

        private void cargarCostosProduccion(string ejecutar)
        {

            Result = cargarCostosProduccionConsulta("CargarCostos");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaCostos.DataSource = Result;
                    DGV_ListaCostos.DataBind();
                }
            }
            else
            {
                DGV_ListaCostos.DataSource = Result;
                DGV_ListaCostos.DataBind();
            }

            UpdatePanel_ListaCostos.Update();
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarCostos", script, true);
        }

        [WebMethod()]
        public static string BTN_ActualizarValorCosto_Click(int idCosto, decimal valor, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ID", idCosto, SqlDbType.Int);
            DT.DT1.Rows.Add("@Valor", valor, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarCostos", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");
            return "correcto";
        }
        #endregion

        #region Costos Empleado
        private DataTable cargarCostosEmpleadosConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC02_0001");
        }

        private void cargarCostosEmpleados(string ejecutar)
        {

            Result = cargarCostosEmpleadosConsulta("CargarEmpleados");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaEmpleados.DataSource = Result;
                    DGV_ListaEmpleados.DataBind();
                }
            }
            else
            {
                DGV_ListaEmpleados.DataSource = Result;
                DGV_ListaEmpleados.DataBind();
            }

            UpdatePanel_ListaEmpleados.Update();
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarCostos", script, true);
        }

        [WebMethod()]
        public static string BTN_ActualizarSalarioEmpleado_Click(int idEmpleado, decimal salario, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDEmpleado", idEmpleado, SqlDbType.Int);
            DT.DT1.Rows.Add("@Salario", salario, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarEmpleado", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC02_0001");
            return "correcto";
        }
        #endregion
    }
}