using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.ControlPedidos.Proveedores
{
    public class LineaDetalle
    {
        public int numeroLinea { get; set; }
        public string codigoProducto { get; set; }
        public decimal cantidad { get; set; }
        public string unidadMedida { get; set; }
        public string detalleProducto { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoTotal { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal subTotal { get; set; }
        public decimal porcentajeImpuesto { get; set; }
        public decimal montoImpuesto { get; set; }
        public decimal montoTotalIVA { get; set; }
        public DateTime fechaFactura { get; set; }
        public string claveFactura { get; set; }
        public string identificacionEmisor { get; set; }
        public string identificacionReceptor { get; set; }

        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        public void GuardarLineaDetalle()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@NumeroLinea", this.numeroLinea, SqlDbType.Int);
            DT.DT1.Rows.Add("@CodigoProducto", this.codigoProducto, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Cantidad", this.cantidad, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@UnidadMedida", this.unidadMedida, SqlDbType.VarChar);           
            DT.DT1.Rows.Add("@DetalleProducto", this.detalleProducto, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@PrecioUnitario", this.precioUnitario, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@PrecioUnitarioFinal", this.precioUnitario, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@MontoTotal", this.montoTotal, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@MontoDescuento", this.montoDescuento, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@SubTotal", this.subTotal, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@PorcentajeImpuesto", this.porcentajeImpuesto, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@MontoImpuesto", this.montoImpuesto, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@MontoTotalIVA", this.montoTotalIVA, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Fecha", this.fechaFactura, SqlDbType.DateTime);
            DT.DT1.Rows.Add("@ClaveFactura", this.claveFactura, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CodigoEmisor", this.identificacionEmisor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroIdentificacionReceptor", this.identificacionReceptor, SqlDbType.VarChar); 

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Insertar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP04_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }
    }
}