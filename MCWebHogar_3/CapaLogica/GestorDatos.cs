using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaDatos;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Text;
using System.Configuration;
using System.Collections;
using LogicLyers.Servicios;
using System.Web.Configuration;

namespace CapaLogica
{
    public class GestorDatos
    {
        #region VariablesGlobales
        public static string siawindb = string.Empty;
        public static string cemdb = string.Empty;
        public static SqlConnection conexion = new SqlConnection();
        public static SqlParameter Parametro;
        public static ArrayList Parametros;
        public static DataAccess access = new DataAccess();
        public static void Connection(string siawin, string cem)
        {
            siawindb = siawin;
            cemdb = cem;
        }
        #endregion

        public static DataTable Consultar(DataTable DT, string Procedure)
        {
            return DataAccess.GESTOR_CONSULTA_CEM(DT, Procedure);
        }

        public static bool ConsultarPermisos(string Usuario, string Formulario, string TipoP, string Tipo)
        {
            bool Resultado = false;
            CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
            DT.DT1.Rows.Add("Usuario", Usuario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("Tipo", Tipo, SqlDbType.VarChar);
            DT.DT1.Rows.Add("Formulario", Formulario, SqlDbType.VarChar);
            DT.DT1.Rows.Add("TipoP", TipoP, SqlDbType.VarChar);
            DataTable DTResultado = Consultar(DT.DT1, "CO_PER02_0002");
            if (DTResultado != null && DTResultado.Rows.Count > 0)
            {
                if (DTResultado.Rows[0][0].ToString().Trim().ToUpper() == "ERROR")
                {
                    Resultado = false;
                }
                else
                {
                    Resultado = true;
                }
            }
            else
            {
                Resultado = false;
            }

            return Resultado;
        }

    }
}