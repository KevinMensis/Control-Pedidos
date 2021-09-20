﻿using CapaLogica.Entidades.ControlPedidos;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                else if (ClasePermiso.Permiso("Ingreso", "Módulo", "Reportes", Convert.ToInt32(Session["UserId"].ToString().Trim())) <= 0)
                {
                    Session.Add("Message", "No tiene permisos para acceder al módulo de Reportes.");
                    Response.Redirect("Pedido.aspx");
                }
                else
                {
                    ViewState["Ordenamiento"] = "ASC";
                    cargarDatos();
                    cargarDDLs();
                    cargarGraficos();
                }
            }
            else
            {
                string opcion = Page.Request.Params["__EVENTTARGET"];
                string argument = Page.Request.Params["__EVENTARGUMENT"];
                if (opcion.Contains("detalleCantidad"))
                {
                    cargarDetalleCantidad();
                }
                if (opcion.Contains("detalleSemanal"))
                {
                    cargarDetalleSemana();
                }
            }
        }

        #region General
        protected void LNK_CerrarSesion_Cick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        protected void cargarDDLs()
        {
            DT.DT1.Clear();
            DT.DT1.Rows.Add("@Usuario", "kpicado", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPlantasProduccion", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP01_0001");

            if (Result != null && Result.Rows.Count > 0)
            {                                 
                DataView dv = new DataView(Result);
                dv.RowFilter = "IDPlantaProduccion <> 0";
                LB_PlantaProduccion.DataSource = dv;
                LB_PlantaProduccion.DataTextField = "DescripcionPlantaProduccion";
                LB_PlantaProduccion.DataValueField = "IDPlantaProduccion";
                LB_PlantaProduccion.DataBind();
            }

            DT.DT1.Clear();
            DT.DT1.Rows.Add("@IDUsuario", Session["UserId"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarPuntosVenta", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP02_0001");

            if (Result != null && Result.Rows.Count > 0)
            {                             
                DataView dv = new DataView(Result);
                dv.RowFilter = "IDPuntoVenta <> 0";
                LB_Sucursal.DataSource = dv;
                LB_Sucursal.DataTextField = "DescripcionPuntoVenta";
                LB_Sucursal.DataValueField = "IDPuntoVenta";
                LB_Sucursal.DataBind();
            }

            DateTime hoy = DateTime.Now;
            string mes = hoy.Month < 10 ? "0" + Convert.ToString(hoy.Month) : Convert.ToString(hoy.Month);
            string dia = hoy.Day < 10 ? "0" + Convert.ToString(hoy.Day) : Convert.ToString(hoy.Day);
            TXT_FechaDesde.Text = Convert.ToString(hoy.Year) + "-" + mes + "-01";
            TXT_FechaHasta.Text = Convert.ToString(hoy.Year) + "-" + mes + "-" + dia;
        }
        
        #region  Información Encabezado
        private void cargarDatos()
        {
            DT.DT1.Clear();
            string puntosVenta = "";
            string plantaProduccion = "";
            try
            {
                #region Puntos Venta
                foreach (ListItem l in LB_Sucursal.Items)
                {
                    if (l.Selected)
                    {
                        puntosVenta += "'" + l.Value + "',";
                    }
                }
                puntosVenta = puntosVenta.TrimEnd(',');
                if (puntosVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", puntosVenta, SqlDbType.VarChar);
                }
                #endregion

                #region Plantas Produccion
                foreach (ListItem l in LB_PlantaProduccion.Items)
                {
                    if (l.Selected)
                    {
                        plantaProduccion += "'" + l.Value + "',";
                    }
                }
                plantaProduccion = plantaProduccion.TrimEnd(',');
                if (plantaProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", plantaProduccion, SqlDbType.VarChar);
                }
                #endregion

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
                    fechaDesde = Convert.ToString(hoy.Year) + "-" + mes + "-01";
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

                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "DatosEncabezado", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    string detalle = dr["Detalle"].ToString().Trim();

                    switch (detalle)
                    {
                        case "Pedidos":
                            div_CantidadPedidos.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
                            break;
                        case "OrdenProduccion":
                            div_CantidadOrdenProduccion.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
                            break;
                        case "Despacho":
                            div_CantidadDespachos.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
                            break;
                        case "PedidoRecibido":
                            div_CantidadPedidosRecibidos.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
                            break;
                        case "Devolucion":
                            div_CantidadDevoluciones.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
                            break;
                        case "Desecho":
                            div_CantidadDesecho.InnerHtml = string.Format("{0:n0}", dr["Cantidad"]);
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
        #endregion

        #region Reportes Excel
        protected void BTN_ReporteExcel_Click(object sender, EventArgs e)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();
            DataTable ResultODP = new DataTable();
            DataTable ResultEmpaque = new DataTable();
            DataTable ResultDesechos = new DataTable();
            DataTable ResultDevoluciones = new DataTable();
            string puntosVenta = "";
            string plantaProduccion = "";

            DT.DT1.Clear();
            #region Puntos Venta
            foreach (ListItem l in LB_Sucursal.Items)
            {
                if (l.Selected)
                {
                    puntosVenta += "'" + l.Value + "',";
                }
            }
            puntosVenta = puntosVenta.TrimEnd(',');            
            #endregion

            #region Plantas Produccion
            foreach (ListItem l in LB_PlantaProduccion.Items)
            {
                if (l.Selected)
                {
                    plantaProduccion += "'" + l.Value + "',";
                }
            }
            plantaProduccion = plantaProduccion.TrimEnd(',');            
            #endregion
            try
            {
                if (puntosVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", puntosVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (plantaProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", plantaProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", TXT_FechaDesde.Text, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", TXT_FechaHasta.Text + " 23:59:59", SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "ReporteExcel", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");
                if (Result.Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_ReporteExcel_Click", "desactivarloading();cargarFiltros();alertifyerror('No hay registros para descargar.');", true);
                    return;
                }
                
                DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelODP";
                ResultODP = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelEmpaque";
                ResultEmpaque = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelDesechos";
                ResultDesechos = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelDevoluciones";
                ResultDevoluciones = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(Result, "Reporte");
                    wb.Worksheets.Add(ResultODP, "ReporteODP");
                    wb.Worksheets.Add(ResultEmpaque, "ReporteEmpaque");
                    wb.Worksheets.Add(ResultDesechos, "ReporteDesechos");
                    wb.Worksheets.Add(ResultDevoluciones, "ReporteDevoluciones");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Reporte.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_ReporteExcel_Click", "desactivarloading();cargarFiltros();", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BTN_ReporteExcelSemanal_Click(object sender, EventArgs e)
        {
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();
            DataTable ResultODP = new DataTable();
            DataTable ResultEmpaque = new DataTable();
            DataTable ResultDesechos = new DataTable();
            DataTable ResultDevoluciones = new DataTable();
            string puntosVenta = "";
            string plantaProduccion = "";

            DT.DT1.Clear();
            #region Puntos Venta
            foreach (ListItem l in LB_Sucursal.Items)
            {
                if (l.Selected)
                {
                    puntosVenta += "'" + l.Value + "',";
                }
            }
            puntosVenta = puntosVenta.TrimEnd(',');
            #endregion

            #region Plantas Produccion
            foreach (ListItem l in LB_PlantaProduccion.Items)
            {
                if (l.Selected)
                {
                    plantaProduccion += "'" + l.Value + "',";
                }
            }
            plantaProduccion = plantaProduccion.TrimEnd(',');
            #endregion
            try
            {
                if (puntosVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", puntosVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (plantaProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", plantaProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", TXT_FechaDesde.Text, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", TXT_FechaHasta.Text + " 23:59:59", SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "ReporteExcelSemanal", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");
                if (Result.Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_ReporteExcel_Click", "desactivarloading();cargarFiltros();alertifyerror('No hay registros para descargar.');", true);
                    return;
                }

                //DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelODP";
                //ResultODP = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                //DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelEmpaque";
                //ResultEmpaque = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                //DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelDesechos";
                //ResultDesechos = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                //DT.DT1.Rows[DT.DT1.Rows.Count - 1][1] = "ReporteExcelDevoluciones";
                //ResultDevoluciones = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(Result, "ReporteSemanal");
                    //wb.Worksheets.Add(ResultODP, "ReporteODP");
                    //wb.Worksheets.Add(ResultEmpaque, "ReporteEmpaque");
                    //wb.Worksheets.Add(ResultDesechos, "ReporteDesechos");
                    //wb.Worksheets.Add(ResultDevoluciones, "ReporteDevoluciones");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ReporteSemanal.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScriptBTN_ReporteExcel_Click", "desactivarloading();cargarFiltros();", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Reportes Tablas
        private DataTable consultaDetalleCantidad()
        {
            string modulo = HDF_Detalle.Value;
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();

            string idsPuntosVenta = "";
            string puntosVenta = "";
            string idsPlantasProduccion = "";
            string plantasProduccion = "";

            DT.DT1.Clear();
            #region Puntos Venta
            foreach (ListItem l in LB_Sucursal.Items)
            {
                if (l.Selected)
                {
                    idsPuntosVenta += "'" + l.Value + "',";
                    puntosVenta += "'" + l.Text + "',";
                }
            }
            idsPuntosVenta = idsPuntosVenta.TrimEnd(',');
            puntosVenta = puntosVenta.TrimEnd(',');
            #endregion

            #region Plantas Produccion
            foreach (ListItem l in LB_PlantaProduccion.Items)
            {
                if (l.Selected)
                {
                    idsPlantasProduccion += "'" + l.Value + "',";
                    plantasProduccion += "'" + l.Text + "',";
                }
            }
            idsPlantasProduccion = idsPlantasProduccion.TrimEnd(',');
            plantasProduccion = plantasProduccion.TrimEnd(',');
            #endregion
            if (idsPuntosVenta != "")
            {
                DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroSucursales", idsPuntosVenta.TrimEnd(','), SqlDbType.VarChar);
                puntoVentaCantidad.InnerText = "Puntos venta: " + puntosVenta.TrimEnd(',');
                puntoVentaCantidadPedido.InnerText = "Puntos venta: " + puntosVenta.TrimEnd(',');
            }
            else
            {
                puntoVentaCantidad.InnerText = "Puntos venta: Todos";
                puntoVentaCantidadPedido.InnerText = "Puntos venta: Todos";
            }
            if (idsPlantasProduccion != "")
            {
                DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                plantaProduccionCantidad.InnerText = "Plantas producción: " + plantasProduccion.TrimEnd(',');
                plantaProduccionCantidadPedido.InnerText = "Plantas producción: " + plantasProduccion.TrimEnd(',');
            }
            else
            {
                plantaProduccionCantidad.InnerText = "Plantas producción: Todos";
                plantaProduccionCantidadPedido.InnerText = "Plantas producción: Todos";
            }
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@fechaDesde", TXT_FechaDesde.Text, SqlDbType.Date);
            DT.DT1.Rows.Add("@fechaHasta", TXT_FechaHasta.Text + " 23:59:59", SqlDbType.DateTime);

            fechaDesdeCantidad.InnerText = "Fecha desde: " + TXT_FechaDesde.Text.Split('-')[2] + "-" + TXT_FechaDesde.Text.Split('-')[1] + "-" + TXT_FechaDesde.Text.Split('-')[0];
            fechaHastaCantidad.InnerText = "Fecha hasta: " + TXT_FechaHasta.Text.Split('-')[2] + "-" + TXT_FechaHasta.Text.Split('-')[1] + "-" + TXT_FechaHasta.Text.Split('-')[0];
            fechaDesdeCantidadPedido.InnerText = "Fecha desde: " + TXT_FechaDesde.Text.Split('-')[2] + "-" + TXT_FechaDesde.Text.Split('-')[1] + "-" + TXT_FechaDesde.Text.Split('-')[0];
            fechaHastaCantidadPedido.InnerText = "Fecha hasta: " + TXT_FechaHasta.Text.Split('-')[2] + "-" + TXT_FechaHasta.Text.Split('-')[1] + "-" + TXT_FechaHasta.Text.Split('-')[0];

            if (HDF_PuntoVenta.Value != "")
            {
                DT.DT1.Rows.Add("@Sucursal", HDF_PuntoVenta.Value, SqlDbType.VarChar);
                puntoVentaCantidad.InnerText = "Punto venta: " + HDF_PuntoVenta.Value;
                puntoVentaCantidadPedido.InnerText = "Punto venta: " + HDF_PuntoVenta.Value;
            }
            if (HDF_PlantaProduccion.Value != "")
            {
                DT.DT1.Rows.Add("@PlantaProduccion", HDF_PlantaProduccion.Value, SqlDbType.VarChar);
                plantaProduccionCantidad.InnerText = "Planta producción: " + HDF_PlantaProduccion.Value;
                plantaProduccionCantidadPedido.InnerText = "Planta producción: " + HDF_PlantaProduccion.Value;
            }

            switch (modulo)
            {
                case "Pedidos":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
                    break;
                case "Orden produccion":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetalleOrdenProduccion", SqlDbType.VarChar);
                    break;
                case "Despachos":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
                    break;
                case "Pedidos recibidos":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
                    break;
                case "Devoluciones":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetalleDevoluciones", SqlDbType.VarChar);
                    break;
                case "Desechos":
                    DT.DT1.Rows.Add("@TipoSentencia", "DetalleDesechos", SqlDbType.VarChar);
                    break;
            }

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");
        }

        private void cargarDetalleCantidad()
        {
            DataTable Result = consultaDetalleCantidad();

            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarFiltros();cargarGraficos(" + usuario + ");";

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    if (HDF_Detalle.Value == "Pedidos" || HDF_Detalle.Value == "Despachos" || HDF_Detalle.Value == "Pedidos recibidos")
                    {
                        modalCantidadPedidoTitle.InnerText = "Detalle " + HDF_Detalle.Value;
                        DGV_DetalleCantidadPedido.DataSource = Result;
                        DGV_DetalleCantidadPedido.DataBind();
                        UpdatePanel_ModalDetalleCantidadPedido.Update();
                        UpdatePanel_DetalleCantidadPedido.Update();
                        string script = "cargarFiltros();abrirModalDetalleCantidadPedido();" + cargar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleCantidad", script, true);
                    }
                    else
                    {
                        modalCantidadTitle.InnerText = "Detalle " + HDF_Detalle.Value;
                        DGV_DetalleCantidad.DataSource = Result;
                        DGV_DetalleCantidad.DataBind();
                        UpdatePanel_ModalDetalleCantidad.Update();
                        UpdatePanel_DetalleCantidad.Update();
                        string script = "cargarFiltros();abrirModalDetalleCantidad();" + cargar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleCantidad", script, true);
                    }                    
                }
            }
            else
            {
                if (HDF_Detalle.Value == "Pedidos" || HDF_Detalle.Value == "Despachos" || HDF_Detalle.Value == "Pedidos recibidos")
                {
                    modalCantidadPedidoTitle.InnerText = "Detalle " + HDF_Detalle.Value;
                    DGV_DetalleCantidadPedido.DataSource = Result;
                    DGV_DetalleCantidadPedido.DataBind();
                    UpdatePanel_ModalDetalleCantidadPedido.Update();
                    UpdatePanel_DetalleCantidadPedido.Update();
                    string script = "cargarFiltros();abrirModalDetalleCantidadPedido();" + cargar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleCantidad", script, true);
                }
                else
                {
                    modalCantidadTitle.InnerText = "Detalle " + HDF_Detalle.Value;
                    DGV_DetalleCantidad.DataSource = Result;
                    DGV_DetalleCantidad.DataBind();
                    UpdatePanel_ModalDetalleCantidad.Update();
                    UpdatePanel_DetalleCantidad.Update();
                    string script = "cargarFiltros();abrirModalDetalleCantidad();" + cargar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleCantidad", script, true);
                }
            }
        }

        private DataTable consultaDetalleSemanal()
        {
            string modulo = HDF_Detalle.Value;
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();

            string idsPuntosVenta = "";
            string puntosVenta = "";
            string idsPlantasProduccion = "";
            string plantasProduccion = "";

            DT.DT1.Clear();
            #region Puntos Venta
            foreach (ListItem l in LB_Sucursal.Items)
            {
                if (l.Selected)
                {
                    idsPuntosVenta += "'" + l.Value + "',";
                    puntosVenta += "'" + l.Text + "',";
                }
            }
            idsPuntosVenta = idsPuntosVenta.TrimEnd(',');
            puntosVenta = puntosVenta.TrimEnd(',');
            #endregion

            #region Plantas Produccion
            foreach (ListItem l in LB_PlantaProduccion.Items)
            {
                if (l.Selected)
                {
                    idsPlantasProduccion += "'" + l.Value + "',";
                    plantasProduccion += "'" + l.Text + "',";
                }
            }
            idsPlantasProduccion = idsPlantasProduccion.TrimEnd(',');
            plantasProduccion = plantasProduccion.TrimEnd(',');
            #endregion
            if (idsPuntosVenta != "")
            {
                DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroSucursales", idsPuntosVenta.TrimEnd(','), SqlDbType.VarChar);
                puntoVentaSemana.InnerText = "Puntos venta: " + puntosVenta.TrimEnd(',');
            }
            else
            {
                puntoVentaSemana.InnerText = "Puntos venta: Todos";
            }
            if (idsPlantasProduccion != "")
            {
                DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                plantaProduccionSemana.InnerText = "Plantas producción: " + plantasProduccion.TrimEnd(',');
            }
            else
            {
                plantaProduccionSemana.InnerText = "Plantas producción: Todos";
            }
            DT.DT1.Rows.Add("@Usuario", Session["Usuario"].ToString().Trim(), SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Dia", HDF_Dia.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Semana", HDF_Semana.Value, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@fechaDesde", TXT_FechaDesde.Text, SqlDbType.Date);
            DT.DT1.Rows.Add("@fechaHasta", TXT_FechaHasta.Text + " 23:59:59", SqlDbType.DateTime);

            fechaDesdeSemana.InnerText = "Fecha desde: " + TXT_FechaDesde.Text.Split('-')[2] + "-" + TXT_FechaDesde.Text.Split('-')[1] + "-" + TXT_FechaDesde.Text.Split('-')[0];
            fechaHastaSemana.InnerText = "Fecha hasta: " + TXT_FechaHasta.Text.Split('-')[2] + "-" + TXT_FechaHasta.Text.Split('-')[1] + "-" + TXT_FechaHasta.Text.Split('-')[0];
            
            if (HDF_PuntoVenta.Value != "")
            {
                DT.DT1.Rows.Add("@Sucursal", HDF_PuntoVenta.Value, SqlDbType.VarChar);
            }
            if (HDF_PlantaProduccion.Value != "")
            {
                DT.DT1.Rows.Add("@PlantaProduccion", HDF_PlantaProduccion.Value, SqlDbType.VarChar);
            }

            DT.DT1.Rows.Add("@TipoSentencia", "ReporteExcelSemanal", SqlDbType.VarChar);
            //switch (modulo)
            //{
            //    case "Pedidos":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
            //        break;
            //    case "Orden produccion":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetalleOrdenProduccion", SqlDbType.VarChar);
            //        break;
            //    case "Despachos":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
            //        break;
            //    case "Pedidos recibidos":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetallePedido", SqlDbType.VarChar);
            //        break;
            //    case "Devoluciones":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetalleDevoluciones", SqlDbType.VarChar);
            //        break;
            //    case "Desechos":
            //        DT.DT1.Rows.Add("@TipoSentencia", "DetalleDesechos", SqlDbType.VarChar);
            //        break;
            //}

            return CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");
        }

        private void cargarDetalleSemana()
        {
            DataTable Result = consultaDetalleSemanal();

            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarFiltros();cargarGraficos(" + usuario + ");";

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return;
                }
                else
                {
                    modalDetalleSemana.InnerText = "Detalle " + HDF_Detalle.Value;
                    DGV_DetalleSemanal.DataSource = Result;
                    DGV_DetalleSemanal.DataBind();
                    UpdatePanel_ModalDetalleSemana.Update();
                    UpdatePanel_DetalleSemanal.Update();
                    string script = "cargarFiltros();abrirModalDetalleSemanal();" + cargar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleSemana", script, true);                    
                }
            }
            else
            {
                modalCantidadPedidoTitle.InnerText = "Detalle " + HDF_Detalle.Value;
                DGV_DetalleCantidadPedido.DataSource = Result;
                DGV_DetalleCantidadPedido.DataBind();
                UpdatePanel_ModalDetalleCantidadPedido.Update();
                UpdatePanel_DetalleCantidadPedido.Update();
                string script = "cargarFiltros();abrirModalDetalleSemanal();" + cargar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarDetalleSemana", script, true); 
            }
        }

        protected void DGV_DetalleCantidad_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = consultaDetalleCantidad();
                                                                                 
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
                    if (HDF_Detalle.Value == "Pedidos" || HDF_Detalle.Value == "Despachos" || HDF_Detalle.Value == "Pedidos recibidos")
                    {
                        DGV_DetalleCantidadPedido.DataSource = Result;
                        DGV_DetalleCantidadPedido.DataBind();
                        UpdatePanel_DetalleCantidadPedido.Update();
                    }
                    else
                    {
                        DGV_DetalleCantidad.DataSource = Result;
                        DGV_DetalleCantidad.DataBind();
                        UpdatePanel_DetalleCantidad.Update();
                    }                    
                }
            }
            else
            {
                if (HDF_Detalle.Value == "Pedidos" || HDF_Detalle.Value == "Despachos" || HDF_Detalle.Value == "Pedidos recibidos")
                {
                    DGV_DetalleCantidadPedido.DataSource = Result;
                    DGV_DetalleCantidadPedido.DataBind();
                    UpdatePanel_DetalleCantidadPedido.Update();
                }
                else
                {
                    DGV_DetalleCantidad.DataSource = Result;
                    DGV_DetalleCantidad.DataBind();
                    UpdatePanel_DetalleCantidad.Update();
                } 
            }
        }

        protected void DGV_DetalleSemanal_Sorting(object sender, GridViewSortEventArgs e)
        {
            Result = consultaDetalleSemanal();

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
                    DGV_DetalleSemanal.DataSource = Result;
                    DGV_DetalleSemanal.DataBind();
                    UpdatePanel_DetalleSemanal.Update();
                }
            }
            else
            {
                DGV_DetalleSemanal.DataSource = Result;
                DGV_DetalleSemanal.DataBind();
                UpdatePanel_DetalleSemanal.Update();
            }
        }
        #endregion

        #region Graficos
        protected void Recargar_Click(object sender, EventArgs e)
        {
            cargarDatos();
            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarFiltros();cargarGraficos(" + usuario + ");";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptRecargar_Click", cargar, true);
        } 
        private void cargarGraficos()
        {
            string usuario = Session["UserID"].ToString().Trim();
            string cargar = "cargarGraficos(" + usuario + ");";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptcargarGraficos", cargar, true);
        }

        [WebMethod()]
        public static List<DatosHistoricos> cargarGraficoPedidos(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta, string idsPlantasProduccion)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (idsPlantasProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoPedido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

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
        public static List<DatosHistoricos> cargarGraficoDevoluciones(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoDevoluciones", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

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

        [WebMethod()]
        public static List<DatosHistoricos> cargarGraficoDesechos(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoDesechos", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.puntoVenta = dr["DescripcionPuntoVenta"].ToString().Trim();
                    dh.cantidadDesecho = Convert.ToInt32(dr["CantidadDesecho"].ToString().Trim());

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
        public static List<DatosHistoricos> cargarGraficoPedidosPuntoVenta(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta, string idsPlantasProduccion)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (idsPlantasProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoMontoPedido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.puntoVenta = dr["DescripcionPuntoVenta"].ToString().Trim();
                    dh.plantaProduccion = dr["DescripcionPlantaProduccion"].ToString().Trim();
                    dh.montoPedidos = Convert.ToInt32(dr["MontoPedido"].ToString().Trim());

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
        public static List<DatosHistoricos> cargarGraficoDespachosPuntoVenta(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta, string idsPlantasProduccion)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (idsPlantasProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoMontoDespacho", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.puntoVenta = dr["DescripcionPuntoVenta"].ToString().Trim();
                    dh.plantaProduccion = dr["DescripcionPlantaProduccion"].ToString().Trim();
                    dh.montoDespacho = Convert.ToInt32(dr["MontoDespacho"].ToString().Trim());

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
        public static List<DatosHistoricos> cargarGraficoRecibidoPuntoVenta(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta, string idsPlantasProduccion)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (idsPlantasProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficoMontoRecibido", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.puntoVenta = dr["DescripcionPuntoVenta"].ToString().Trim();
                    dh.plantaProduccion = dr["DescripcionPlantaProduccion"].ToString().Trim();
                    dh.montoDespacho = Convert.ToInt32(dr["MontoDespacho"].ToString().Trim());

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
        public static List<DatosHistoricos> cargarGraficoPedidosSemana(string idUsuario, string fechaDesde, string fechaHasta, string idsPuntoVenta, string idsPlantasProduccion)
        {
            List<DatosHistoricos> lista = new List<DatosHistoricos>();
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DataTable Result = new DataTable();

            DT.DT1.Clear();
            try
            {
                if (idsPuntoVenta != "")
                {
                    DT.DT1.Rows.Add("@FiltrarSucursales", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroSucursales", idsPuntoVenta.TrimEnd(','), SqlDbType.VarChar);
                }
                if (idsPlantasProduccion != "")
                {
                    DT.DT1.Rows.Add("@FiltrarPlantasProduccion", 1, SqlDbType.Int);
                    DT.DT1.Rows.Add("@FiltroPlantasProduccion", idsPlantasProduccion.TrimEnd(','), SqlDbType.VarChar);
                }
                DT.DT1.Rows.Add("@Usuario", idUsuario, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@fechaDesde", fechaDesde, SqlDbType.Date);
                DT.DT1.Rows.Add("@fechaHasta", fechaHasta, SqlDbType.DateTime);
                DT.DT1.Rows.Add("@TipoSentencia", "CargarGraficosSemanal", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP_Reportes_0001");

                foreach (DataRow dr in Result.Rows)
                {
                    DatosHistoricos dh = new DatosHistoricos();

                    dh.dia = dr["Dia"].ToString().Trim();
                    dh.semana = dr["Semana"].ToString().Trim();
                    dh.cantidadPedidos = Convert.ToInt32(dr["CantidadSolicitada"].ToString().Trim()); 
                    dh.montoPedidos = Convert.ToInt32(dr["MontoPedido"].ToString().Trim());
                    dh.cantidadDespacho = Convert.ToInt32(dr["CantidadDespachada"].ToString().Trim());
                    dh.montoDespacho = Convert.ToInt32(dr["MontoDespacho"].ToString().Trim());
                    dh.cantidadRecibido = Convert.ToInt32(dr["CantidadRecibida"].ToString().Trim());
                    dh.montoRecibido = Convert.ToInt32(dr["MontoPedidoRecibido"].ToString().Trim());
                    
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