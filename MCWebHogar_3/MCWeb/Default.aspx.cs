using CapaLogica;
using CapaLogica.Servicios;
using System;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;

namespace MCWebHogar
{
    public partial class Default : System.Web.UI.Page
    {
        CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
        DataTable Result = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();            
        }
    }
}