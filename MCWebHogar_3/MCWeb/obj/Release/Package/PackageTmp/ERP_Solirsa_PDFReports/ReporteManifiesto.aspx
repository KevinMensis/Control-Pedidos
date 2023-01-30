<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteManifiesto.aspx.cs" MasterPageFile="~/MenuPrincipal.Master" Inherits="MCWebHogar.ERP_Solirsa_PDFReports.ReporteManifiesto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Reporte manifiesto</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <div class="wrapper">
        <asp:HiddenField ID="HDF_IDManifest" runat="server" Value="0" Visible="true" />      
    </div>
</asp:Content>
