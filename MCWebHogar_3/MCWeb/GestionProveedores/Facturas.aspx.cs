using ClosedXML.Excel;
using MCWebHogar.ControlPedidos;
using MCWebHogar.ControlPedidos.Proveedores;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.GestionProveedores
{
    public partial class Facturas : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Proveedores", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Proveedores.");
                    Response.Redirect("../ControlPedidos/Pedido.aspx");
                }
                else
                {
                    cargarFiltros();
                    cargarFacturas("");
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarFacturas("");
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        private void cargarFiltros()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmisores", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

            LB_Emisores.DataSource = Result;
            LB_Emisores.DataTextField = "NombreComercial";
            LB_Emisores.DataValueField = "IDEmisor";
            LB_Emisores.DataBind();

            #region Fechas
            DateTime hoy = DateTime.Now;
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            TXT_FechaDesde.Text = Convert.ToString(hoy.Year) + "-01-01";
            TXT_FechaHasta.Text = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
            #endregion
        }

        protected void BTN_Sincronizar_Click(object sender, EventArgs e)
        {
            LecturaXML xml = new LecturaXML();
            xml.ReadEmails();
            xml.ReadXML();
            cargarFacturas("");
            string script = "desactivarloading();alertifysuccess('Sincronizacion completada');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_Sincronizar_Click", script, true);
        }

        protected void Recargar_Click(object sender, EventArgs e)
        {
            cargarFacturas("");
        }
        #endregion   
    
        #region Facturas
        private DataTable cargarFacturasConsulta(string consulta)
        {
            DT.DT1.Clear();

            #region Fechas
            DateTime hoy = DateTime.Now;
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            string fechaDesde = "1900-01-01";
            string fechaHasta = "1900-01-01";

            try
            {
                fechaDesde = Convert.ToDateTime(TXT_FechaDesde.Text).ToString();
            }
            catch (Exception e)
            {
                fechaDesde = Convert.ToString(hoy.Year) + "-01-01";
            }
            try
            {
                hoy = Convert.ToDateTime(TXT_FechaHasta.Text);
                mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
                dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
                fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
            }
            catch (Exception e)
            {
                fechaHasta = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
            }

            DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
            DT.DT1.Rows.Add("@fechaHasta", fechaHasta + " 23:59:59", SqlDbType.DateTime);
            #endregion

            string emisores = "";
            string emisoresText = "";
            // LBL_Filtro.Text = "Filtros: ";

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
                // LBL_Filtro.Text += " Categoría=" + categoriasText + "; ";
                DT.DT1.Rows.Add("@FiltrarEmisor", 1, SqlDbType.Int);
            }
            #endregion

            #region Descripcion
            if (TXT_Buscar.Text != "")
            {
                // LBL_Filtro.Text += " Descripción=" + TXT_Buscar.Text + "; ";
            }
            #endregion

            DT.DT1.Rows.Add("@EmisoresFiltro", emisores, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroConsecutivoFactura", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", consulta, SqlDbType.VarChar);

            //  if (LBL_Filtro.Text == "Filtros: ")
            //  {
            //      LBL_Filtro.Text += "Ninguno;";
            //  }

            UpdatePanel_FiltrosFacturas.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "GP02_0001");
        }

        private void cargarFacturas(string ejecutar)
        {
            Result = cargarFacturasConsulta("CargarFacturasAll");

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
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_ListaFacturas.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void FiltrarFacturas_OnClick(object sender, EventArgs e)
        {
            Result = cargarFacturasConsulta("CargarFacturasAll");

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
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_ListaFacturas.Update();
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptFiltrarFacturas_OnClick", script, true);
        }

        protected void DGV_ListaFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarFacturasConsulta("CargarFacturasAll");

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
                    DGV_ListaFacturas.DataSource = Result;
                    DGV_ListaFacturas.DataBind();
                    UpdatePanel_ListaFacturas.Update();
                }
            }
            else
            {
                DGV_ListaFacturas.DataSource = Result;
                DGV_ListaFacturas.DataBind();
                UpdatePanel_ListaFacturas.Update();
            }
            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaFacturas_Sorting", script, true);
        }

        protected void DGV_ListaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idFactura = Convert.ToInt32(DGV_ListaFacturas.DataKeys[rowIndex].Value.ToString().Trim());
                string numeroConsecutivo = DGV_ListaFacturas.DataKeys[rowIndex].Values[3].ToString().Trim();
                string fechaFactura = DGV_ListaFacturas.DataKeys[rowIndex].Values[4].ToString().Trim();
                string fechaSincronizacion = DGV_ListaFacturas.DataKeys[rowIndex].Values[5].ToString().Trim();
                string nombreComercial = DGV_ListaFacturas.DataKeys[rowIndex].Values[6].ToString().Trim();
                decimal totalVenta = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[7].ToString().Trim());
                decimal totalDescuento = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[8].ToString().Trim());
                decimal totalImpuesto = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[9].ToString().Trim());
                decimal totalComprobante = Convert.ToDecimal(DGV_ListaFacturas.DataKeys[rowIndex].Values[10].ToString().Trim());

                if (e.CommandName == "VerProductos")
                {
                    HDF_IDFactura.Value = idFactura.ToString();
                    
                    TXT_NombreComercial.Text = nombreComercial;
                    TXT_NumeroConsecutivo.Text = numeroConsecutivo;
                    TXT_FechaFactura.Text = fechaFactura;
                    TXT_FechaSincronizacion.Text = fechaSincronizacion;
                    TXT_TotalVenta.Text = String.Format("{0:n}", totalVenta);
                    TXT_TotalDescuento.Text = String.Format("{0:n}", totalDescuento);
                    TXT_TotalImpuesto.Text = String.Format("{0:n}", totalImpuesto);
                    TXT_TotalComprobante.Text = String.Format("{0:n}", totalComprobante);
                    cargarProductos("abrirModalVerProductos();");
                }
            }
        }

        protected void BTN_DescargarFacturas_OnClick(object sender, EventArgs e)
        {
            Result = cargarFacturasConsulta("CargarFacturasReporte");

            if (Result.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarFacturas_OnClick", "desactivarloading();alertifyerror('No hay registros para descargar.');", true);
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(Result, "Facturas");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Facturas.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            UpdatePanel_ListaFacturas.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_DescargarFacturas_OnClick", "desactivarloading();cargarFiltros();", true);
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
    }
}