using ClosedXML.Excel;
using MCWebHogar.ControlPedidos;
using MCWebHogar.ControlPedidos.Proveedores;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.GestionProveedores
{
    public partial class Proveedores : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();
        string identificacionReceptor = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            identificacionReceptor = Session["IdentificacionReceptor"].ToString().Trim();
            
            if (!Page.IsPostBack)
            {
                if (identificacionReceptor == "3101485961")
                {
                    li_MiKFe.Attributes.Add("class", "active");
                    H1_Title.InnerText = "La Priedra Calisa SA - Proveedores";
                }
                else if (identificacionReceptor == "115210651")
                {
                    li_Esteban.Attributes.Add("class", "active");
                    H1_Title.InnerText = "Panadería La Central - Proveedores";
                }
                if (Session["UserId"] == null)
                {
                    Response.Redirect("../Default.aspx", true);
                }
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Proveedores", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Proveedores.");
                    Response.Redirect("../ControlPedidos/Pedido.aspx");
                }
                else
                {
                    cargarEmisores("");
                    HDF_IDUsuario.Value = Session["Usuario"].ToString();
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarEmisores("");
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

        protected void BTN_Sincronizar_Click(object sender, EventArgs e)
        {
            LecturaXML xml = new LecturaXML();
            UpdatePanel_Progress.Update();
            xml.ReadEmails();
            UpdatePanel_Progress.Update();
            xml.ReadXML();
            cargarEmisores("");
            string script = "desactivarloading();alertifysuccess('Sincronizacion completada');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_Sincronizar_Click", script, true);
        }
        #endregion 
      
        #region Emisores
        private DataTable cargarEmisoresConsulta(string consulta)
        {
            DT.DT1.Clear();

            //string categorias = "";
            //string categoriasText = "";
            //LBL_Filtro.Text = "Filtros: ";

            //#region Categorias
            //foreach (ListItem l in LB_Categoria.Items)
            //{
            //    if (l.Selected)
            //    {
            //        categorias += "'" + l.Value + "',";
            //        categoriasText += l.Text + ",";
            //    }
            //}
            //categorias = categorias.TrimEnd(',');
            //categoriasText = categoriasText.TrimEnd(',');
            //if (categoriasText != "")
            //{
            //    // LBL_Filtro.Text += " Categoría=" + categoriasText + "; ";
            //    DT.DT1.Rows.Add("@FiltrarCategoria", 1, SqlDbType.Int);
            //}
            //#endregion

            #region Descripcion
            if (TXT_Buscar.Text != "")
            {
                // LBL_Filtro.Text += " Descripción=" + TXT_Buscar.Text + "; ";
            }
            #endregion

            //DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NombreComercial", TXT_Buscar.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@IDEmisor", HDF_IDEmisor.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            //  if (LBL_Filtro.Text == "Filtros: ")
            //  {
            //      LBL_Filtro.Text += "Ninguno;";
            //  }

            UpdatePanel_FiltrosEmisores.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");
        }

        private void cargarEmisores(string ejecutar)
        {
            Result = cargarEmisoresConsulta("CargarEmisoresAll");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaEmisores.DataSource = Result;
                    DGV_ListaEmisores.DataBind();
                    UpdatePanel_ListaEmisores.Update();
                }
            }
            else
            {
                DGV_ListaEmisores.DataSource = Result;
                DGV_ListaEmisores.DataBind();
                UpdatePanel_ListaEmisores.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarEmisores", script, true);
        }

        private void ActivarDesactivarEmisor(int idEmisor, int estado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDEmisor", idEmisor, SqlDbType.Int);
            DT.DT1.Rows.Add("@Activo", estado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActivarDesactivarEmisor", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    cargarEmisores("");
                }
            }
            else
            {
                cargarEmisores("");
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivarDesactivarEmisor", script, true);
        }

        protected void DGV_ListaEmisores_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarEmisoresConsulta("CargarEmisoresAll");

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
                    DGV_ListaEmisores.DataSource = Result;
                    DGV_ListaEmisores.DataBind();
                    UpdatePanel_ListaEmisores.Update();
                }
            }
            else
            {
                DGV_ListaEmisores.DataSource = Result;
                DGV_ListaEmisores.DataBind();
                UpdatePanel_ListaEmisores.Update();
            }
        }

        protected void DGV_ListaEmisores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idEmisor = Convert.ToInt32(DGV_ListaEmisores.DataKeys[rowIndex].Value.ToString().Trim());
                string nombreComercial = DGV_ListaEmisores.DataKeys[rowIndex].Values[5].ToString().Trim();

                if (e.CommandName == "activar")
                {
                    ActivarDesactivarEmisor(idEmisor, 1);
                }
                else if (e.CommandName == "desactivar")
                {
                    ActivarDesactivarEmisor(idEmisor, 0);
                }  
                else if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDEmisor", idEmisor, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarDetalleEmisor", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDEmisor.Value = dr["IDEmisor"].ToString().Trim();
                            TXT_NombreComercial.Text = dr["NombreComercial"].ToString().Trim();
                            TXT_NumeroIdentificacion.Text = dr["NumeroIdentificacion"].ToString().Trim();
                            TXT_CorreoEmisor.Text = dr["CorreoEmisor"].ToString().Trim();
                            TXT_Provincia.Text = dr["Provincia"].ToString().Trim();
                            TXT_Canton.Text = dr["Canton"].ToString().Trim();
                            TXT_Distrito.Text = dr["Distrito"].ToString().Trim();
                            TXT_Barrio.Text = dr["Barrio"].ToString().Trim();
                            TXT_Telefono.Text = dr["Telefono"].ToString().Trim();
                            TXT_OtrasSenas.Text = dr["OtrasSenas"].ToString().Trim();
                        }
                    }
                    
                    // cargarUnidadesMedida();
                    UpdatePanel_ModalEditarEmisor.Update();

                    string script = "abrirModalEditarEmisor();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaEmisores_RowCommand", script, true);
                } 
                else if (e.CommandName == "VerFacturas") 
                {
                    HDF_IDEmisor.Value = idEmisor.ToString();
                    UpdatePanel_ModalEditarEmisor.Update();
                    H1.InnerText = nombreComercial;
                    cargarFacturas("abrirModalVerFacturas();");
                }
            }
        }

        protected void DGV_ListaEmisores_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);
                bool estado = Convert.ToBoolean(DGV_ListaEmisores.DataKeys[rowIndex].Values[1]);

                Button BTN_Activar = e.Row.Cells[0].FindControl("BTN_Activar") as Button;
                Button BTN_Eliminar = e.Row.Cells[0].FindControl("BTN_Eliminar") as Button;
                LinkButton BTN_VerFacturas = e.Row.Cells[0].FindControl("BTN_VerFacturas") as LinkButton;

                BTN_Activar.Visible = !estado;
                BTN_VerFacturas.Visible = estado;
                BTN_Eliminar.Visible = estado;
            }
        }

        protected void FiltrarEmisores_OnClick(object sender, EventArgs e)
        {
            Result = cargarEmisoresConsulta("CargarEmisoresAll");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaEmisores.DataSource = Result;
                    DGV_ListaEmisores.DataBind();
                    UpdatePanel_ListaEmisores.Update();
                }
            }
            else
            {
                DGV_ListaEmisores.DataSource = Result;
                DGV_ListaEmisores.DataBind();
                UpdatePanel_ListaEmisores.Update();
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptFiltrarEmisores_OnClick", script, true);
        }
          
        [WebMethod()]
        public static string BTN_GuardarEmisor_Click(string idEmisor, string numeroIdentificacion, string nombreComercial, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDEmisor", idEmisor, SqlDbType.Int);
            DT.DT1.Rows.Add("@NumeroIdentificacion", numeroIdentificacion, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NombreComercial", nombreComercial, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Actualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");            
            return "correcto";
        }

        [WebMethod()]
        public static string BTN_GuardarUnidadesMedida_Click(string idEmisor, int idUnidadMedida, int cantidadEquivalente, string usuario)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@EmisorID", idEmisor, SqlDbType.Int);
            DT.DT1.Rows.Add("@IDUnidadMedida", idUnidadMedida, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CantidadEquivalente", cantidadEquivalente, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Actualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0002");
            return "correcto";
        }

        private void descargarReporteEmisores()
        {
            DataTable ResultUnidadesMedida = new DataTable();
            DataTable ResultFacturas = new DataTable();
            DataTable ResultProductos = new DataTable();
            DataTable ResultHistorico = new DataTable();

            Result = cargarEmisoresConsulta("CargarEmisoresReporte");
            ResultUnidadesMedida = cargarEmisoresConsulta("ReporteUnidadesMedidaEmisor");
            ResultFacturas = cargarEmisoresConsulta("ReporteFacturasEmisor");
            ResultProductos = cargarEmisoresConsulta("ReporteProductosEmisor");
            ResultHistorico = cargarEmisoresConsulta("ReporteHistoricoEmisor");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarEmisor_OnClick", "desactivarloading();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Proveedores");
                wb.Worksheets.Add(ResultUnidadesMedida, "UnidadesMedida");
                wb.Worksheets.Add(ResultFacturas, "FacturasProveedor");
                wb.Worksheets.Add(ResultProductos, "ProductosProveedor");
                wb.Worksheets.Add(ResultHistorico, "HistoricoProveedor");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Emisores.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            UpdatePanel_ListaEmisores.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarEmisor_OnClick", "desactivarloading();cargarFiltros();", true);
        }

        protected void BTN_DescargarEmisor_OnClick(object sender, EventArgs e)
        {
            descargarReporteEmisores();
        }

        protected void BTN_DescargarEmisores_OnClick(object sender, EventArgs e)
        {
            HDF_IDEmisor.Value = "0";
            UpdatePanel_ModalEditarEmisor.Update();
            descargarReporteEmisores();
        }

        #region Unidades de Medida
        private DataTable cargarUnidadesMedidaConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@EmisorID", HDF_IDEmisor.Value, SqlDbType.VarChar); 
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_ModalEditarEmisor.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0002");
        }

        private void cargarUnidadesMedida()
        {
            Result = cargarUnidadesMedidaConsulta("CargarUnidadesMedida");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_UnidadesMedida.DataSource = Result;
                    DGV_UnidadesMedida.DataBind();
                    UpdatePanel_ModalEditarEmisor.Update();
                }
            }
            else
            {
                DGV_UnidadesMedida.DataSource = Result;
                DGV_UnidadesMedida.DataBind();
                UpdatePanel_ModalEditarEmisor.Update();
            }            
        }
        #endregion

        #region Facturas
        private DataTable cargarFacturasConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@FiltrarEmisor", 1, SqlDbType.Int);            
            DT.DT1.Rows.Add("@EmisoresFiltro", HDF_IDEmisor.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", identificacionReceptor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroConsecutivoFactura", "", SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_ListaFacturas.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP02_0001");
        }

        private void cargarFacturas(string ejecutar)
        {
            Result = cargarFacturasConsulta("ReporteFacturasEmisor");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaFacturas.DataSource = Result;
                    DGV_ListaFacturas.DataBind();
                    UpdatePanel_VerFacturas.Update();
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_VerFacturas.Update();
                UpdatePanel_ListaFacturas.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void DGV_ListaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idFactura = Convert.ToInt32(DGV_ListaFacturas.DataKeys[rowIndex].Value.ToString().Trim());
                string numeroConsecutivo = DGV_ListaFacturas.DataKeys[rowIndex].Values[2].ToString().Trim();
                string fechaFactura = DGV_ListaFacturas.DataKeys[rowIndex].Values[3].ToString().Trim();
                string fechaSincronizacion = DGV_ListaFacturas.DataKeys[rowIndex].Values[4].ToString().Trim();
                string nombreComercial = DGV_ListaFacturas.DataKeys[rowIndex].Values[5].ToString().Trim();
                decimal totalVenta = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[6].ToString().Trim());
                decimal totalDescuento = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[7].ToString().Trim());
                decimal totalImpuesto = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[8].ToString().Trim());
                decimal totalComprobante = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[9].ToString().Trim());

                if (e.CommandName == "VerProductos")
                {
                    HDF_IDFactura.Value = idFactura.ToString();

                    TXT_NombreComercialFactura.Text = nombreComercial;
                    TXT_NumeroConsecutivo.Text = numeroConsecutivo;
                    TXT_FechaFactura.Text = fechaFactura;
                    TXT_FechaSincronizacion.Text = fechaSincronizacion;
                    TXT_TotalVenta.Text = String.Format("{0:n}", totalVenta);
                    TXT_TotalDescuento.Text = String.Format("{0:n}", totalDescuento);
                    TXT_TotalImpuesto.Text = String.Format("{0:n}", totalImpuesto);
                    TXT_TotalComprobante.Text = String.Format("{0:n}", totalComprobante);
                    cargarProductos("cerrarModalVerFacturas();abrirModalVerProductos();");
                }
            }
        }

        #region Productos
        private DataTable cargarProductosConsulta(string consulta)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@FiltrarFactura", 1, SqlDbType.Int);
            DT.DT1.Rows.Add("@FacturasFiltro", HDF_IDFactura.Value, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            UpdatePanel_ListaProductos.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");
        }

        private void cargarProductos(string ejecutar)
        {
            Result = cargarProductosConsulta("CargarProductosAll");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_VerProductos.Update();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_VerProductos.Update();
                UpdatePanel_ListaProductos.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }
        #endregion
        #endregion
        #endregion
    }
}