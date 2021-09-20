using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Productos : System.Web.UI.Page
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Productos", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Productos.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    cargarFiltros();
                    cargarProductos("");
                    ViewState["Ordenamiento"] = "ASC";
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                if (opcion.Contains("TXT_Buscar"))
                {
                    cargarProductos("");
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
            DT.DT1.Rows.Add("@TipoSentencia", "CargarCategorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_Categoria_001");

            LB_Categoria.DataSource = Result;
            LB_Categoria.DataTextField = "DescripcionCategoria";
            LB_Categoria.DataValueField = "IDCategoria";
            LB_Categoria.DataBind();

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Categorias", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_Categoria_001");

            DDL_Categoria.DataSource = Result;
            DDL_Categoria.DataTextField = "DescripcionCategoria";
            DDL_Categoria.DataValueField = "IDCategoria";
            DDL_Categoria.DataBind();
        }
        #endregion

        #region Productos
        private DataTable cargarProductosConsulta()
        {
            DT.DT1.Clear();

            string categorias = "";
            string categoriasText = "";
            // LBL_Filtro.Text = "Filtros: ";

            #region Categorias
            foreach (ListItem l in LB_Categoria.Items)
            {
                if (l.Selected)
                {
                    categorias += "'" + l.Value + "',";
                    categoriasText += l.Text + ",";
                }
            }
            categorias = categorias.TrimEnd(',');
            categoriasText = categoriasText.TrimEnd(',');
            if (categoriasText != "")
            {
                // LBL_Filtro.Text += " Categoría=" + categoriasText + "; ";
                DT.DT1.Rows.Add("@FiltrarCategoria", 1, SqlDbType.Int);
            }
            #endregion

            #region Descripcion
            if (TXT_Buscar.Text != "")
            {
                // LBL_Filtro.Text += " Descripción=" + TXT_Buscar.Text + "; ";
            }
            #endregion

            DT.DT1.Rows.Add("@CategoriasFiltro", categorias, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@DescripcionProducto", TXT_Buscar.Text, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarProductosAll", SqlDbType.VarChar);
            
            //  if (LBL_Filtro.Text == "Filtros: ")
            //  {
            //      LBL_Filtro.Text += "Ninguno;";
            //  }

            UpdatePanel_FiltrosProductos.Update();

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");
        }

        private void ActivarDesactivarProducto(int idProducto, int estado)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);
            DT.DT1.Rows.Add("@Activo", estado, SqlDbType.Int);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "ActivarDesactivarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    Result = cargarProductosConsulta();
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                Result = cargarProductosConsulta();
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }

            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivarDesactivarProducto", script, true);
        }

        private void cargarProductos(string ejecutar)
        {
            Result = cargarProductosConsulta();

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
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
            string script = "cargarFiltros();" + ejecutar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarProductos", script, true);
        }

        protected void FiltrarProductos_OnClick(object sender, EventArgs e)
        {
            Result = cargarProductosConsulta();

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
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
            
            string script = "cargarFiltros();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptFiltrarProductos_OnClick", script, true);
        }

        protected void DGV_ListaProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = cargarProductosConsulta();

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
                    DGV_ListaProductos.DataSource = Result;
                    DGV_ListaProductos.DataBind();
                    UpdatePanel_ListaProductos.Update();
                }
            }
            else
            {
                DGV_ListaProductos.DataSource = Result;
                DGV_ListaProductos.DataBind();
                UpdatePanel_ListaProductos.Update();
            }
        }

        protected void DGV_ListaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idProducto = Convert.ToInt32(DGV_ListaProductos.DataKeys[rowIndex].Value.ToString().Trim());

                if (e.CommandName == "activar")
                {
                    ActivarDesactivarProducto(idProducto, 1);
                }
                else if (e.CommandName == "desactivar")
                {
                    ActivarDesactivarProducto(idProducto, 0);
                }
                else if (e.CommandName == "editar")
                {
                    DT.DT1.Clear();

                    DT.DT1.Rows.Add("@IDProducto", idProducto, SqlDbType.Int);

                    DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
                    DT.DT1.Rows.Add("@TipoSentencia", "CargarProducto", SqlDbType.VarChar);

                    Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");

                    if (Result != null && Result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Rows)
                        {
                            HDF_IDProducto.Value = dr["IDProducto"].ToString().Trim();
                            TXT_DescripcionProducto.Text = dr["DescripcionProducto"].ToString().Trim();
                            DDL_Categoria.SelectedValue = dr["Categoria"].ToString().Trim();
                            TXT_CodigoBarra.Text = dr["CodigoBarra"].ToString().Trim();
                            TXT_Costo.Text = dr["Costo"].ToString().Trim();
                            TXT_PrecioVenta.Text = dr["PrecioVenta"].ToString().Trim();
                            TXT_MedidaVenta.Text = dr["MedidaVenta"].ToString().Trim();
                            TXT_MedidaProduccion.Text = dr["MedidaProduccion"].ToString().Trim();
                            TXT_UnidadMedida.Text = dr["UnidadMedida"].ToString().Trim();
                            CHK_EsEmpaque.Checked = Convert.ToBoolean(dr["EsEmpaque"]);
                            CHK_EsInsumo.Checked = Convert.ToBoolean(dr["EsInsumo"]);
                        }
                    }

                    title_CrearPedido.InnerHtml = "Editar producto";
                    UpdatePanel_ModalCrearProducto.Update();

                    string script = "abrirModalCrearProducto();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearProducto_OnClick", script, true);
                }
            }
        }

        protected void DGV_ListaProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);
                bool estado =  Convert.ToBoolean(DGV_ListaProductos.DataKeys[rowIndex].Values[1]);

                Button BTN_Activar = e.Row.Cells[0].FindControl("BTN_Activar") as Button;
                Button BTN_Eliminar = e.Row.Cells[0].FindControl("BTN_Eliminar") as Button;

                BTN_Activar.Visible = !estado;
                BTN_Eliminar.Visible = estado;
            }
        }

        protected void BTN_CrearProducto_OnClick(object sender, EventArgs e)
        {
            HDF_IDProducto.Value = "0";
            TXT_DescripcionProducto.Text = "";
            DDL_Categoria.SelectedValue = "0";
            TXT_CodigoBarra.Text = "";
            TXT_Costo.Text = "0";
            TXT_PrecioVenta.Text = "0";
            TXT_MedidaVenta.Text = "0";
            TXT_MedidaProduccion.Text = "0";
            TXT_UnidadMedida.Text = "";
            CHK_EsEmpaque.Checked = false;
            CHK_EsInsumo.Checked = false;

            title_CrearPedido.InnerHtml = "Crear producto";
            UpdatePanel_ModalCrearProducto.Update();

            string script = "abrirModalCrearProducto();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_CrearProducto_OnClick", script, true);
        }

        protected void BTN_GuardarProducto_OnClick(object sender, EventArgs e)
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@IDProducto", HDF_IDProducto.Value, SqlDbType.Int);
            DT.DT1.Rows.Add("@DescripcionProducto", TXT_DescripcionProducto.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Categoria", DDL_Categoria.SelectedValue, SqlDbType.Int);
            DT.DT1.Rows.Add("@Costo", TXT_Costo.Text, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@PrecioVenta", TXT_PrecioVenta.Text, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@CodigoBarra", TXT_CodigoBarra.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@MedidaVenta", TXT_MedidaVenta.Text, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@MedidaProduccion", TXT_MedidaProduccion.Text, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@UnidadMedida", TXT_UnidadMedida.Text, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@EsEmpaque", CHK_EsEmpaque.Checked, SqlDbType.Bit);
            DT.DT1.Rows.Add("@EsInsumo", CHK_EsInsumo.Checked, SqlDbType.Bit);

            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            if (HDF_IDProducto.Value == "0")
                DT.DT1.Rows.Add("@TipoSentencia", "CrearProducto", SqlDbType.VarChar);
            else
                DT.DT1.Rows.Add("@TipoSentencia", "EditarProducto", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP03_0001");
            string script = "";
            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    script += "alertifyerror('No se ha guardado el producto. Error: " + Result.Rows[0][1].ToString().Trim() + "');";
                    cargarProductos(script);
                }
                else
                {
                    script += "cerrarModalCrearProducto();alertifysuccess('Se ha guardado el producto con éxito.')";
                    cargarProductos(script);
                }
            }
            else
            {
                script += "alertifywarning('No se ha guardado el producto. Por favor, intente nuevamente');";
                cargarProductos(script);
            }
        }
        #endregion
    }
}