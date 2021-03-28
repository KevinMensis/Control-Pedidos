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
            if (Session["mensaje"] != null && Session["correo"] != null)
            {
                string mensaje = Session["mensaje"].ToString().Trim();
                string correo = Session["correo"].ToString().Trim();
                ViewState["contrasena"] = Session["contrasena"].ToString().Trim();
                string asignarRol = "";
                if (mensaje == "Se ha activado correctamente el usuario:")
                {
                    cargarDDL();
                    // TXT_CorreoUsuario.Text = correo;
                    // UpdatePanel_ModalActivarUsuario.Update();
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivacionUsuario", "abrirModalActivarUsuario();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptActivacionUsuario", "alert('" + mensaje + " " + correo + asignarRol + "');", true);
                }                
            }
            Session.RemoveAll();
        }

        private void cargarDDL()
        {
            DataTable DT_Roles = new DataTable();
            DT_Roles.Columns.Add("IDRol");
            DT_Roles.Columns.Add("DescripcionRol");

            DT_Roles.Rows.Add(0, "Seleccione");
            DT_Roles.Rows.Add(1, "Administrador del Sistema");
            DT_Roles.Rows.Add(2, "MINAE");
            DT_Roles.Rows.Add(3, "Coordinador de PGAI");
            DT_Roles.Rows.Add(4, "General");

            //DDL_Rol.DataSource = DT_Roles;
            //DDL_Rol.DataValueField = "IDRol";
            //DDL_Rol.DataTextField = "DescripcionRol";
            //DDL_Rol.DataBind();
        }

//        private bool enviarCorreoActivacion(string contrasena)
//        {
//            try
//            {
//                string cadena = HttpContext.Current.Request.Url.AbsoluteUri;

//                MailMessage msg = new MailMessage();

//                string destinatario = TXT_CorreoUsuario.Text.Trim();
//                msg.To.Add(new MailAddress(destinatario));

//                string BBC = "kevin@mensis.cr";
//                msg.Bcc.Add(new MailAddress(BBC));
//                BBC = "dviquez@minae.go.cr";
//                msg.Bcc.Add(new MailAddress(BBC));
//                BBC = "jrdomenech@yahoo.es";
//                msg.Bcc.Add(new MailAddress(BBC));

//                msg.From = new MailAddress(TXTMailUser, "Activación Usuario");
//                msg.Subject = "Activación de Usuario";
                
//                string mensaje = "";

//                mensaje =
//@"Estimado(a) usuario(a),<br />
//<br />
//Su solicitud de acceso a la herramienta de estimación de ahorros y beneficios ambientales en eficiencia energética ha sido aceptada.<br />
//<br />
//Usuario: <a href='' style='color:blue;'>" + TXT_CorreoUsuario.Text.Trim() + @"</a><br />
//Contraseña: <a href='' style='color:blue;'>" + contrasena + @"</a><br />
//<br />
//Puede accesar a la herramienta en el siguiente enlace: " + cadena + @"<br />
//<br />
//Muchas gracias y saludos cordiales.";

//                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensaje, null, MediaTypeNames.Text.Html);
//                msg.AlternateViews.Add(htmlView);

//                var client = new SmtpClient(TXTServerMail.Trim(), 587)
//                {
//                    Credentials = new NetworkCredential(TXTMailUser.Trim(), TXTContrasena.Trim()),
//                    EnableSsl = true
//                };

//                client.Send(msg);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//                throw ex;
//            }
//        }

        //protected void BTN_ActivacionUsuario_Click(object sender, EventArgs e)
        //{
        //    if (DDL_Rol.SelectedValue != "0")
        //    {
        //        DT.DT1.Clear();
        //        DT.DT1.Rows.Add("@Correo", TXT_CorreoUsuario.Text, SqlDbType.VarChar);
        //        DT.DT1.Rows.Add("@RolID", DDL_Rol.SelectedValue, SqlDbType.VarChar);
        //        DT.DT1.Rows.Add("@Activo", 1, SqlDbType.Int);
        //        DT.DT1.Rows.Add("@Token", ViewState["contrasena"].ToString().Trim(), SqlDbType.VarChar);

        //        DT.DT1.Rows.Add("@Usuario", "", SqlDbType.VarChar);
        //        DT.DT1.Rows.Add("@TipoSentencia", "ActivarUsuario", SqlDbType.VarChar);

        //        Result = CapaLogica.GestorDatos.Consultar(DT.DT1, "EE03_0001");
        //        string contrasena = "";
        //        if (Result != null && Result.Rows.Count > 0)
        //        {
        //            if (Result.Rows[0][0].ToString().Trim() == "ERROR")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ConfirmarEditar_ClickIncorrecta", "alert('" + Result.Rows[0][1].ToString().Trim() + "');", true);
        //                return;
        //            }
        //            else
        //            {
        //                contrasena = Result.Rows[0][1].ToString().Trim();
        //            }
        //        }
        //        enviarCorreoActivacion(contrasena);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ActivacionUsuario_Click", "cerrarModalActivarUsuario();alertifysuccess('Se le ha notificado al usuario la activación de su cuenta.')", true);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerScriptBTN_ActivacionUsuario_Click", "alertifywarning('Por favor, seleccione un rol para el nuevo usuario.');", true);
        //    }
        //}

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
                            Environment.SetEnvironmentVariable("Printer", "Microsoft Print to PDF");
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