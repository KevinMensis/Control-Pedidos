using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaDatos;
using CapaLogica;
using CapaLogica.Servicios;
using System.Web.Configuration;
using System.Web.Services;

namespace MCWebHogar
{
    // alert('Se ha producido un error al traer las imágenes'); para poner error en scrip de imagenes
    public partial class MenuPrincipal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserDB DB = new UserDB(WebConfigurationManager.ConnectionStrings["db_a8c525_solirsabakup"].ConnectionString, "db_a8c525_solirsabakup");
                GestorAccess.Conectividad(DB);
            }
        }
    }
}