﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class PedidosRecibidos : System.Web.UI.Page
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
                    cargarPedidosRecibidos();
                    // cargarDDLs();
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
        #endregion

        #region Pedidos Recibidos
        private DataTable cargarPedidosRecibidosConsulta()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarRecibidoPedidos", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP12_0001");
        }

        private void cargarPedidosRecibidos()
        {
            Result = cargarPedidosRecibidosConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPedidosRecibidos", "alertifywarning('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                    return;
                }
                else
                {
                    DGV_ListaPedidosRecibidos.DataSource = Result;
                    DGV_ListaPedidosRecibidos.DataBind();
                    UpdatePanel_ListaPedidosRecibidos.Update();
                }
            }
            else
            {
                DGV_ListaPedidosRecibidos.DataSource = Result;
                DGV_ListaPedidosRecibidos.DataBind();
                UpdatePanel_ListaPedidosRecibidos.Update();
            }
        }

        protected void TXT_FiltrarPedidosRecibidos_OnTextChanged(object sender, EventArgs e)
        {
            cargarPedidosRecibidos();
        }

        protected void DGV_ListaPedidosRecibidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string idPedidoRecibido = DGV_ListaPedidosRecibidos.DataKeys[rowIndex].Value.ToString().Trim();

                if (e.CommandName == "VerDetalle")
                {
                    Session["IDRecibidoPedido"] = idPedidoRecibido;
                    Response.Redirect("DetallePedidoRecibido.aspx", true);
                }
            }
        }

        protected void DGV_ListaPedidosRecibidos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarPedidosRecibidosConsulta();

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
                    DGV_ListaPedidosRecibidos.DataSource = Result;
                    DGV_ListaPedidosRecibidos.DataBind();
                    UpdatePanel_ListaPedidosRecibidos.Update();
                }
            }
            else
            {
                DGV_ListaPedidosRecibidos.DataSource = Result;
                DGV_ListaPedidosRecibidos.DataBind();
                UpdatePanel_ListaPedidosRecibidos.Update();
            }
        }

        protected void DGV_ListaPedidosRecibidos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                string estado = rowView["Estado"].ToString().Trim();
            }
        }
        #endregion
    }
}