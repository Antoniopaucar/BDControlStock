<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmReportes.aspx.cs" Inherits="WebApplication1.frmReportes" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div>
    <asp:Button ID="btn_Rep_Mov_Fechas" runat="server" Text="REPORTE DE MOVIMIENTOS POR RANGO DE FECHAS" class="boton-reporte"/>
    <br />
    <asp:Button ID="btn_Rep_Prod_Sal" runat="server" Text="REPORTE DE PRODUCTOS CON MAS SALIDAS" class="boton-reporte"/>
    <br />
    <asp:Button ID="btn_Rep_Perdidas" runat="server" Text="REPORTE DE PERDIDAS" class="boton-reporte" OnClick="btn_Rep_Perdidas_Click"/>
    <br />
    <asp:Button ID="btn_Rep_Inv_Actual" runat="server" Text="REPORTE DE INVENTARIO ACTUAL" class="boton-reporte" />
</div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <rsweb:ReportViewer ID="ReportViewerPerdidas" runat="server" Width="100%" Height="600px" Visible="false" ProcessingMode="Local" KeepSessionAlive="false" SizeToReportContent="true">
        <LocalReport ReportPath="~/ReportePerdidas.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>


   
</asp:Content>
