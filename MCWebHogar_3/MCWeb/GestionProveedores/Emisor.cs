using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.ControlPedidos.Proveedores
{
    public class Emisor
    {
        public string nombre { get; set; }
        public string nombreComercial { get; set; }
        public string tipoIdentificacion { get; set; }
        public string numeroIdentificacion { get; set; }
        public string provincia { get; set; }
        public string canton { get; set; }
        public string distrito { get; set; }
        public string barrio { get; set; }
        public string otrasSenas { get; set; }
        public string telefono { get; set; }
        public string correoEmisor { get; set; }
        public string correoFacturacion { get; set; }

        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        public void GuardarEmisor()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@NumeroIdentificacion", this.numeroIdentificacion, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoIdentificacion", this.tipoIdentificacion, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Nombre", this.nombre, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@NombreComercial", this.nombreComercial, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@CorreoEmisor", this.correoEmisor, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Telefono", this.telefono, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Provincia", this.provincia, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Canton", this.canton, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Distrito", this.distrito, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Barrio", this.barrio, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@OtrasSenas", this.otrasSenas, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "InsertarActualizar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP01_0001");

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