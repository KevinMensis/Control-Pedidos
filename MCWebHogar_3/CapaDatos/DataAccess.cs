using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
namespace CapaDatos
{
    public class DataAccess
    {
        public static string SIAWINDB = string.Empty;
        public static string CEMDB = string.Empty;
        public static SqlTransaction Transaccion;
        public static ArrayList Parametros;
        public static void Conexion(string siawin, string cem)
        {
            SIAWINDB = siawin;
            CEMDB = cem;
        }

        public static DataTable SIA_DT_Ejecutar(string sql, SqlCommand command)
        {
            DataTable dt = new DataTable();
            SqlConnection conexion = new SqlConnection(SIAWINDB);
            command.Connection = conexion;

            command.CommandText = sql;
            //command = new SqlCommand(sql,conexion);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public static DataTable CEM_DT_Ejecutar(string sql, SqlCommand command)
        {

            DataTable dt = new DataTable();
            SqlConnection conexion = new SqlConnection(CEMDB);
            command.Connection = conexion;
            command.CommandText = sql;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public static DataTable GESTOR_CONSULTA_CEM(DataTable Parametros, string Procedure)
        {
            DataTable ds = new DataTable();
            SqlConnection conexion = new SqlConnection(CEMDB);
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {

                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(new System.Data.SqlDbType());

                SqlCommand command = new SqlCommand();
                command.Connection = conexion;

                command.CommandText = Procedure;
                command.CommandType = CommandType.StoredProcedure;
             
                da.SelectCommand = command;
                da.SelectCommand.CommandTimeout = 480;
                
                if (Procedure == "CED" || Procedure.Trim() == "HO11_BuscarFile" || Procedure.Trim() == "HO05_Insert" || Procedure.Trim() == "HX00_Commandos" ||
                  Procedure.Trim() == "HO05_Buscar" || Procedure.Trim() == "IM00_ImagenInsert" || Procedure.Trim() == "CED03_ReimprimirTransaccion" || Procedure.Trim() == "IM00Imagen_Update")
                {
                    da.SelectCommand.CommandTimeout = 480;
                }
                foreach (DataRow DR in Parametros.Rows)
                {
                    System.Data.SqlDbType sqldbtype = (System.Data.SqlDbType)tc.ConvertFromString(DR["TipoValor"].ToString());
                    /*Se asigna el parametro como una nueva instancia para poder setear correctamente el valor del sqldbtype*/
                    if (DR["TipoValor"].ToString().Trim() == "Image")
                    {
                        command.Parameters.Add(new SqlParameter(DR["Variable"].ToString(), sqldbtype));
                        command.Parameters[DR["Variable"].ToString()].Value = DR["IMAGEN"];
                    }
                    else
                    {

                        command.Parameters.Add(new SqlParameter(DR["Variable"].ToString(), sqldbtype));
                        command.Parameters[DR["Variable"].ToString()].Value = DR["Valor"];
                    }

                }
                //if (conexion.State != ConnectionState.Closed)
                //{
                    //se indica el nombre de la tabla
                    da.Fill(ds); //se rellena el dataset
                    return ds;
                //}
                //else
                //{
                //    //return ds = null;
                //    return ds;
                //}
            }
            catch (Exception ex)
            {
                EscribirLog(ex.ToString());
            }

            finally
            {
                if (conexion != null) conexion.Close();

            }

            return ds;

        }//FIN METODO



        public static bool EjecutarProcedimientoAlmacenado2(string pNombre, ArrayList pParametros, ref DataTable pDt, SqlConnection conexion2,ref string excep)
        {
            bool returnValue;
            SqlDataAdapter Da;
            SqlParameter Param;
            SqlConnection conexion = new SqlConnection(CEMDB);
            returnValue = false;
            try
            {
                Da = new SqlDataAdapter();

                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.CommandText = pNombre;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                Da.SelectCommand.Connection = conexion;

                if (pParametros != null)
                {
                    foreach (SqlParameter tempLoopVar_Param in pParametros)
                    {
                        Param = tempLoopVar_Param;
                        Da.SelectCommand.Parameters.Add(Param);
                    }
                }

                if (Transaccion != null)
                {
                    Da.SelectCommand.Transaction = Transaccion;
                }

                pDt = new DataTable();
                Da.Fill(pDt);
                conexion2.Close();
                //db.Desconectar(Conexion);
                returnValue = true;

            }
            catch (Exception ex)
            {
                excep = ex.ToString();
                string Parametros;
                conexion2.Close();
                Parametros = "";
                if (pParametros != null)
                {
                    foreach (SqlParameter tempLoopVar_Param in pParametros)
                    {
                        Param = tempLoopVar_Param;
                        Parametros = Parametros + Param.ParameterName + "=" + Param.Value + " *** ";
                    }
                }

                

            }
            return returnValue;
        }

        public static void EscribirLog(string Pexepcion)
        {
            string Pruta;
            System.IO.TextWriter Fw;

            Pruta = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (Pruta == "")
            {
                Pruta = "c:\\temp";
            }
            Pruta = Pruta + "\\error.log";

            try
            {
                if (System.IO.File.Exists(Pruta))
                {
                    System.IO.FileInfo val = new System.IO.FileInfo(Pruta);
                    if (val.Length > 2048000)
                    {
                        System.IO.File.Delete(Pruta);
                    }
                }
                else
                {
                    System.IO.File.Delete(Pruta);
                    Fw = System.IO.File.CreateText(Pruta);
                    Fw.Close();
                }

                Fw = System.IO.File.AppendText(Pruta);
                Fw.WriteLine(Pexepcion);
                Fw.WriteLine("SISTEMA= MC Web"); //+ (new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.AssemblyName);
                Fw.WriteLine("USUARIO=" + System.Environment.UserName);
                Fw.WriteLine("FECHA= " + DateTime.Now);
                Fw.WriteLine("ERROR= " + Pexepcion);
                Fw.WriteLine("*********************************");
                Fw.WriteLine("");
                Fw.Close();

            }
            catch (Exception)
            {

            }

            RegistrarErrorEventViewer(Pexepcion, EventLogEntryType.Error);

        }
        public static void RegistrarErrorEventViewer(string PMensaje, EventLogEntryType pTipo)
        {
            string sSource;
            string sLog;
            string sEvent;

            try
            {
                sSource = "MCWeb";//(new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.ProductName;
                sLog = "Application";
                sEvent = PMensaje;

                if (!EventLog.SourceExists(sSource))
                {
                    EventLog.CreateEventSource(sSource, sLog);
                }

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, pTipo, 234);
            }
            catch (Exception)
            {

            }

        }
    }
}