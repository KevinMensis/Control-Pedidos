<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrarse.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.Registrarse" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="Assets/img//apple-icon.png" />
    <link rel="icon" type="image/png" href="Assets/img//favicon.png" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Registrarse</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" rel="stylesheet" />
    <!-- CSS Files -->
    <link href="Assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/css/paper-dashboard.css?v=2.1.1" rel="stylesheet" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link href="Assets/demo/demo.css" rel="stylesheet" />

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
    <style type="text/css">
        body {
            overflow-y: auto;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            demo.checkFullPageBackgroundImage();
        });

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

        function activarloading() {
            document.getElementById('fade2').style.display = 'block';
            document.getElementById('modalloading').style.display = 'block';
        }

        function desactivarloading() {
            document.getElementById('fade2').style.display = 'none';
            document.getElementById('modalloading').style.display = 'none';
        }

        function validarCorreo() {
            var TXT_Correo = document.getElementById('<%= TXT_Correo.ClientID %>').value.trim()
            TXT_Correo = TXT_Correo.trim()
            if (/^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i.test(TXT_Correo)) {

            } else {
                alertifyerror("La dirección de correo es inválida.")
                desactivarloading()
                return false;
            }
        }

        function validarUsuario() {
            activarloading()
            var correcto = true
            var nombre = document.getElementById("<%= TXT_Nombre.ClientID %>").value.trim()
            var institucion = document.getElementById("<%= TXT_Institucion.ClientID %>").value.trim()
            var departamento = document.getElementById("<%= TXT_Departamento.ClientID %>").value.trim()
            var cargo = document.getElementById("<%= TXT_Cargo.ClientID %>").value.trim()
            var telefono = document.getElementById("<%= TXT_Telefono.ClientID %>").value.trim()
            var correo = document.getElementById("<%= TXT_Correo.ClientID %>").value.trim()

            if (correo.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('El Correo es obligatorio')
            } else {
                correcto = validarCorreo()
            }
            if (nombre.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('El Nombre es obligatorio')
            }
            if (institucion.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('La Institución es obligartoria')
            }
            if (departamento.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('El Departamento es obligatorio')
            }
            if (cargo.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('El Cargo es obligatorio')
            }
            if (telefono.length == 0) {
                correcto = false
                desactivarloading()
                alertifyerror('El Teléfono es obligatorio')
            }

            return correcto
        }

        function abrirModalInstituciones() {
            document.getElementById("btnpopInstituciones").click()
            return false;
        }

        function cerrarModalInstituciones() {
            document.getElementById("btnpopInstituciones").click()
            return false;
        }

        var htmlObject = ""
        function filtrarInstitucion(txt_busqueda) {
            activarloading()
            document.getElementById("<%= BTN_BuscarInstitucion.ClientID %>").click()
            htmlObject = txt_busqueda
        }

        function cargarValoresModal(categoria, nombre, codigo) {
            document.getElementById('Content_DGV_ListaInstituciones_DDL_Categoria').value = categoria
            document.getElementById('Content_DGV_ListaInstituciones_TXT_Institucion').value = nombre
            document.getElementById('Content_DGV_ListaInstituciones_TXT_Codigo').value = codigo
            if (htmlObject !== "") {
                document.getElementById(htmlObject.id).focus()
            }            
            desactivarloading()
            return false
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="modalloading" class="loading">
        <img src="images/cargando5.gif" width="100" height="100" />
    </div>
    <div id="fade2" class="overlayload"></div>
    <div id="divCompleto2" class="main container">
        <div class="full-page register-page section-image" data-image="Assets/img/bg/jan-sendereks.jpg">
            <div class="" style="padding-top: 30px;">
                <div class="container">
                    <img src="Assets/img/bg/logos.jpeg" title="Proyecto financiado por el Fondo para el Medio Ambiente Mundial (GEF), implementado por el Programa de las Naciones Unidas para el Medio Ambiente con el apoyo de la iniciativa global Unidos por la Eficiencia (U4E) y ejecutado por el Banco Centroamericano de Integración Económica (BCIE). La contraparte nacional del proyecto ha sido el Gobierno de la República de Costa Rica a través de su Ministerio de Ambiente y Energía." />
                    <h4 class="card-title" style="text-align: center; color: white">Bienvenido(a) a la herramienta de eficiencia energética del proyecto “Desarrollo de un mercado de eficiencia energética en iluminación, aires acondicionados y refrigeradores en Costa Rica”
                    </h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel_Formulario" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container">
                            <div class="row">
                                <div style="margin: 0 auto; text-align: center; display: block;" class="col-lg-5 col-md-6">
                                    <div class="card card-signup text-center">
                                        <div class="card-body ">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="nc-icon nc-single-02"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TXT_Nombre" runat="server" CssClass="form-control" placeholder="Nombre Completo"></asp:TextBox>
                                            </div>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="nc-icon nc-bank"></i>
                                                    </span>
                                                </div>
                                                <asp:HiddenField ID="HDF_IDInstitucion" runat="server" Value="0" visible="false" />
                                                <asp:TextBox ID="TXT_Institucion" onclick="abrirModalInstituciones();" onkeypress="abrirModalInstituciones();" runat="server" CssClass="form-control" placeholder="Institución"></asp:TextBox>
                                            </div>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="fa fa-id-card-o"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TXT_Departamento" runat="server" CssClass="form-control" placeholder="Departamento"></asp:TextBox>
                                            </div>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="fa fa-id-card-o"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TXT_Cargo" runat="server" CssClass="form-control" placeholder="Cargo"></asp:TextBox>
                                            </div>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="fa fa-phone"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TXT_Telefono" runat="server" CssClass="form-control" placeholder="Teléfono"></asp:TextBox>
                                            </div>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">
                                                        <i class="nc-icon nc-email-85"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TXT_Correo" runat="server" CssClass="form-control" placeholder="Correo"></asp:TextBox>
                                            </div>
                                            <asp:Button Style="background-color: #94c733;" ID="BTN_ActivarUsuario" runat="server" Text="Solicitar activación de usuario" CssClass="btn btn-info btn-round" OnClientClick="return validarUsuario();" OnClick="BTN_ActivarUsuario_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <button type="button" id="btnpopInstituciones" data-toggle="modal" data-target="#popInstituciones" style="visibility: hidden;">open</button>

    <div class="modal fade bd-example-modal-lg" id="popInstituciones" tabindex="-1" role="dialog" aria-labelledby="popInstituciones" aria-hidden="true">        
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2>Seleccionar Institución
                        <button type="button" class="close" aria-hidden="true" data-dismiss="modal">&times;</button>
                    </h2>
                </div>
                <div class="modal-body">
                    <div style="overflow: auto">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel_DGV_ListaInstituciones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="BTN_BuscarInstitucion" runat="server" OnClick="BTN_BuscarInstitucion_Click" AutoPostBack="false"></asp:LinkButton>
                                        <asp:GridView ID="DGV_ListaInstituciones" Width="100%" runat="server"
                                            AutoGenerateColumns="False" DataKeyNames="IDInstitucion,IDCategoriaInstitucion,DescripcionInstitucion,Codigo" 
                                            ShowHeaderWhenEmpty="true" Font-Size="11" BackColor="White" BorderColor="#DAD9D8" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="2" EmptyDataText="No hay registros."
                                            OnRowCommand="DGV_ListaInstituciones_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button class="btn btn-primary" ID="BTN_SeleccionarInstitucion" runat="server"
                                                            CommandName="Elegir"
                                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                            Text="Elegir"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_Categoria" runat="server" Text="Categoria"></asp:Label>
                                                        <asp:TextBox CssClass="form-control" ID="DDL_Categoria" runat="server" placeholder="Buscar..." onkeydown="filtrarInstitucion( this );" AutoPostBack="false"></asp:TextBox>
                                                        <%--<asp:DropDownList CssClass="form-control" ID="DDL_Categoria" runat="server" OnSelectedIndexChanged="BTN_BuscarInstitucion_Click"></asp:DropDownList>--%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Categoria1" runat="server" Text='<%#Eval("DescripcionCategoriaInstitucion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                    
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_Institucion" runat="server" Text="Nombre"></asp:Label>
                                                        <asp:TextBox CssClass="form-control" ID="TXT_Institucion" runat="server" placeholder="Buscar..." onkeydown="filtrarInstitucion( this );" AutoPostBack="false"></asp:TextBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Institucion1" runat="server" Text='<%#Eval("DescripcionInstitucion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Lbl_Codigo" runat="server" Text="Código"></asp:Label>
                                                        <asp:TextBox CssClass="form-control" ID="TXT_Codigo" runat="server" placeholder="Buscar..." onkeydown="filtrarInstitucion( this );" AutoPostBack="false"></asp:TextBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_Codigo1" runat="server" Text='<%#Eval("Codigo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>