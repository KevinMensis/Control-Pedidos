using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class GestionUsuarios : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Gestión de usuarios", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Gestión de usuarios.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    cargarFiltros();
                    cargarPermisos();
                    cargarUsuarios("");
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarUsuarios("");
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
        
        private void cargarFiltros()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarRol", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_Rol_001");

            LB_Rol.DataSource = Result;
            LB_Rol.DataTextField = "DescripcionRol";
            LB_Rol.DataValueField = "IDRol";
            LB_Rol.DataBind();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Roles", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_Rol_001");

            DDL_Rol.DataSource = Result;
            DDL_Rol.DataTextField = "DescripcionRol";
            DDL_Rol.DataValueField = "IDRol";
            DDL_Rol.DataBind();
        }
        
        private void cargarPermisos()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPermisos", SqlDbType.VarChar);
            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");
            DDL_Modulo.DataSource = Result;
            DDL_Modulo.DataTextField = "Modulo";
            DDL_Modulo.DataValueField = "Modulo";
            DDL_Modulo.DataBind();
        }
        #endregion

        #region Usuarios
        #region Mantenimiento usuarios
        private DataTable cargarUsuariosConsulta()
        {
            DT.DT1.Clear();

            string roles = "";

            #region Categorias
            foreach (ListItem l in LB_Rol.Items)
            {
                if (l.Selected)
                {
                    roles += "'" + l.Value + "',";
                }
            }
            roles = roles.TrimEnd(',');
            if (roles != "")
            {
                DT.DT1.Rows.Add("@FiltrarRol", 1, SqlDbType.Int);
            }
            #endregion

            DT.DT1.Rows.Add("@RolFiltro", roles, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Buscar", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarUsuariosAll", SqlDbType.VarChar);
            
            UpdatePanel_FiltrosUsuarios.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");
        }

        private void ActivarDesactivarUsuario(int idUsuario, int estado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", idUsuario, SqlDbType.Int);
            DT.DT1.Rows.Add("@Activo", estado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActivarDesactivarUsuario", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    Result = cargarUsuariosConsulta();
                    DGV_ListaUsuarios.DataSource = Result;
                    DGV_ListaUsuarios.DataBind();
                    UpdatePanel_ListaUsuarios.Update();
                }
            }
            else
            {
                Result = cargarUsuariosConsulta();
                DGV_ListaUsuarios.DataSource = Result;
                DGV_ListaUsuarios.DataBind();
                UpdatePanel_ListaUsuarios.Update();
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivarDesactivarUsuario", script, true);
        }

        private void cargarUsuarios(string ejecutar)
        {
            Result = cargarUsuariosConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaUsuarios.DataSource = Result;
                    DGV_ListaUsuarios.DataBind();
                    UpdatePanel_ListaUsuarios.Update();
                }
            }
            else
            {
                DGV_ListaUsuarios.DataSource = Result;
                DGV_ListaUsuarios.DataBind();
                UpdatePanel_ListaUsuarios.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarUsuarios", script, true);
        }

        protected void FiltrarUsuarios_OnClick(object sender, EventArgs e)
        {
            Result = cargarUsuariosConsulta();

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    DGV_ListaUsuarios.DataSource = Result;
                    DGV_ListaUsuarios.DataBind();
                    UpdatePanel_ListaUsuarios.Update();
                }
            }
            else
            {
                DGV_ListaUsuarios.DataSource = Result;
                DGV_ListaUsuarios.DataBind();
                UpdatePanel_ListaUsuarios.Update();
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptFiltrarUsuarios_OnClick", script, true);
        }

        protected void DGV_ListaUsuarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarUsuariosConsulta();

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
                    DGV_ListaUsuarios.DataSource = Result;
                    DGV_ListaUsuarios.DataBind();
                    UpdatePanel_ListaUsuarios.Update();
                    string script = "cargarFiltros();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarUsuarios", script, true);
                }
            }
            else
            {
                DGV_ListaUsuarios.DataSource = Result;
                DGV_ListaUsuarios.DataBind();
                UpdatePanel_ListaUsuarios.Update();
            }
        }

        protected void DGV_ListaUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idUsuario = Convert.ToInt32(DGV_ListaUsuarios.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "activar")
                {
                    ActivarDesactivarUsuario(idUsuario, 1);
                }
                else if (e.CommandName == "desactivar")
                {
                    ActivarDesactivarUsuario(idUsuario, 0);
                }
                else if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDUsuario", idUsuario, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarUsuario", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDUsuario.Value = dr["IDUsuario"].ToString().Trim();
                            TXT_NombreUsuario.Text = dr["Nombre"].ToString().Trim();
                            DDL_Rol.SelectedValue = dr["RolID"].ToString().Trim();
                            TXT_Usuario.Text = dr["Usuario"].ToString().Trim();
                            TXT_Cargo.Text = dr["Cargo"].ToString().Trim();
                            TXT_Contrasena.Text = dr["Contrasena"].ToString().Trim();
                            TXT_ConfirmarContrasena.Text = dr["Contrasena"].ToString().Trim();
                            TXT_Usuario.Enabled = false;
                        }
                    }

                    title_CrearPedido.InnerHtml = "Editar Usuario";
                    UpdatePanel_ModalCrearUsuario.Update();

                    string script = "abrirModalCrearUsuario();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearUsuario_OnClick", script, true);
                }
                else if (e.CommandName == "permisos") 
                {
                    HDF_IDUsuarioPermisos.Value = idUsuario.ToString().Trim();
                    title_Permisos.InnerHtml = "Asignar permisos al usuario: " + DGV_ListaUsuarios.DataKeys[rowIndex].Values[5].ToString().Trim();

                    verPermisosUsuario();

                    string script = "abrirModalPermisosUsuario();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_PermisosUsuario_OnClick", script, true);
                }
            }
        }

        protected void DGV_ListaUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);
                bool estado = Convert.ToBoolean(DGV_ListaUsuarios.DataKeys[rowIndex].Values[1]);

                Button BTN_Activar = e.Row.Cells[0].FindControl("BTN_Activar") as Button;
                Button BTN_Eliminar = e.Row.Cells[0].FindControl("BTN_Eliminar") as Button;

                BTN_Activar.Visible = !estado;
                BTN_Eliminar.Visible = estado;
            }
        }

        protected void BTN_CrearUsuario_OnClick(object sender, EventArgs e)
        {
            HDF_IDUsuario.Value = "0";
            TXT_NombreUsuario.Text = "";
            DDL_Rol.SelectedValue = "0";
            TXT_Cargo.Text = "";
            TXT_Usuario.Text = "";
            TXT_Contrasena.Text = "";
            TXT_ConfirmarContrasena.Text = "";
            TXT_Usuario.Enabled = true;

            title_CrearPedido.InnerHtml = "Crear Usuario";
            UpdatePanel_ModalCrearUsuario.Update();

            string script = "abrirModalCrearUsuario();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearUsuario_OnClick", script, true);
        }

        protected void BTN_GuardarUsuario_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", HDF_IDUsuario.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@Nombre", TXT_NombreUsuario.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@RolID", DDL_Rol.SelectedValue, SqlDbType.Int);
            DT.DT1.Rows.Add("@Cargo", TXT_Cargo.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CorreoUsuario", TXT_Usuario.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Contrasena", TXT_Contrasena.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            if (HDF_IDUsuario.Value == "0")
                DT.DT1.Rows.Add("@TipoSentencia", "CrearUsuario", SqlDbType.VarChar);
            else
                DT.DT1.Rows.Add("@TipoSentencia", "EditarUsuario", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");
            string script = "";
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    script += "alertifyerror('No se ha guardado el usuario. Error: " + Result.Rows[0][1].ToString().Trim() + "');";
                    cargarUsuarios(script);
                }
                else
                {
                    script += "cerrarModalCrearUsuario();alertifysuccess('Se ha guardado el usuario con éxito.')";
                    cargarUsuarios(script);
                }
            }
            else
            {
                script += "alertifywarning('No se ha guardado el Usuario. Por favor, intente nuevamente');";
                cargarUsuarios(script);
            }
        }
        #endregion

        #region Permisos
        private void verPermisosUsuario()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@Modulo", DDL_Modulo.SelectedValue, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "PermisosSinAsignar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

            DGV_PermisosSinAsignar.DataSource = Result;
            DGV_PermisosSinAsignar.DataBind();

            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@Modulo", DDL_Modulo.SelectedValue, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "PermisosAsignados", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

            DGV_PermisosAsignados.DataSource = Result;
            DGV_PermisosAsignados.DataBind();
            UpdatePanel_ModalPermisosUsuario.Update();
            UpdatePanel_TablaPermisos.Update();
        }
        
        protected void DDL_Modulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            verPermisosUsuario();
        }

        #region Permisos sin asignar
        protected void DGV_PermisosSinAsignar_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@Modulo", DDL_Modulo.SelectedValue, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "PermisosSinAsignar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

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
                    DGV_PermisosSinAsignar.DataSource = Result;
                    DGV_PermisosSinAsignar.DataBind();
                    UpdatePanel_ModalPermisosUsuario.Update();
                }
            }
            else
            {
                DGV_PermisosSinAsignar.DataSource = Result;
                DGV_PermisosSinAsignar.DataBind();
                UpdatePanel_ModalPermisosUsuario.Update();
            }
        }

        protected void DGV_PermisosSinAsignar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                asignarPermiso(rowIndex);
            }
        }

        protected void DGV_PermisosSinAsignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer;";                
            }
        }

        private void asignarPermiso(int index)
        {
            int idPermiso = Convert.ToInt32(DGV_PermisosSinAsignar.DataKeys[index].Value.ToString().Trim());
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@IDPermiso", idPermiso, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "AsignarPermiso", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

            verPermisosUsuario();
        }
        #endregion

        #region Permisos asignados
        protected void DGV_PermisosAsignados_Sorting(object sender, GridViewSortEventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@Modulo", DDL_Modulo.SelectedValue, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "PermisosAsignados", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");
                                              
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
                    DGV_PermisosAsignados.DataSource = Result;
                    DGV_PermisosAsignados.DataBind();
                    UpdatePanel_ModalPermisosUsuario.Update();
                }
            }
            else
            {
                DGV_PermisosAsignados.DataSource = Result;
                DGV_PermisosAsignados.DataBind();
                UpdatePanel_ModalPermisosUsuario.Update();
            }
        }

        protected void DGV_PermisosAsignados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                eliminarPermiso(rowIndex);
            }
        }

        protected void DGV_PermisosAsignados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer;";
            }
        }

        private void eliminarPermiso(int index)
        {
            int idLinea = Convert.ToInt32(DGV_PermisosAsignados.DataKeys[index].Values[1].ToString().Trim());
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDUsuario", Convert.ToInt32(HDF_IDUsuarioPermisos.Value), SqlDbType.Int);
            DT.DT1.Rows.Add("@IDLinea", idLinea, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "EliminarPermiso", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

            verPermisosUsuario();
        }
        #endregion
        #endregion
        #endregion
    }
}                                             