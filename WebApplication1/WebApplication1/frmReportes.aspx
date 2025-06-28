<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmReportes.aspx.cs" Inherits="WebApplication1.frmReportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div>
    <asp:Button ID="btn_Rep_Mov_Fechas" runat="server" Text="REPORTE DE MOVIMIENTOS POR RANGO DE FECHAS" class="boton-reporte"/>
    <br />
    <asp:Button ID="btn_Rep_Prod_Sal" runat="server" Text="REPORTE DE PRODUCTOS CON MAS SALIDAS" class="boton-reporte"/>
    <br />
    <asp:Button ID="btn_Rep_Perdidas" runat="server" Text="REPORTE DE PERDIDAS" class="boton-reporte"/>
    <br />
    <asp:Button ID="btn_Rep_Inv_Actual" runat="server" Text="REPORTE DE INVENTARIO ACTUAL" class="boton-reporte" />
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>
</asp:Content>
