﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Insumos : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Insumos", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Insumos.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    cargarDDLs();
                    cargarInsumos();                    
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
            TXT_FechaCreacionDesde.Text = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia; 

            UpdatePanel_FiltrosInsumos.Update();
        }
        #endregion

        #region Insumos
        private DataTable cargarInsumosConsulta()
        {
            DT.DT1.Clear();
            #region Fechas
            string fechaInsumoDesde = "1900-01-01";

            try
            {
                fechaInsumoDesde = Convert.ToDateTime(TXT_FechaCreacionDesde.Text).ToString();    
            }
            catch (Exception e)
            {
                fechaInsumoDesde = "1900-01-01";
            }

            DT.DT1.Rows.Add("@fechaInsumoDesde", fechaInsumoDesde, SqlDbType.Date);
            #endregion
            
            UpdatePanel_FiltrosInsumos.Update();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarInsumos", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");
        }

        private void cargarInsumos()
        {
            Result = cargarInsumosConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarInsumos", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaInsumos.DataSource = Result;
                    DGV_ListaInsumos.DataBind();
                    UpdatePanel_ListaInsumos.Update();
                }
            }
            else
            {
                DGV_ListaInsumos.DataSource = Result;
                DGV_ListaInsumos.DataBind();
                UpdatePanel_ListaInsumos.Update();
            }
        }

        protected void TXT_FiltrarInsumos_OnTextChanged(object sender, EventArgs e)
        {
            cargarInsumos();
        }
          
        protected void DGV_ListaInsumos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idInsumo = DGV_ListaInsumos.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDInsumo"] = idInsumo;
                    Response.Redirect("DetalleInsumo.aspx", true);
                }
                else if (e.CommandName == "Eliminar") 
                {
                    if (ClasePermiso.Permiso("Eliminar", "Acciones", "Eliminar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0)
                    {
                        HDF_IDInsumo.Value = idInsumo;
                        UpdatePanel_EliminarInsumo.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaInsumos_RowCommand", "abrirModalEliminarInsumo();", true);
                    }
                    else
                    {
                        HDF_IDInsumo.Value = "0";
                        UpdatePanel_EliminarInsumo.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptDGV_ListaInsumos_RowCommand", "alertifywarning('No tiene permisos para realizar esta acción.');", true);
                    }
                }
            }
        }

        protected void DGV_ListaInsumos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarInsumosConsulta();

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
                    DGV_ListaInsumos.DataSource = Result;
                    DGV_ListaInsumos.DataBind();
                    UpdatePanel_ListaInsumos.Update();
                }
            }
            else
            {
                DGV_ListaInsumos.DataSource = Result;
                DGV_ListaInsumos.DataBind();
                UpdatePanel_ListaInsumos.Update();
            }
        }

        protected void DGV_ListaInsumos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                // string estado = rowView["Estado"].ToString().Trim();
            }
        }

        protected void BTN_CrearInsumos_Click(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CrearInsumo", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearInsumos_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    Session["IDInsumo"] = Result.Rows[0][1].ToString().Trim();
                    Session["NuevoInsumo"] = Result.Rows[0][1].ToString().Trim();
                    Response.Redirect("DetalleInsumo.aspx", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearInsumos_Click", "alertifywarning('No se ha podido crear la orden de insumo, por favor intente de nuevo.');", true);
                return;
            }
        }

        protected void BTN_EliminarInsumo_Click(object sender, EventArgs e)
        {
            if (ClasePermiso.Permiso("Eliminar", "Acciones", "Eliminar", Convert.ToInt32(Session["UserId"].ToString().Trim())) > 0)
            {
                DT.DT1.Clear();

                DT.DT1.Rows.Add("@IDInsumo", HDF_IDInsumo.Value, SqlDbType.Int);
                DT.DT1.Rows.Add("@UsuarioID", Session["UserId"].ToString(), SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "EliminarInsumo", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        HDF_IDInsumo.Value = "0";
                        UpdatePanel_EliminarInsumo.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarInsumo_Click", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        HDF_IDInsumo.Value = "0";
                        UpdatePanel_EliminarInsumo.Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarInsumo_Click", "alertifysuccess('Insumo eliminado con éxito');cerrarModalEliminarInsumo();", true);
                        cargarInsumos();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarInsumo_Click", "alertifywarning('No se ha podido eliminar la orden de insumo, por favor intente de nuevo.');", true);
                    return;
                }
            }
            else
            {
                HDF_IDInsumo.Value = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_EliminarInsumo_Clickd", "alertifywarning('No tiene permisos para realizar esta acción.');", true);
            }
        }
        #endregion
    }
}