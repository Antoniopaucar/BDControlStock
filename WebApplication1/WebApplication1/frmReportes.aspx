<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmReportes.aspx.cs" Inherits="WebApplication1.frmReportes" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div>
    <asp:Button ID="btn_Rep_Mov_Fechas" runat="server" Text="REPORTE DE MOVIMIENTOS POR RANGO DE FECHAS" class="boton-reporte" OnClick="btn_Rep_Mov_Fechas_Click"/>
    <br />
    <asp:Button ID="btn_Rep_Prod_Sal" runat="server" Text="REPORTE DE PRODUCTOS CON MAS SALIDAS" class="boton-reporte" OnClick="btn_Rep_Prod_Sal_Click"/>
    <br />
    <asp:Button ID="btn_Rep_Perdidas" runat="server" Text="REPORTE DE PERDIDAS" class="boton-reporte" OnClick="btn_Rep_Perdidas_Click"/>
    <br />
    <asp:Button ID="btn_Rep_Inv_Actual" runat="server" Text="REPORTE DE INVENTARIO ACTUAL" class="boton-reporte" OnClick="btn_Rep_Inv_Actual_Click" />
</div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">


<asp:Panel ID="pnlReportes" runat="server" Visible="false">

        <rsweb:ReportViewer ID="ReportViewerPerdidas" runat="server"
                             Width="100%" Height="565px"
                             ProcessingMode="Local" KeepSessionAlive="false"
                             SizeToReportContent="true">
            <LocalReport ReportPath="~/ReportePerdidas.rdlc" />
        </rsweb:ReportViewer>

        <rsweb:ReportViewer ID="ReportViewerInventario" runat="server"
                             Width="100%" Height="600px"
                             ProcessingMode="Local" KeepSessionAlive="false"
                             SizeToReportContent="true" Visible="false">
            <LocalReport ReportPath="~/ReporteInventario.rdlc" />
        </rsweb:ReportViewer>

</asp:Panel>
<asp:Panel ID="pnlGrillas" runat="server" Visible="true">

<div class="filtro-fechas">
    <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date"
                 CssClass="full-width-textbox" placeholder="Desde…" />

    <asp:TextBox ID="txtFechaFin" runat="server" TextMode="Date"
                 CssClass="full-width-textbox" placeholder="Hasta…" />

    <asp:Button  ID="btnFiltrar" runat="server" Text="FILTRAR"
                 CssClass="btn btn-info"
                 OnClick="btnFiltrar_Click" />

    <asp:Label   ID="lblMensaje" runat="server"
                 ForeColor="Red" CssClass="mt-2" />
</div>
<div style="max-height:300px;overflow:auto;">
    <asp:GridView ID="gvMovimientos" runat="server"
                  AutoGenerateColumns="False"
                  CssClass="gridview-style"
                  AllowPaging="True" PageSize="10"
                  Visible="false"
                  OnPageIndexChanging="gvMovimientos_PageIndexChanging">

        <Columns>
            <asp:BoundField DataField="IdMovimiento" HeaderText="N.º Mov." />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="IdTipoMovimiento" HeaderText="Tipo" />
            <asp:BoundField DataField="IdDetalleTipoMovimiento" HeaderText="Detalle" />
            <asp:BoundField DataField="IdProducto" HeaderText="Producto" />
            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad"
                            DataFormatString="{0:N2}" />
            <asp:BoundField DataField="Costo" HeaderText="Costo U."
                            DataFormatString="{0:C}" />
            <asp:BoundField DataField="IdUsuario" HeaderText="Usuario" />
        </Columns>
    </asp:GridView>
    <asp:GridView ID="gvProductosMasSalidas"
              runat="server"
              CssClass="gridview-style"
              AutoGenerateColumns="False"
              AllowPaging="True" PageSize="5"
              Visible="false"
              OnPageIndexChanging="gvProdMasSalidas_PageIndexChanging">

    <Columns>
        <asp:BoundField DataField="IdProducto"     HeaderText="ID" />
        <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
        <asp:BoundField DataField="TotalSalidas"   HeaderText="Salidas"
                        DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
    </Columns>
</asp:GridView>
</div>
</asp:Panel>
</asp:Content>
