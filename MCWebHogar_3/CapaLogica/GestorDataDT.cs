using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CapaLogica
{
    public class GestorDataDT
    {
        private DataTable DT = new DataTable();

        public GestorDataDT()
        {
            DT.Columns.Add("Variable");
            DT.Columns.Add("Valor");
            DT.Columns.Add("TipoValor");
            DT.Columns.Add("IMAGEN");
            DT.Columns["Imagen"].DataType = typeof(byte[]);
        }
        public DataTable DT1
        {
            get { return DT; }
            set { DT = value; }
        }

        public DataTable DTData()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Procedure");
                dt.Columns.Add("rpt");
                dt.Columns.Add("DataSet");
                dt.Columns.Add("DTName");

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
    }
}