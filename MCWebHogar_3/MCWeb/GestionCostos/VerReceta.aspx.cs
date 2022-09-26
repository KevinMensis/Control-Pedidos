using CapaLogica.Entidades.ControlCostos;
using ClosedXML.Excel;
using MCWebHogar.ControlPedidos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.GestionCostos
{
    public partial class VerReceta : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        decimal CostoTotal = 0, TotalMOD = 0, CostoProduccion = 0;

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
                    cargarProductos("");
                    cargarMateriasPrimas("");
                    cargarEmpleados("");
                    cargarCostosProduccion();
                    cargarResumen();
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_BuscarProducto"))
                {
                    cargarProductos("");
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
        }

        private void MostrarOcultarElementos(bool mostrar)
        {
            H5_Title.InnerText = "";
            H5_Title.Visible = mostrar;
            H6_Subtitle.InnerText = "";
            H6_Subtitle.Visible = mostrar;
            H5_TitleEmpleado.InnerText = "";
            H5_TitleEmpleado.Visible = mostrar;
            H6_SubtitleEmpleado.InnerText = "";
            H6_SubtitleEmpleado.Visible = mostrar;
            H5_Subtitle.InnerText = "";
            H5_Subtitle.Visible = mostrar;
            H5_Resumen.InnerText = "";
            H5_Resumen.Visible = mostrar;
        }
        #endregion

        #region Productos Terminados
        private DataTable cargarProductosConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC06_0001");
        }

        private void cargarProductos(string ejecutar)
        {            
            Result = cargarProductosConsulta("CargarProductosTerminados");
            
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ProductosTerminados.DataSource = Result;
                    DGV_ProductosTerminados.DataBind();
                }
            }
            else
            {
                DGV_ProductosTerminados.DataSource = Result;
                DGV_ProductosTerminados.DataBind();
            }

            UpdatePanel_Filtros.Update();

            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void DGV_ProductosTerminados_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarProductosConsulta("CargarProductosTerminados");

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
                    DGV_ProductosTerminados.DataSource = Result;
                    DGV_ProductosTerminados.DataBind();
                }
            }
            else
            {
                DGV_ProductosTerminados.DataSource = Result;
                DGV_ProductosTerminados.DataBind();
            }

            UpdatePanel_Filtros.Update();

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaProductos_Sorting", script, true);
        }

        protected void DGV_ProductosTerminados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProductoTerminado = Convert.ToInt32(DGV_ProductosTerminados.DataKeys[rowIndex].Value.ToString().Trim());
                string detalleProductoTerminado = DGV_ProductosTerminados.DataKeys[rowIndex].Values[1].ToString().Trim();

                if (e.CommandName == "VerProductoTerminado")
                {
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    HDF_DetalleProducto.Value = detalleProductoTerminado;

                    cargarMateriasPrimas("");
                    cargarEmpleados("");
                    cargarCostosProduccion();
                    cargarResumen();
                }
            }
        }

        protected void FiltrarProductos_OnClick(object sender, EventArgs e)
        {
            cargarProductos("");
        }

        protected void BTN_ReporteProductos_OnClick(object sender, EventArgs e)
        {
            DataTable ResultHistorico = new DataTable();

            Result = cargarProductosConsulta("ReporteProductosTerminados");            

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Productos");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ProductosTerminados.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarProducto_OnClick", "desactivarloading();cargarFiltros();", true);
        }
        #endregion

        #region Materias primas
        private DataTable cargarMateriasPrimasAsignadasConsulta(string consulta, int idProductoTerminado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC01_0001");
        }

        private void cargarMateriasPrimas(string ejecutar)
        {
            int idProductoTerminado = Convert.ToInt32(HDF_IDProductoTerminado.Value);

            if (idProductoTerminado != 0)
            {
                MostrarOcultarElementos(true);
                H5_Title.InnerText = "Receta de: " + HDF_DetalleProducto.Value;
                H6_Subtitle.InnerText = "Materia prima";
                H5_TitleEmpleado.InnerText = HDF_DetalleProducto.Value;
                H6_SubtitleEmpleado.InnerText = "Mano de obra directa";
                H5_Subtitle.InnerText = "Costos indirectos";
                H5_Resumen.InnerText = "Resumen";
                UpdatePanel_Filtros.Update();

                Result = cargarMateriasPrimasAsignadasConsulta("CargarRecetaProductoTerminado", idProductoTerminado);

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        return;
                    }
                    else
                    {
                        DGV_ListaMateriasPrimasAsignadas.DataSource = Result;
                        DGV_ListaMateriasPrimasAsignadas.DataBind();
                    }
                }
                else
                {
                    DGV_ListaMateriasPrimasAsignadas.DataSource = Result;
                    DGV_ListaMateriasPrimasAsignadas.DataBind();
                }
            }
            else
            {
                MostrarOcultarElementos(false);
                DGV_ListaMateriasPrimasAsignadas.DataSource = null;
                DGV_ListaMateriasPrimasAsignadas.DataBind();
            }

            UpdatePanel_MateriasPrimasAsignadas.Update();
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void DGV_ListaMateriasPrimasAsignadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LBL_CostoTotal = (Label)e.Row.FindControl("LBL_CostoTotal");
                LBL_CostoTotal.Text = Decimal.Parse(LBL_CostoTotal.Text).ToString("N2");
                CostoTotal += Convert.ToDecimal(LBL_CostoTotal.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_CostoTotal = (Label)e.Row.FindControl("LBL_FOO_CostoTotal");
                LBL_FOO_CostoTotal.Text = CostoTotal.ToString("N2");
            }
        }
        #endregion

        #region Colaboradores
        private DataTable cargarEmpleadosConsulta(string consulta, int idProductoTerminado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC03_0001");
        }

        private void cargarEmpleados(string ejecutar)
        {
            int idProductoTerminado = Convert.ToInt32(HDF_IDProductoTerminado.Value);

            if (idProductoTerminado != 0)
            {
                MostrarOcultarElementos(true);
                H5_Title.InnerText = "Receta de: " + HDF_DetalleProducto.Value;
                H6_Subtitle.InnerText = "Materia prima";
                H5_TitleEmpleado.InnerText = HDF_DetalleProducto.Value;
                H6_SubtitleEmpleado.InnerText = "Mano de obra directa";
                H5_Subtitle.InnerText = "Costos indirectos";
                H5_Resumen.InnerText = "Resumen";
                UpdatePanel_Filtros.Update();

                Result = cargarEmpleadosConsulta("CargarRecetaProductoTerminado", idProductoTerminado);

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
            }
            else
            {
                MostrarOcultarElementos(false);
                DGV_ListaEmpleados.DataSource = null;
                DGV_ListaEmpleados.DataBind();
            }

            UpdatePanel_ListaEmpleados.Update();
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void DGV_ListaEmpleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LBL_TotalMOD = (Label)e.Row.FindControl("LBL_TotalMOD");
                LBL_TotalMOD.Text = Decimal.Parse(LBL_TotalMOD.Text).ToString("N2");
                TotalMOD += Convert.ToDecimal(LBL_TotalMOD.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_TotalMOD = (Label)e.Row.FindControl("LBL_FOO_TotalMOD");
                LBL_FOO_TotalMOD.Text = TotalMOD.ToString("N2");
            }
        }
        #endregion

        #region Costos Produccion
        private DataTable cargarCostosProduccionConsulta(string consulta, int idProductoTerminado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");
        }

        private void cargarCostosProduccion()
        {
            int idProductoTerminado = Convert.ToInt32(HDF_IDProductoTerminado.Value);
            UpdatePanel_Filtros.Update();

            if (idProductoTerminado != 0)
            {
                Result = cargarCostosProduccionConsulta("ResumenCostos", idProductoTerminado);

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        return;
                    }
                    else
                    {
                        DGV_ListaCostosProduccion.DataSource = Result;
                        DGV_ListaCostosProduccion.DataBind();
                    }
                }
                else
                {
                    DGV_ListaCostosProduccion.DataSource = Result;
                    DGV_ListaCostosProduccion.DataBind();
                }
            }
            else
            {
                DGV_ListaCostosProduccion.DataSource = null;
                DGV_ListaCostosProduccion.DataBind();
            }

            UpdatePanel_Resumen.Update();
        }

        protected void DGV_ListaCostosProduccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LBL_CostoProduccion = (Label)e.Row.FindControl("LBL_CostoProduccion");
                LBL_CostoProduccion.Text = Decimal.Parse(LBL_CostoProduccion.Text).ToString("N2");
                CostoProduccion += Convert.ToDecimal(LBL_CostoProduccion.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label LBL_FOO_CostoProduccion = (Label)e.Row.FindControl("LBL_FOO_CostoProduccion");
                LBL_FOO_CostoProduccion.Text = CostoProduccion.ToString("N2");
            }
        }
        #endregion

        #region Resumen
        private DataTable cargarResumenConsulta(string consulta, int idProductoTerminado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");
        }

        private void cargarResumen()
        {
            int idProductoTerminado = Convert.ToInt32(HDF_IDProductoTerminado.Value);
            UpdatePanel_Filtros.Update();

            if (idProductoTerminado != 0)
            {
                Result = cargarResumenConsulta("Resumen", idProductoTerminado);

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        return;
                    }
                    else
                    {
                        DGV_ListaResumen.DataSource = Result;
                        DGV_ListaResumen.DataBind();
                        string usuario = Session["UserID"].ToString().Trim();
                        string script = "cargarFiltros();cargarGraficos(" + usuario + ");";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcarcargarResumen", script, true);
                    }
                }
                else
                {
                    DGV_ListaResumen.DataSource = Result;
                    DGV_ListaResumen.DataBind();
                }
            }
            else
            {
                DGV_ListaResumen.DataSource = null;
                DGV_ListaResumen.DataBind();
            }

            UpdatePanel_Resumen.Update();
        }

        [WebMethod()]
        public static List<DatosCostos> cargarGraficoCostos(string idUsuario, string idProductoTerminado)
        {
            List<DatosCostos> lista = new List<DatosCostos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.VarChar);
                
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoCostos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosCostos dh = new DatosCostos();

                    dh.factor = dr["Factor"].ToString().Trim();
                    dh.porcentaje = Convert.ToDecimal(dr["Porcentaje"].ToString().Trim());
                    dh.monto = Convert.ToDecimal(dr["Monto"].ToString().Trim());

                    lista.Add(dh);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return lista;
        }

        [WebMethod()]
        public static List<DatosCostos> cargarGraficoCostosProduccion(string idUsuario, string idProductoTerminado)
        {
            List<DatosCostos> lista = new List<DatosCostos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "ResumenCostos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosCostos dh = new DatosCostos();

                    dh.factor = dr["Descripcion"].ToString().Trim();
                    dh.porcentaje = Convert.ToDecimal(dr["Valor"].ToString().Trim());
                    dh.monto = Convert.ToDecimal(dr["CostoProduccion"].ToString().Trim());

                    lista.Add(dh);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return lista;
        }

        [WebMethod()]
        public static List<DatosCostos> cargarGraficoCostoResumen(string idUsuario, string idProductoTerminado)
        {
            List<DatosCostos> lista = new List<DatosCostos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "Resumen", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC00_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    string factor = dr["Descripcion"].ToString().Trim();
                    if (!factor.Contains("Precio")){
                        DatosCostos dh = new DatosCostos();

                        dh.factor = factor;
                        dh.porcentaje = Convert.ToDecimal(dr["Porcentaje"].ToString().Trim());
                        dh.monto = Convert.ToDecimal(dr["Valor"].ToString().Trim());
                        
                        lista.Add(dh);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return lista;
        }
        #endregion
    }
}