<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetearContrasena.aspx.cs" Inherits="MCWebHogar.ResetearContrasena" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="Assets/img//apple-icon.png" />
    <link rel="icon" type="image/png" href="Assets/img//favicon.png" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Resetear Contraseña</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" rel="stylesheet" />
    <!-- CSS Files -->
    <link href="Assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/css/paper-dashboard.css?v=2.1.1" rel="stylesheet" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link href="Assets/demo/demo.css" rel="stylesheet" />
    <style type="text/css">
        body {
            overflow-y: auto;
        }
    </style>
    <!--   Core JS Files   -->
    <script src="Assets/js/core/jquery.min.js"></script>
    <script src="Assets/js/core/popper.min.js"></script>
    <script src="Assets/js/core/bootstrap.min.js"></script>
    <script src="Assets/js/plugins/perfect-scrollbar.jquery.min.js"></script>
    <script src="Assets/js/plugins/moment.min.js"></script>
    <!--  Plugin for Switches, full documentation here: http://www.jque.re/plugins/version3/bootstrap.switch/ -->
    <script src="Assets/js/plugins/bootstrap-switch.js"></script>
    <!--  Plugin for Sweet Alert -->
    <script src="Assets/js/plugins/sweetalert2.min.js"></script>
    <!-- Forms Validations Plugin -->
    <script src="Assets/js/plugins/jquery.validate.min.js"></script>
    <!--  Plugin for the Wizard, full documentation here: https://github.com/VinceG/twitter-bootstrap-wizard -->
    <script src="Assets/js/plugins/jquery.bootstrap-wizard.js"></script>
    <!--	Plugin for Select, full documentation here: http://silviomoreto.github.io/bootstrap-select -->
    <script src="Assets/js/plugins/bootstrap-selectpicker.js"></script>
    <!--  Plugin for the DateTimePicker, full documentation here: https://eonasdan.github.io/bootstrap-datetimepicker/ -->
    <script src="Assets/js/plugins/bootstrap-datetimepicker.js"></script>
    <!--  DataTables.net Plugin, full documentation here: https://datatables.net/    -->
    <script src="Assets/js/plugins/jquery.dataTables.min.js"></script>
    <!--	Plugin for Tags, full documentation here: https://github.com/bootstrap-tagsinput/bootstrap-tagsinputs  -->
    <script src="Assets/js/plugins/bootstrap-tagsinput.js"></script>
    <!-- Plugin for Fileupload, full documentation here: http://www.jasny.net/bootstrap/javascript/#fileinput -->
    <script src="Assets/js/plugins/jasny-bootstrap.min.js"></script>
    <!--  Full Calendar Plugin, full documentation here: https://github.com/fullcalendar/fullcalendar    -->
    <script src="Assets/js/plugins/fullcalendar/fullcalendar.min.js"></script>
    <script src="Assets/js/plugins/fullcalendar/daygrid.min.js"></script>
    <script src="Assets/js/plugins/fullcalendar/timegrid.min.js"></script>
    <script src="Assets/js/plugins/fullcalendar/interaction.min.js"></script>
    <!-- Vector Map plugin, full documentation here: http://jvectormap.com/documentation/ -->
    <script src="Assets/js/plugins/jquery-jvectormap.js"></script>
    <!--  Plugin for the Bootstrap Table -->
    <script src="Assets/js/plugins/nouislider.min.js"></script>
    <!--  Google Maps Plugin    -->
    <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE"></script>
    <!-- Chart JS -->
    <script src="Assets/js/plugins/chartjs.min.js"></script>
    <!--  Notifications Plugin    -->
    <script src="Assets/js/plugins/bootstrap-notify.js"></script>
    <!-- Control Center for Now Ui Dashboard: parallax effects, scripts for the example pages etc -->
    <script src="Assets/js/paper-dashboard.min.js?v=2.1.1" type="text/javascript"></script>
    <!-- Paper Dashboard DEMO methods, don't include it in your project! -->
    <script src="Assets/demo/demo.js"></script>
    <script src="Scripts/Alertify/alertify.min.js"></script>
    <link href="Styles/AlertifyCss/alertify.css" rel="stylesheet" />
    <link href="Styles/AlertifyCss/alertify.min.css" rel="stylesheet" />
    <link href="Styles/AlertifyCss/default.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            demo.checkFullPageBackgroundImage();
        });
        window.onscroll = function () {
            window.scrollTo(0, 0);
        }
        function alertifysuccess(msj) {
            alertify.notify(msj, 'success', 5, null);
            return false;
        }
        function alertifyerror(msj) {
            alertify.notify(msj, 'error', 5, null);
            return false;
        }
        function alertifywarning(msj) {
            alertify.notify(msj, 'warning', 5, null);
            return false;
        }
  </script>
</head>

<body class="login-page">
    <!-- Navbar -->

    <!-- End Navbar -->
    <div class="wrapper wrapper-full-page ">
        <div class="full-page section-image" data-image="Assets/img/bg/edificios-inteligentes1.jpg" style="width: 100%; height: 100%;">
            <!--   you can change the color of the filter page using: data-color="blue | purple | green | orange | red | rose " -->
            <div class="" style="padding-top: 30px;">
                <div class="container">
                    <img src="Assets/img/bg/logos.jpeg" title="Proyecto financiado por el Fondo para el Medio Ambiente Mundial (GEF), implementado por el Programa de las Naciones Unidas para el Medio Ambiente con el apoyo de la iniciativa global Unidos por la Eficiencia (U4E) y ejecutado por el Banco Centroamericano de Integración Económica (BCIE). La contraparte nacional del proyecto ha sido el Gobierno de la República de Costa Rica a través de su Ministerio de Ambiente y Energía." />
                    <h4 class="card-title" style="text-align: center; color: white">Bienvenido(a) a la herramienta de eficiencia energética del proyecto “Desarrollo de un mercado de eficiencia energética en iluminación, aires acondicionados y refrigeradores en Costa Rica”</h4>
                </div>
                <div class="container">
                    <div class="col-lg-4 col-md-6 ml-auto mr-auto">
                        <form class="form" runat="server">
                            <div class="card card-login">
                                <div class="card-body ">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="nc-icon nc-email-85"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="TXT_Correo" runat="server" CssClass="form-control" placeholder="Correo"></asp:TextBox>                                        
                                    </div>
                                    <asp:Button style="background-color: #94c733;" ID="BTN_ReenviarContrasena" runat="server" Text="Resetear Contraseña" 
                                        CssClass="btn btn-warning btn-round btn-block mb-3"  OnClick="BTN_ReenviarContrasena_Click">
                                    </asp:Button>
                                    <a href="Default.aspx">Ingresar</a>
                                    <a style="float: right;" href="Registrarse.aspx">Regístrate</a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</body>

</html>
