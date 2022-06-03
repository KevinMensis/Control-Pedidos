using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MCWebHogar.GestionProveedores
{
    public class Email
    {
        public string MessageID { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public DateTime FechaMail { get; set; }

        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        public List<string> CargarEmails()
        {
            List<string> listaEmails = new List<string>();
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "CargarEmail", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP00_Mail_0001");

            if (Result != null && Result.Rows.Count > 0)
            {
                foreach (DataRow dr in Result.Rows)
                {
                    string messageID = dr["MessageID"].ToString().Trim();
                    listaEmails.Add(messageID);
                }
            }
            return listaEmails;
        }

        public int GuardarEmail()
        {
            DT.DT1.Clear();

            DT.DT1.Rows.Add("@MessageID", this.MessageID, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@EmailFrom", this.EmailFrom, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@Subject", this.Subject, SqlDbType.VarChar);
            DT.DT1.Rows.Add("@FechaMail", this.FechaMail, SqlDbType.DateTime);

            DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
            DT.DT1.Rows.Add("@TipoSentencia", "Insertar", SqlDbType.VarChar);

            Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "GP00_Mail_0001");

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