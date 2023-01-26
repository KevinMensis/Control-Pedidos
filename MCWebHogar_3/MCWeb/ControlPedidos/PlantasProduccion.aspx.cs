using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class PlantasProduccion : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Planta de producción", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Planta de producción.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    cargarPlantasProduccion("");
                    ViewState["Ordenamiento"] = "ASC";                    
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarPlantasProduccion("");
                }
                else if (opcion.Contains("Identificacion"))
                {
                    string identificacion = opcion.Split(';')[1];
                    Session["IdentificacionReceptor"] = identificacion;
                    Response.Redirect("../GestionProveedores/Proveedores.aspx", true);
                }
                if (opcion.Contains("Receta"))
                {
                    string negocio = opcion.Split(';')[1];
                    Session["RecetaNegocio"] = negocio;
                    Response.Redirect("../GestionCostos/CrearReceta.aspx", true);
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

        #region Plantas Produccion
        private DataTable cargarPlantasProduccionConsulta()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPlantasProduccionAll", SqlDbType.VarChar);

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");
        }

        private void ActivarDesactivarPlantaProduccion(int idPlantaProduccion, int estado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDPlantaProduccion", idPlantaProduccion, SqlDbType.Int);
            DT.DT1.Rows.Add("@Activo", estado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActivarDesactivarPlantaProduccion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    Result = cargarPlantasProduccionConsulta();
                    DGV_ListaPlantasProduccion.DataSource = Result;
                    DGV_ListaPlantasProduccion.DataBind();
                    UpdatePanel_ListaPlantasProduccion.Update();
                }
            }
            else
            {
                Result = cargarPlantasProduccionConsulta();
                DGV_ListaPlantasProduccion.DataSource = Result;
                DGV_ListaPlantasProduccion.DataBind();
                UpdatePanel_ListaPlantasProduccion.Update();
            }

            string script = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivarDesactivarPlantaProduccion", script, true);
        }

        private void cargarPlantasProduccion(string ejecutar)
        {
            Result = cargarPlantasProduccionConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaPlantasProduccion.DataSource = Result;
                    DGV_ListaPlantasProduccion.DataBind();
                    UpdatePanel_ListaPlantasProduccion.Update();
                }
            }
            else
            {
                DGV_ListaPlantasProduccion.DataSource = Result;
                DGV_ListaPlantasProduccion.DataBind();
                UpdatePanel_ListaPlantasProduccion.Update();
            }
            string script = "" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarPlantasProduccion", script, true);
        }

        protected void TXT_Buscar_OnTextChanged(object sender, EventArgs e)
        {
            cargarPlantasProduccion("");
        }

        protected void DGV_ListaPlantasProduccion_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarPlantasProduccionConsulta();

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
                    DGV_ListaPlantasProduccion.DataSource = Result;
                    DGV_ListaPlantasProduccion.DataBind();
                    UpdatePanel_ListaPlantasProduccion.Update();
                }
            }
            else
            {
                DGV_ListaPlantasProduccion.DataSource = Result;
                DGV_ListaPlantasProduccion.DataBind();
                UpdatePanel_ListaPlantasProduccion.Update();
            }
        }

        protected void DGV_ListaPlantasProduccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idPlantaProduccion = Convert.ToInt32(DGV_ListaPlantasProduccion.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "activar")
                {
                    ActivarDesactivarPlantaProduccion(idPlantaProduccion, 1);
                }
                else if (e.CommandName == "desactivar")
                {
                    ActivarDesactivarPlantaProduccion(idPlantaProduccion, 0);
                }
                else if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDPlantaProduccion", idPlantaProduccion, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarPlantaProduccion", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDPlantaProduccion.Value = dr["IDPlantaProduccion"].ToString().Trim();
                            TXT_DescripcionPlantaProduccion.Text = dr["DescripcionPlantaProduccion"].ToString().Trim();
                            TXT_UbicacionPlantaProduccion.Text = dr["UbicacionPlantaProduccion"].ToString().Trim();
                        }
                    }

                    title_CrearPlantaProduccion.InnerHtml = "Editar planta producción";
                    UpdatePanel_ModalCrearPlantaProduccion.Update();

                    string script = "abrirModalCrearPlantaProduccion();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearPlantaProduccion_OnClick", script, true);
                }
            }
        }

        protected void DGV_ListaPlantasProduccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);
                bool estado = Convert.ToBoolean(DGV_ListaPlantasProduccion.DataKeys[rowIndex].Values[1]);

                Button BTN_Activar = e.Row.Cells[0].FindControl("BTN_Activar") as Button;
                Button BTN_Eliminar = e.Row.Cells[0].FindControl("BTN_Eliminar") as Button;

                BTN_Activar.Visible = !estado;
                BTN_Eliminar.Visible = estado;
            }
        }

        protected void BTN_CrearPlantaProduccion_OnClick(object sender, EventArgs e)
        {
            HDF_IDPlantaProduccion.Value = "0";
            TXT_DescripcionPlantaProduccion.Text = "";
            TXT_UbicacionPlantaProduccion.Text = "";
            
            title_CrearPlantaProduccion.InnerHtml = "Crear planta producción";
            UpdatePanel_ModalCrearPlantaProduccion.Update();

            string script = "abrirModalCrearPlantaProduccion();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearPlantaProduccion_OnClick", script, true);
        }

        protected void BTN_GuardarPlantaProduccion_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDPlantaProduccion", HDF_IDPlantaProduccion.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@DescripcionPlantaProduccion", TXT_DescripcionPlantaProduccion.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@UbicacionPlantaProduccion", TXT_UbicacionPlantaProduccion.Text, SqlDbType.VarChar);
           
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            if (HDF_IDPlantaProduccion.Value == "0")
                DT.DT1.Rows.Add("@TipoSentencia", "CrearPlantaProduccion", SqlDbType.VarChar);
            else
                DT.DT1.Rows.Add("@TipoSentencia", "EditarPlantaProduccion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");
            string script = "";
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    script += "alertifyerror('No se ha guardado la planta de produccion. Error: " + Result.Rows[0][1].ToString().Trim() + "');";
                    cargarPlantasProduccion(script);
                }
                else
                {
                    script += "cerrarModalCrearPlantaProduccion();alertifysuccess('Se ha guardado la planta de producción con éxito.')";
                    cargarPlantasProduccion(script);
                }
            }
            else
            {
                script += "alertifywarning('No se ha guardado la planta de producción. Por favor, intente nuevamente');";
                cargarPlantasProduccion(script);
            }
        }
        #endregion
    }
}