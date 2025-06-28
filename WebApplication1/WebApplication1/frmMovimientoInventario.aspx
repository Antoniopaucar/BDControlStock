<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmMovimientoInventario.aspx.cs" Inherits="WebApplication1.frmMovimientoInventario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="tablaMovimientos" class="tablaForm" >
        <tr class="titulo">
            <td colspan="4">
                <asp:Label ID="Label2" runat="server" Text="Movimientos"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Numero:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_Id" runat="server" Enabled="False" Width="80px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label16" runat="server" Text="Buscar por Nro:"></asp:Label>
                <asp:TextBox ID="txt_Buscar" runat="server" Width="80px" TextMode="Number"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btn_Buscar" runat="server" Text="BUSCAR" class="btn btn-info" OnClick="btn_Buscar_Click"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label15" runat="server" Text="Fecha:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_Fecha" runat="server" CssClass="full-width-textbox" TextMode="Date"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Tipo de Movimiento:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="Ddl_Movimiento" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Movimiento_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Sub Tipo de Movimiento:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="Ddl_TipoMovimiento" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label13" runat="server" Text="Observaciones:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txt_Observaciones" runat="server" TextMode="MultiLine" Columns="35" Rows="4" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Usuario:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="Ddl_Usuario" runat="server" Enabled="False"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_Agregar" runat="server" Text="AGREGAR" 
                    OnClientClick="return confirm('¿Deseas agregar este registro?') && validarCamposTabla('tablaMovimientos','txt_Id,txt_Observaciones,txt_Buscar');" 
                    class="btn btn-exito" OnClick="btn_Agregar_Click" />
            </td>
            <td>
                <asp:Button ID="btn_Modificar" runat="server" Text="MODIFICAR" 
                    OnClientClick="return confirm('¿Deseas modificar esta registro?') && validarCamposTabla('tablaMovimientos','txt_Id,txt_Observaciones,txt_Buscar');" 
                    class="btn btn-primario" OnClick="btn_Modificar_Click" />
            </td>
            <td>
                <asp:Button ID="btn_Limpiar" runat="server" Text="LIMPIAR" class="btn btn-advertencia" OnClick="btn_Limpiar_Click" />
            </td>
            <td>
                <asp:Button ID="btn_Eliminar" runat="server" Text="ELIMINAR" 
                    OnClientClick="return confirm('¿Deseas eliminar este registro?');"
                    class="btn btn-peligro" OnClick="btn_Eliminar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table id="tablaDetalles" class="tablaForm">
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Producto:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label17" runat="server" Text="Cantidad:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" Placeholder="Cantidad"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Costo:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCosto" runat="server" Placeholder="Costo" Enabled="False"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>
               <asp:Button ID="btnAgregarFila" runat="server" Text="Agregar"
                    CssClass="btn btn-exito"
                    OnClick="btnAgregarFila_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDetalles_RowCommand" DataKeyNames="IdProducto" CssClass="gridview-style">
        <Columns>
            <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
            <asp:BoundField DataField="Costo" HeaderText="Costo" />
            <asp:TemplateField HeaderText="Accion">
                <ItemTemplate>
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                        CommandName="EliminarFila" 
                        CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-peligro"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>

