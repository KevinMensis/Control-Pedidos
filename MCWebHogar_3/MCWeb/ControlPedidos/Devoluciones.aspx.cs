﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Devoluciones : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Devoluciones", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Devoluciones.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    cargarDDLs();
                    cargarDevoluciones();                    
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
            // TXT_FechaDevolucionDesde.Text = "1900-01-01";
            // TXT_FechaDevolucionHasta.Text = "1900-01-01";

            UpdatePanel_FiltrosDevoluciones.Update();
        }
        #endregion

        #region Devoluciones
        private DataTable cargarDevolucionesConsulta()
        {
            DT.DT1.Clear();

            #region Fechas
            string fechaDevolucionDesde = "1900-01-01";
            // string fechaDevolucionHasta = "1900-01-01";

            try
            {
                fechaDevolucionDesde = Convert.ToDateTime(TXT_FechaCreacionDesde.Text).ToString();
                // fechaDevolucionHasta = Convert.ToDateTime(TXT_FechaDevolucionHasta.Text).ToString();

            }
            catch (Exception e)
            {
                fechaDevolucionDesde = "1900-01-01";
                // fechaDevolucionHasta = "1900-01-01";
            }

            DT.DT1.Rows.Add("@fechaDevolucionDesde", fechaDevolucionDesde, SqlDbType.Date);
            // DT.DT1.Rows.Add("@fechaDevolucionHasta", fechaDevolucionHasta, SqlDbType.Date);
            #endregion

            UpdatePanel_FiltrosDevoluciones.Update();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarDevoluciones", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP18_0001");
        }

        private void cargarDevoluciones()
        {
            Result = cargarDevolucionesConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDevoluciones", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaDevoluciones.DataSource = Result;
                    DGV_ListaDevoluciones.DataBind();
                    UpdatePanel_ListaDevoluciones.Update();
                }
            }
            else
            {
                DGV_ListaDevoluciones.DataSource = Result;
                DGV_ListaDevoluciones.DataBind();
                UpdatePanel_ListaDevoluciones.Update();
            }
        }

        protected void TXT_FiltrarDevoluciones_OnTextChanged(object sender, EventArgs e)
        {
            cargarDevoluciones();
        }

        protected void DGV_ListaDevoluciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idDevolucion = DGV_ListaDevoluciones.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDDevolucion"] = idDevolucion;
                    Response.Redirect("DetalleDevolucion.aspx", true);
                }
                else if (e.CommandName == "Eliminar")
                {
                    if (ClasePermiso.Permiso("Eliminar", "Acciones", "Eliminar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0)
                    {
                        HDF_IDDevolucion.Value = idDevolucion;
                        UpdatePanel_EliminarDevolucion.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaDevoluciones_RowCommand", "abrirModalEliminarDevolucion();", true);
                    }
                    else
                    {
                        HDF_IDDevolucion.Value = "0";
                        UpdatePanel_EliminarDevolucion.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaDevoluciones_RowCommand", "alertifywarning('No tiene permisos para realizar esta acción.');", true);
                    }
                }
            }
        }

        protected void DGV_ListaDevoluciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarDevolucionesConsulta();

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
                    DGV_ListaDevoluciones.DataSource = Result;
                    DGV_ListaDevoluciones.DataBind();
                    UpdatePanel_ListaDevoluciones.Update();
                }
            }
            else
            {
                DGV_ListaDevoluciones.DataSource = Result;
                DGV_ListaDevoluciones.DataBind();
                UpdatePanel_ListaDevoluciones.Update();
            }
        }

        protected void DGV_ListaDevoluciones_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                // string estado = rowView["Estado"].ToString().Trim();
            }
        }

        protected void BTN_CrearDevoluciones_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearDevolucion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP18_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDevoluciones_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    Session["IDDevolucion"] = Result.Rows[0][1].ToString().Trim();
                    Session["NuevaDevolucion"] = Result.Rows[0][1].ToString().Trim();
                    Response.Redirect("DetalleDevolucion.aspx", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearDevoluciones_Click", "alertifywarning('No se ha podido crear la orden de devolución, por favor intente de nuevo.');", true);
                return;
            }
        }

        protected void BTN_EliminarDevolucion_Click(object sender, EventArgs e)
        {
            if (ClasePermiso.Permiso("Eliminar", "Acciones", "Eliminar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0)
            {
                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDDevolucion", HDF_IDDevolucion.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "EliminarDevolucion", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP18_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        HDF_IDDevolucion.Value = "0";
                        UpdatePanel_EliminarDevolucion.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarDevolucion_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        HDF_IDDevolucion.Value = "0";
                        UpdatePanel_EliminarDevolucion.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarDevolucion_Click", "alertifysuccess('Devolucion eliminado con éxito');cerrarModalEliminarDevolucion();", true);
                        cargarDevoluciones();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarDevolucion_Click", "alertifywarning('No se ha podido eliminar la orden de Devolucion, por favor intente de nuevo.');", true);
                    return;
                }
            }
            else
            {
                HDF_IDDevolucion.Value = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarDevolucion_Clickd", "alertifywarning('No tiene permisos para realizar esta acción.');", true);
            }
        }
        #endregion
    }
}