using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.ControlPedidos
{
    public class ClasePermiso
    {
        public static int Permiso(string tipo, string modulo, string formulario, int idUsuario)
        {
            CapaLogica.GestorDataDT DT;
            DataTable dt = new DataTable();

            dt = new DataTable();
            DT = new CapaLogica.GestorDataDT();
            DT.DT1.Rows.Add("@IDUsuario", idUsuario, SqlDbType.Int);
            DT.DT1.Rows.Add("@Modulo", modulo, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@formulario", formulario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@tipo", tipo, SqlDbType.VarChar);

            dt = CapaLogica.GestorDatos.Consultar(DT.DT1, "PER00_Permisos");

            if (dt != null)
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return 0;
        }
    }
}