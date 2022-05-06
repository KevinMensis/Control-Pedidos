using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.GestionProveedores
{
    public class Attachment
    {
        public int MailID { get; set; }
        public string FileName { get; set; }

        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        public int GuardarAttachment()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@MailID", this.MailID, SqlDbType.Int);
            DT.DT1.Rows.Add("@FileName", this.FileName, SqlDbType.VarChar);

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Insertar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP00_Attachment_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(Result.Rows[0][1].ToString().Trim());
                }
            }
            else
            {
                return 0;
            }
        }
    }
}