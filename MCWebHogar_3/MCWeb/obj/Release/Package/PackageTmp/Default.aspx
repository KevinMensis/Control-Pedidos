<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Inicio de sesión</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" rel="stylesheet" />
    <!-- CSS Files -->
    <link href="Assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/css/paper-dashboard.css?v=2.1.1" rel="stylesheet" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link href="Assets/demo/demo.css" rel="stylesheet" />
    <!-- Custom fonts for this template-->
    <link href="Assets/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

    <%--<!-- Custom styles for this template-->
    <link href="Assets/css/sb-admin-2.min.css" rel="stylesheet" />--%>

    <script src="/Scripts/Alertify/alertify.min.js"></script>
    <link href="/Styles/AlertifyCss/alertify.css" rel="stylesheet" />
    <link href="/Styles/AlertifyCss/alertify.min.css" rel="stylesheet" />
    <link href="/Styles/AlertifyCss/default.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function success(msj) {
            alertify.notify(msj, 'success', 5, null);
        }

        function error(msj) {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error(msj, 6, null);
        }

        function warning(msj) {
            alertify.notify(msj, 'warning', 5, null);
        }

        function alertifysuccess(msj) {
            alertify.notify(msj, 'success', 5, null);
            return false
        }

        function alertifyerror(msj) {
            alertify.notify(msj, 'error', 5, null);
            return false
        }

        function alertifywarning(msj) {
            alertify.notify(msj, 'warning', 5, null);
            return false
        }
    </script>
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
    <script>
        //$(document).ready(function () {
        //    demo.checkFullPageBackgroundImage();
        //});
        //window.onscroll = function () {
        //    window.scrollTo(0, 0);
        //}
    </script>
    <style>
        .bg-login-image {
            background: url(https://image.freepik.com/vector-gratis/fachada-cafeteria-diseno-plano_23-2147560163.jpg);
            background-position: center;
            background-size: cover;
        }

        .card-body {
            flex: 1 1 auto;
            min-height: 1px;
            padding: 1.25rem;
        }

        .row {
            display: flex;
            flex-wrap: wrap;
            margin-right: -.75rem;
            margin-left: -.75rem;
            width: auto;
            margin: 15px auto;
        }

        .col, .col-1, .col-10, .col-11, .col-12, .col-2, .col-3, .col-4, .col-5, .col-6, .col-7, .col-8, .col-9, .col-auto, .col-lg, .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-auto, .col-md, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-auto, .col-sm, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-auto, .col-xl, .col-xl-1, .col-xl-10, .col-xl-11, .col-xl-12, .col-xl-2, .col-xl-3, .col-xl-4, .col-xl-5, .col-xl-6, .col-xl-7, .col-xl-8, .col-xl-9, .col-xl-auto {
            position: relative;
            width: 100%;
            padding-right: .75rem;
            padding-left: .75rem;
        }

        .container, .container-fluid, .container-lg, .container-md, .container-sm, .container-xl {
            padding-left: 1.5rem;
            padding-right: 1.5rem;
        }

        .justify-content-center {
            justify-content: center!important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-xl-10 col-lg-12 col-md-9">
                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0">
                        <!-- Nested Row within Card Body -->
                        <div class="row">
                            <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                            <div class="col-lg-6">
                                <div class="p-5">
                                    <div class="text-center">
                                        <h1 class="h4 text-gray-900 mb-4">Bienvenido a Mi K-Fe!</h1>
                                    </div>
                                    <div class="user">
                                        <div class="form-group">
                                            <input type="text" class="form-control form-control-user" placeholder="Ingrese su correo o usuario" id="TXT_Usuario" name="TXT_Usuario" aria-describedby="emailHelp" />
                                        </div>
                                        <div class="form-group">
                                            <input type="password" class="form-control form-control-user" placeholder="Ingrese su contraseña" id="TXT_Contrasena" name="TXT_Contrasena" />
                                        </div>
                               
                                        <asp:Button ID="btn_login" runat="server" Text="Iniciar sesión" CssClass="btn btn-primary btn-user btn-block" OnClick="btn_login_Click" />
                                        <hr/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--<button type="button" id="BTN_ModalActivarUsuario" data-toggle="modal" data-target="#ModalActivarUsuario" style="visibility: visible;">open</button>

    <div class="modal fade bd-example-modal-sm" id="ModalActivarUsuario" tabindex="-1" role="dialog" aria-labelledby="popModalActivarUsuario" aria-hidden="false">
        <asp:UpdatePanel ID="UpdatePanel_ModalActivarUsuario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Activación de Usuarios</h5>
                        </div>
                        <div class="modal-body">
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label for="TXT_CorreoUsuario">Se ha activado correctamente el usuario</label>
                                    <asp:TextBox ID="TXT_CorreoUsuario" runat="server" CssClass="form-control" Enabled="false" value=""></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                    <label for="DDL_Rol">Por favor, asigne el rol correspondiente</label>
                                    <asp:DropDownList ID="DDL_Rol" class="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="border-top: none;">
                            <asp:Button ID="BTN_ActivacionUsuario" runat="server" Text="Activar" CssClass="btn btn-success" OnClientClick="return validarActivacionUsuario();" OnClick="BTN_ActivacionUsuario_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>
</asp:Content>
