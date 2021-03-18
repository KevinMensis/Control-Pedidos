using CapaLogica.Entidades.EficienciaEnergetica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MCWebHogar.ControlPedidos
{
    public partial class Reportes : System.Web.UI.Page
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
                    cargarDatos();
                    cargarGraficos();
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

        #region  Información Encabezado
        private void cargarDatos()
        {
            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "DatosEncabezado", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP20_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    string detalle = dr["Detalle"].ToString().Trim();

                    switch (detalle)
                    {
                        case "Pedidos":
                            div_CantidadPedidos.InnerHtml = dr["Cantidad"].ToString().Trim();
                            break;
                        case "Despacho":
                            div_CantidadDespachos.InnerHtml = dr["Cantidad"].ToString().Trim();
                            break;
                        case "Devolucion":
                            div_CantidadDevoluciones.InnerHtml = dr["Cantidad"].ToString().Trim();
                            break;
                        case "PedidosPendientes":
                            div_CantidadPendientes.InnerHtml = dr["Cantidad"].ToString().Trim();
                            break;
                    }
                    UpdatePanel_divEncabezados.Update();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Graficos
        private void cargarGraficos()
        {
            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarGraficos(" + usuario + ");";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptCargarHistoricoInstitucion", cargar, true);
        }

        [WebMethod()]
        public static List<DatosHistoricos> cargarGraficoPedidos(string idUsuario)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoPedido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP04_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.dia = dr["FechaPedido"].ToString().Trim();
                    dh.cantidadPedidos = Convert.ToInt32(dr["CantidadPedidos"].ToString().Trim());

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
        public static List<DatosHistoricos> cargarGraficoDevoluciones(string idUsuario)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoDevoluciones", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP19_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.puntoVenta = dr["DescripcionPuntoVenta"].ToString().Trim();
                    dh.cantidadDevolucion = Convert.ToInt32(dr["CantidadDevolucion"].ToString().Trim());

                    lista.Add(dh);
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