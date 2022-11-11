using CapaLogica.Entidades.ControlCostos;
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
    public partial class CrearReceta : System.Web.UI.Page
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
                else if (opcion.Contains("TXT_BuscarMateriaPrima"))
                {
                    cargarMateriasPrimas("");
                }
                else if (opcion.Contains("CargarMateriasPrimas"))
                {
                    cargarMateriasPrimas("");
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
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", "3101485961", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmisoresMateriaPrima", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

            LB_Emisores.DataSource = Result;
            LB_Emisores.DataTextField = "NombreComercial";
            LB_Emisores.DataValueField = "IDEmisor";
            LB_Emisores.DataBind();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "SeleccionarEmpleado", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC02_0001");

            DDL_Empleado.DataSource = Result;
            DDL_Empleado.DataTextField = "Descripcion";
            DDL_Empleado.DataValueField = "IDEmpleado";
            DDL_Empleado.DataBind();

            UpdatePanel_MateriasPrimasAsignadas.Update();
        }

        private void MostrarOcultarElementos(bool mostrar)
        {
            H5_Title.InnerText = "";
            H5_Title.Visible = mostrar;
            H6_Subtitle.InnerText = "";
            H6_Subtitle.Visible = mostrar;
            H6_SubtitleEmpleado.InnerText = "";
            H6_SubtitleEmpleado.Visible = mostrar;
            H5_Subtitle.InnerText = "";
            H5_Subtitle.Visible = mostrar;
            H5_Resumen.InnerText = "";
            H5_Resumen.Visible = mostrar;
            LBL_CantidadProducida.Visible = mostrar;
            TXT_CantidadProducida.Visible = mostrar;
            DDL_Empleado.Visible = mostrar;
            LBL_Empleado.Visible = mostrar;
            TXT_CantidadMinutos.Visible = mostrar;
            LBL_CantidadMinutos.Visible = mostrar;
            BTN_AsignarEmpleado.Visible = mostrar;
            BTN_ActualizarCostoProducto.Visible = mostrar;
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

        #region Productos Terminados
        private DataTable cargarProductosConsulta(string consulta)
        {
            DT.DT1.Clear();
                          
            DT.DT1.Rows.Add("@DescripcionProducto", TXT_BuscarProducto.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);
                
            UpdatePanel_Filtros.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");
        }

        private void cargarProductos(string ejecutar)
        {
            Result = cargarProductosConsulta("CargarProductosTerminado");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    LB_Productos.DataSource = Result;
                    LB_Productos.DataTextField = "DescripcionProducto";
                    LB_Productos.DataValueField = "IDProducto";                    
                    LB_Productos.DataBind();
                    UpdatePanel_Filtros.Update();
                }
            }
            else
            {
                LB_Productos.DataSource = Result;
                LB_Productos.DataTextField = "DescripcionProducto";
                LB_Productos.DataValueField = "IDProducto";
                LB_Productos.DataBind();
                UpdatePanel_Filtros.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void FiltrarProductos_OnClick(object sender, EventArgs e)
        {
            cargarProductos("");
        }

        protected void LB_Productos_OnClick(object sender, EventArgs e)
        {
            cargarUnidadesProducidas();
            cargarMateriasPrimas("");
            cargarEmpleados("");
            cargarCostosProduccion();
            cargarResumen();
            cargarUnidadesProducidas();
        }

        protected void BTN_ActualizarCostoProducto_OnClick(object sender, EventArgs e)
        {
            int idProductoTerminado = Convert.ToInt32(HDF_IDProductoTerminado.Value);
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProductoTerminado", idProductoTerminado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarProductoTerminado", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC06_0001");

            string script = "cargarFiltros(); desactivarloading();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        private void cargarUnidadesProducidas()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProductoTerminado", HDF_IDProductoTerminado.Value, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductoTerminadoInfo", SqlDbType.VarChar);

            UpdatePanel_Filtros.Update();

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC06_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    TXT_CantidadProducida.Text = Result.Rows[0]["MedidaUnidades"].ToString().Trim();
                }
            }
        }
        #endregion

        #region Materias primas
        private DataTable cargarMateriasPrimasConsulta(string consulta, int idProductoTerminado)
        {
            DT.DT1.Clear();

            string emisores = "";
            string emisoresText = "";
            string categorias = "";
            string categoriasText = "";

            #region Emisores
            foreach (ListItem l in LB_Emisores.Items)
            {
                if (l.Selected)
                {
                    emisores += "'" + l.Value + "',";
                    emisoresText += l.Text + ",";
                }
            }
            emisores = emisores.TrimEnd(',');
            emisoresText = emisoresText.TrimEnd(',');
            if (emisoresText != "")
            {
                DT.DT1.Rows.Add("@FiltrarEmisor", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@EmisoresFiltro", emisores, SqlDbType.VarChar);
            }
            #endregion

            DT.DT1.Rows.Add("@DetalleProducto", TXT_BuscarMateriaPrima.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@IDProductoTerminado", idProductoTerminado, SqlDbType.Int);
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", "3101485961", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");
        }

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
            int idProductoTerminado = 0;

            foreach (ListItem l in LB_Productos.Items)
            {
                if (l.Selected)
                {
                    idProductoTerminado = Convert.ToInt32(l.Value);
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    MostrarOcultarElementos(true);
                    H5_Title.InnerText = "Receta de: " + l.Text;
                    H6_Subtitle.InnerText = "Materia prima";
                    H6_SubtitleEmpleado.InnerText = "Mano de obra directa";
                    H5_Subtitle.InnerText = "Costos indirectos";
                    H5_Resumen.InnerText = "Resumen";
                    UpdatePanel_Filtros.Update();
                }
            }

            if (idProductoTerminado != 0)
            {
                Result = cargarMateriasPrimasConsulta("CargarMateriaPrimaSinAsignar", idProductoTerminado);

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        return;
                    }
                    else
                    {
                        DGV_ListaMateriasPrimas.DataSource = Result;
                        DGV_ListaMateriasPrimas.DataBind();
                    }
                }
                else
                {
                    DGV_ListaMateriasPrimas.DataSource = Result;
                    DGV_ListaMateriasPrimas.DataBind();
                }

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
                DGV_ListaMateriasPrimas.DataSource = null;
                DGV_ListaMateriasPrimas.DataBind();
                DGV_ListaMateriasPrimasAsignadas.DataSource = null;
                DGV_ListaMateriasPrimasAsignadas.DataBind();
            }
            
            UpdatePanel_ListaMateriasPrimas.Update();
            UpdatePanel_MateriasPrimasAsignadas.Update();
            string script = "cargarFiltros();productosMarcados();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void FiltrarMateriaPrima_OnClick(object sender, EventArgs e)
        {
            cargarMateriasPrimas("");
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

        protected void TXT_UnidadesProducidas_OnTextChanged(object sender, EventArgs e)
        {
            string cantidad = TXT_CantidadProducida.Text;
            if (cantidad == "" || cantidad == "0")
            {
                string script = "cargarFiltros();alertifyerror('Por favor, ingrese la cantidad de unidades producida.')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptTXT_UnidadesProducidas_OnTextChanged", script, true);
            }
            else
            {
                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDProductoTerminado", HDF_IDProductoTerminado.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@MedidaUnidades", Convert.ToDecimal(cantidad), SqlDbType.Decimal);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "ActualizarCantidadProducida", SqlDbType.VarChar);

                UpdatePanel_Filtros.Update();

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC06_0001");
                cargarCostosProduccion();
                cargarResumen();
            }
        }

        [WebMethod()]
        public static string BTN_Agregar_Click(int idProductoTerminado, int idProductoMateriaPrima, int productoIntermedio, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);
            DT.DT1.Rows.Add("@ProductoMateriaPrimaID", idProductoMateriaPrima, SqlDbType.Int);
            DT.DT1.Rows.Add("@ProductoIntermedio", 0, SqlDbType.Int);
            DT.DT1.Rows.Add("@MateriaPrimaIntermedia", productoIntermedio, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AgregarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC01_0001");
            return "correcto";
        }

        [WebMethod()]
        public static string BTN_Eliminar_Click(int idConsecutivoReceta, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDConsecutivoReceta", idConsecutivoReceta, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "EliminarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC01_0001");
            return "correcto";
        }

        [WebMethod()]
        public static string BTN_ActualizarMedidaUnidades_Click(int idProducto, decimal medidaUnidades, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@MedidaUnidades", medidaUnidades, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarMedidaUnidades", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP03_0001");
            return "correcto";
        }

        [WebMethod()]
        public static string BTN_ActualizarCantidadNecesaria_Click(int idConsecutivoReceta, decimal cantidadNecesaria, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDConsecutivoReceta", idConsecutivoReceta, SqlDbType.Int);
            DT.DT1.Rows.Add("@CantidadNecesaria", cantidadNecesaria, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActualizarCantidadNecesaria", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC01_0001");
            return "correcto";
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
            int idProductoTerminado = 0;

            foreach (ListItem l in LB_Productos.Items)
            {
                if (l.Selected)
                {
                    idProductoTerminado = Convert.ToInt32(l.Value);
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    MostrarOcultarElementos(true);
                    H5_Title.InnerText = "Receta de: " + l.Text;
                    H6_Subtitle.InnerText = "Materia prima";
                    H6_SubtitleEmpleado.InnerText = "Mano de obra directa";
                    H5_Subtitle.InnerText = "Costos indirectos";
                    H5_Resumen.InnerText = "Resumen";
                    UpdatePanel_Filtros.Update();
                }
            }

            if (idProductoTerminado != 0)
            {
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

            UpdatePanel_MateriasPrimasAsignadas.Update();
            string script = "cargarFiltros();productosMarcados();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        private void agregarEmpleados()
        {
            int idProductoTerminado = 0;

            foreach (ListItem l in LB_Productos.Items)
            {
                if (l.Selected)
                {
                    idProductoTerminado = Convert.ToInt32(l.Value);
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    MostrarOcultarElementos(true);
                    H5_Title.InnerText = "Receta de: " + l.Text;
                    H6_Subtitle.InnerText = "Materia prima";
                    H6_SubtitleEmpleado.InnerText = "Mano de obra directa";
                    H5_Subtitle.InnerText = "Costos indirectos";
                    H5_Resumen.InnerText = "Resumen";
                    UpdatePanel_Filtros.Update();
                }
            }

            if (idProductoTerminado != 0)
            {
                DT.DT1.Clear();

                DT.DT1.Rows.Add("@ProductoTerminadoID", idProductoTerminado, SqlDbType.Int);
                DT.DT1.Rows.Add("@EmpleadoID", DDL_Empleado.SelectedValue, SqlDbType.Int);
                DT.DT1.Rows.Add("@CantidadMinutos", TXT_CantidadMinutos.Text, SqlDbType.Decimal);
                DT.DT1.Rows.Add("@ProductoIntermedio", 0, SqlDbType.Int);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "AgregarEmpleado", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC03_0001");
                cargarEmpleados("");
                cargarCostosProduccion();
                cargarResumen();
            }

            UpdatePanel_MateriasPrimasAsignadas.Update();
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

        protected void DGV_ListaEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idConsecutivoReceta = DGV_ListaEmpleados.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "Eliminar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDConsecutivoReceta", idConsecutivoReceta, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "EliminarEmpleado", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CC03_0001");
                    cargarMateriasPrimas("");
                    cargarEmpleados("");
                    cargarResumen();
                }
            }
        }

        protected void BTN_AsignarEmpleado_Click(object sender, EventArgs e)
        {
            agregarEmpleados();
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
            int idProductoTerminado = 0;

            foreach (ListItem l in LB_Productos.Items)
            {
                if (l.Selected)
                {
                    idProductoTerminado = Convert.ToInt32(l.Value);
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    UpdatePanel_Filtros.Update();
                }
            }

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
            int idProductoTerminado = 0;

            foreach (ListItem l in LB_Productos.Items)
            {
                if (l.Selected)
                {
                    idProductoTerminado = Convert.ToInt32(l.Value);
                    HDF_IDProductoTerminado.Value = idProductoTerminado.ToString();
                    UpdatePanel_Filtros.Update();
                }
            }

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
                        string script = "cargarFiltros();";
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
        #endregion
    }
}