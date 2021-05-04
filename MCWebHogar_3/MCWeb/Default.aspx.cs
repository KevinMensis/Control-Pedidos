using CapaLogica;
using CapaLogica.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void btn_login_Click(object sender, EventArgs e)
        {
            UserDB DB = new UserDB(WebConfigurationManager.ConnectionStrings["DB_A6F48F_mikfepz"].ConnectionString, "DB_A6F48F_mikfepz_admin");
            GestorAccess.Conectividad(DB);

            string mail = Request.Form["TXT_Usuario"];
            string pwd = Request.Form["TXT_Contrasena"]; 
            string IP = Request.ServerVariables["REMOTE_ADDR"];
            
            try
            {
                CapaLogica.GestorDataDT DT = new CapaLogica.GestorDataDT();
                DataTable Result = new DataTable();

                DT.DT1.Rows.Add("@CorreoUsuario", mail, SqlDbType.VarChar);
                DT.DT1.Rows.Add("@Contrasena", pwd, SqlDbType.VarChar);

                DT.DT1.Rows.Add("@Usuario","", SqlDbType.VarChar);
                DT.DT1.Rows.Add("@TipoSentencia", "Ingresar", SqlDbType.VarChar);

                Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "CP00_0001");
                
                if (Result != null && Result.Rows.Count > 0)
                {
                    if (Result.Rows[0][0].ToString().Trim() == "ERROR")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptLoginIncorrect", "alert('" + Result.Rows[0][1].ToString().Trim() + "');", true);
                        return;
                    }
                    else
                    {
                        if (Result.Rows[0][0].ToString().Trim() != "")
                        {
                            Session["Printer"] = Result.Rows[0][15].ToString().Trim();
                            Session["UserId"] = Result.Rows[0][0].ToString().Trim();
                            Session["Usuario"] = Result.Rows[0][4].ToString().Trim();
                            Response.Redirect("ControlPedidos/Pedido.aspx", true);
                        }
                    }
                }
                else if (Result != null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                string Script = "alertifyerror('Ocurrio un error. Descripción: " + ex.Message + ".');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", Script, true);
            }           
        }
    }
}