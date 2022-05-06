using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.ControlPedidos.Proveedores
{
    public class Factura
    {
        public string claveFactura { get; set; }
        public string numeroConsecuetivoFactura { get; set; }
        public string identificacionEmisor { get; set; }
        public DateTime fechaFactura { get; set; }
        public decimal totalGravado { get; set; }
        public decimal totalExento { get; set; }
        public decimal totalExonerado { get; set; }
        public decimal totalVenta { get; set; }
        public decimal totalDescuento { get; set; }
        public decimal totalVentaNeta { get; set; }
        public decimal totalImpuesto { get; set; }
        public decimal totalComprobante { get; set; }

        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        public void GuardarFactura()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@ClaveFactura", this.claveFactura, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NumeroConsecutivoFactura", this.numeroConsecuetivoFactura, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CodigoEmisor", this.identificacionEmisor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@FechaFactura", this.fechaFactura, SqlDbType.DateTime);
            DT.DT1.Rows.Add("@TotalGravado", this.totalGravado, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalExento", this.totalExento, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalExonerado", this.totalExonerado, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalVenta", this.totalVenta, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalDescuento", this.totalDescuento, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalVentaNeta", this.totalVentaNeta, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalImpuesto", this.totalImpuesto, SqlDbType.Decimal);
            DT.DT1.Rows.Add("@TotalComprobante", this.totalComprobante, SqlDbType.Decimal);

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "InsertarActualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP02_0001");

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