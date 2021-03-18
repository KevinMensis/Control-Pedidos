using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;
using Microsoft.VisualBasic;
using System.Data;
using LogicLyers.Servicios;
using CapaDatos;

namespace LogicLyers.Servicios
{
    public class GestorSQLserver
    {
        public static SqlConnection Conexion = new SqlConnection();
        public static SqlTransaction Transaccion;
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
                Fw.WriteLine("SISTEMA= EuroPagos"); //+ (new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.AssemblyName);
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
                sSource = "EuroPagos";//(new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.ProductName;
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
        public bool BeginTransaccion()
        {
            bool returnValue;
            returnValue = false;
            try
            {
                Transaccion = Conexion.BeginTransaction();
                returnValue = true;
            }
            catch (Exception ex)
            {
                EscribirLog(ex.ToString());
            }
            return returnValue;
        }
        public bool CommitTransaccion()
        {
            bool returnValue;
            returnValue = false;
            try
            {
                Transaccion.Commit();
                Transaccion = null;
                returnValue = true;
            }
            catch (Exception ex)
            {
                EscribirLog(ex.ToString());
            }
            return returnValue;
        }
        public bool RollbackTransaccion()
        {
            bool returnValue;
            returnValue = false;
            try
            {
                Transaccion.Rollback();
                Transaccion = null;
                returnValue = true;
            }
            catch (Exception ex)
            {
                EscribirLog(ex.ToString());
            }
            return returnValue;
        }

        public static bool EjecutarProcedimientoAlmacenado(string pNombre, SqlParameterCollection pParametros, ref DataTable pDt)
        {
            bool returnValue;
            SqlDataAdapter Da;
            SqlParameter Param;

            returnValue = false;
            try
            {
                Da = new SqlDataAdapter();

                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.CommandText = pNombre;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                Da.SelectCommand.Connection = Conexion;

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

                returnValue = true;
                //return pDt;

            }
            catch (Exception ex)
            {
                string Parametros;
                Parametros = "";
                if (pParametros != null)
                {
                    foreach (SqlParameter tempLoopVar_Param in pParametros)
                    {
                        Param = tempLoopVar_Param;
                        Parametros = Parametros + Param.ParameterName + "=" + Param.Value + " *** ";
                    }
                }

                EscribirLog(ex.ToString() + " " + pNombre + " " + Parametros);
            }

            return returnValue;
        }
        public static bool EjecutarProcedimientoAlmacenado2(string pNombre, ArrayList pParametros, ref DataTable pDt,SqlConnection conexion2)
        {
            bool returnValue;
            SqlDataAdapter Da;
            SqlParameter Param;
            //db.Desconectar(Conexion);
            //Conexion = db.Conectar();
            conexion2.Open();
            returnValue = false;
            try
            {
                Da = new SqlDataAdapter();

                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.CommandText = pNombre;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                Da.SelectCommand.Connection = conexion2;

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

                GestorSQLserver.EscribirLog(ex.ToString() + " " + pNombre + " " + Parametros);
            }
            return returnValue;
        }

   	
    }
}
